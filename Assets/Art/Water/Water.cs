namespace UnityStandardAssets.Water
{
    using System.Collections.Generic;
    using UnityEngine;
    [ExecuteInEditMode]
    public class Water : MonoBehaviour
    {
        public enum WaterMode { Simple, Reflective, Refractive };
        public WaterMode waterMode = WaterMode.Refractive;
        public bool disablePixelLights = true;
        public int textureSize = 256;
        public float clipPlaneOffset = 0.07f;
        public LayerMask reflectLayers = -1, refractLayers = -1;
        private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>(), m_RefractionCameras = new Dictionary<Camera, Camera>();
        private RenderTexture m_ReflectionTexture = null, m_RefractionTexture = null;
        private WaterMode m_HardwareWaterSupport = WaterMode.Refractive;
        private int m_OldReflectionTextureSize = 0, m_OldRefractionTextureSize = 0;
        private static bool s_InsideWater = false;
        public void OnWillRenderObject()
        {
            if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial || !GetComponent<Renderer>().enabled)
                return;
            Camera cam = Camera.current;
            if (!cam || s_InsideWater)
                return;
            s_InsideWater = true;
            m_HardwareWaterSupport = FindHardwareWaterSupport();
            WaterMode mode = GetWaterMode;
            Camera reflectionCamera, refractionCamera;
            CreateWaterObjects(cam, out reflectionCamera, out refractionCamera);
            Vector3 pos = transform.position, normal = transform.up;
            int oldPixelLightCount = QualitySettings.pixelLightCount;
            if (disablePixelLights)
                QualitySettings.pixelLightCount = 0;
            UpdateCameraModes(cam, reflectionCamera);
            UpdateCameraModes(cam, refractionCamera);
            if (WaterMode.Simple != mode)
            {
                float d = -Vector3.Dot(normal, pos) - clipPlaneOffset;
                Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);
                Matrix4x4 reflection = Matrix4x4.zero;
                CalculateReflectionMatrix(ref reflection, reflectionPlane);
                Vector3 oldpos = cam.transform.position, newpos = reflection.MultiplyPoint(oldpos);
                reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;
                Vector4 clipPlane = CameraSpacePlane(reflectionCamera, pos, normal, 1.0f);
                reflectionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);
                reflectionCamera.cullingMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;
                reflectionCamera.cullingMask = ~(1 << 4) & reflectLayers.value;
                reflectionCamera.targetTexture = m_ReflectionTexture;
                bool oldCulling = GL.invertCulling;
                GL.invertCulling = !oldCulling;
                reflectionCamera.transform.position = newpos;
                Vector3 euler = cam.transform.eulerAngles;
                reflectionCamera.transform.eulerAngles = new Vector3(-euler.x, euler.y, euler.z);
                reflectionCamera.Render();
                reflectionCamera.transform.position = oldpos;
                GL.invertCulling = oldCulling;
                GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", m_ReflectionTexture);
                if (WaterMode.Refractive == mode)
                {
                    refractionCamera.worldToCameraMatrix = cam.worldToCameraMatrix;
                    clipPlane = CameraSpacePlane(refractionCamera, pos, normal, -1.0f);
                    refractionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);
                    refractionCamera.cullingMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;
                    refractionCamera.cullingMask = ~(1 << 4) & refractLayers.value;
                    refractionCamera.targetTexture = m_RefractionTexture;
                    refractionCamera.transform.position = cam.transform.position;
                    refractionCamera.transform.rotation = cam.transform.rotation;
                    refractionCamera.Render();
                    GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", m_RefractionTexture);
                }
            }
            if (disablePixelLights)
                QualitySettings.pixelLightCount = oldPixelLightCount;
            switch (mode)
            {
                case WaterMode.Simple:
                    Shader.EnableKeyword("WATER_SIMPLE");
                    Shader.DisableKeyword("WATER_REFLECTIVE");
                    Shader.DisableKeyword("WATER_REFRACTIVE");
                    break;
                case WaterMode.Reflective:
                    Shader.DisableKeyword("WATER_SIMPLE");
                    Shader.EnableKeyword("WATER_REFLECTIVE");
                    Shader.DisableKeyword("WATER_REFRACTIVE");
                    break;
                case WaterMode.Refractive:
                    Shader.DisableKeyword("WATER_SIMPLE");
                    Shader.DisableKeyword("WATER_REFLECTIVE");
                    Shader.EnableKeyword("WATER_REFRACTIVE");
                    break;
            }
            s_InsideWater = false;
        }
        private void OnDisable()
        {
            if (m_ReflectionTexture)
            {
                DestroyImmediate(m_ReflectionTexture);
                m_ReflectionTexture = null;
            }
            if (m_RefractionTexture)
            {
                DestroyImmediate(m_RefractionTexture);
                m_RefractionTexture = null;
            }
            foreach (var kvp in m_ReflectionCameras)
                DestroyImmediate((kvp.Value).gameObject);
            m_ReflectionCameras.Clear();
            foreach (var kvp in m_RefractionCameras)
                DestroyImmediate((kvp.Value).gameObject);
            m_RefractionCameras.Clear();
        }
        private void Update()
        {
            if (!GetComponent<Renderer>())
                return;
            Material mat = GetComponent<Renderer>().sharedMaterial;
            if (!mat)
                return;
            Vector4 waveSpeed = mat.GetVector("WaveSpeed");
            float waveScale = mat.GetFloat("_WaveScale");
            Vector4 waveScale4 = new Vector4(waveScale, waveScale, waveScale * 0.4f, waveScale * 0.45f);
            double t = (Time.timeSinceLevelLoad * 0.05) * waveScale;
            Vector4 offsetClamped = new Vector4(
                (float)System.Math.IEEERemainder(waveSpeed.x * t, 1.0),
                (float)System.Math.IEEERemainder(waveSpeed.y * t, 1.0),
                (float)System.Math.IEEERemainder(waveSpeed.z * (t * 0.4), 1.0),
                (float)System.Math.IEEERemainder(waveSpeed.w * (t * 0.45), 1.0)
                );
            mat.SetVector("_WaveOffset", offsetClamped);
            mat.SetVector("_WaveScale4", waveScale4);
        }
        private void UpdateCameraModes(Camera src, Camera dest)
        {
            if (null == dest)
                return;
            dest.clearFlags = src.clearFlags;
            dest.backgroundColor = src.backgroundColor;
            if (CameraClearFlags.Skybox == src.clearFlags)
            {
                Skybox sky = src.GetComponent<Skybox>(), mysky = dest.GetComponent<Skybox>();
                if (sky && sky.material)
                {
                    mysky.enabled = true;
                    mysky.material = sky.material;
                }
                else
                    mysky.enabled = false;
            }
            dest.farClipPlane = src.farClipPlane;
            dest.nearClipPlane = src.nearClipPlane;
            dest.orthographic = src.orthographic;
            dest.fieldOfView = src.fieldOfView;
            dest.aspect = src.aspect;
            dest.orthographicSize = src.orthographicSize;
        }
        private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
        {
            WaterMode mode = GetWaterMode;
            reflectionCamera = null;
            refractionCamera = null;
            if (WaterMode.Simple != mode)
            {
                if (!m_ReflectionTexture || m_OldReflectionTextureSize != textureSize)
                {
                    if (m_ReflectionTexture)
                        DestroyImmediate(m_ReflectionTexture);
                    m_ReflectionTexture = new RenderTexture(textureSize, textureSize, 16)
                    {
                        name = "__WaterReflection" + GetInstanceID(),
                        isPowerOfTwo = true,
                        hideFlags = HideFlags.DontSave
                    };
                    m_OldReflectionTextureSize = textureSize;
                }
                m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
                if (!reflectionCamera)
                {
                    GameObject go = new GameObject("Water Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
                    reflectionCamera = go.GetComponent<Camera>();
                    reflectionCamera.enabled = false;
                    reflectionCamera.transform.position = transform.position;
                    reflectionCamera.transform.rotation = transform.rotation;
                    reflectionCamera.gameObject.AddComponent<FlareLayer>();
                    go.hideFlags = HideFlags.HideAndDontSave;
                    m_ReflectionCameras[currentCamera] = reflectionCamera;
                }
                if (WaterMode.Refractive == mode)
                {
                    if (!m_RefractionTexture || m_OldRefractionTextureSize != textureSize)
                    {
                        if (m_RefractionTexture)
                            DestroyImmediate(m_RefractionTexture);
                        m_RefractionTexture = new RenderTexture(textureSize, textureSize, 16)
                        {
                            name = "__WaterRefraction" + GetInstanceID(),
                            isPowerOfTwo = true,
                            hideFlags = HideFlags.DontSave
                        };
                        m_OldRefractionTextureSize = textureSize;
                    }
                    m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
                    if (!refractionCamera)
                    {
                        GameObject go = new GameObject("Water Refr Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
                        refractionCamera = go.GetComponent<Camera>();
                        refractionCamera.enabled = false;
                        refractionCamera.transform.position = transform.position;
                        refractionCamera.transform.rotation = transform.rotation;
                        refractionCamera.gameObject.AddComponent<FlareLayer>();
                        go.hideFlags = HideFlags.HideAndDontSave;
                        m_RefractionCameras[currentCamera] = refractionCamera;
                    }
                }
            }
        }
        private WaterMode GetWaterMode => (m_HardwareWaterSupport < waterMode) ? m_HardwareWaterSupport : waterMode;
        private WaterMode FindHardwareWaterSupport()
        {
            if (GetComponent<Renderer>())
            {
                Material mat = GetComponent<Renderer>().sharedMaterial;
                if (mat)

                {
                    string mode = mat.GetTag("WATERMODE", false);
                    if ("Refractive" == mode)
                        return WaterMode.Refractive;
                    if ("Reflective" == mode)
                        return WaterMode.Reflective;
                }
            }
            return WaterMode.Simple;
        }
        private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
        {
            Matrix4x4 m = cam.worldToCameraMatrix;
            pos = m.MultiplyPoint(pos + normal * clipPlaneOffset);
            normal = m.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(pos, normal));
        }
        private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1.0f - 2.0f * plane.x * plane.x);
            reflectionMat.m01 = (-2.0f * plane.x * plane.y);
            reflectionMat.m02 = (-2.0f * plane.x * plane.z);
            reflectionMat.m03 = (-2.0f * plane.x * plane.w);
            reflectionMat.m10 = (-2.0f * plane.y * plane.x);
            reflectionMat.m11 = (1.0f - 2.0f * plane.y * plane.y);
            reflectionMat.m12 = (-2.0f * plane.y * plane.z);
            reflectionMat.m13 = (-2.0f * plane.y * plane.w);
            reflectionMat.m20 = (-2.0f * plane.z * plane.x);
            reflectionMat.m21 = (-2.0f * plane.z * plane.y);
            reflectionMat.m22 = (1.0f - 2.0f * plane.z * plane.z);
            reflectionMat.m23 = (-2.0f * plane.z * plane.w);
            reflectionMat.m30 = 0.0f;
            reflectionMat.m31 = 0.0f;
            reflectionMat.m32 = 0.0f;
            reflectionMat.m33 = 1.0f;
        }
    }
}