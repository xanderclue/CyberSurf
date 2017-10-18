using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

public class EffectEditor : MaterialEditor
{
    bool find(string[] array, string key)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == key)
            {
                return true;
            }
        }
        return false;
    }

    public override void OnInspectorGUI()
    {
        if(!isVisible) { return; }

        Material material = target as Material;

        MaterialProperty[] properties = GetMaterialProperties(targets);

        string[] keys = material.shaderKeywords;

        bool effectLayer1Enabled = find(keys, "EFFECTLAYER1ON");
        bool effectLayer2Enabled = find(keys, "EFFECTLAYER2ON");
        bool effectLayer3Enabled = find(keys, "EFFECTLAYER3ON");

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < 3; i++)
        {
            TexturePropertySingleLine(new GUIContent(properties[i].displayName), properties[i]);
        }
        FloatProperty(properties[3], "Alpha");
        FloatProperty(properties[4], "Main Tex X Scroll Speed");
        FloatProperty(properties[5], "Main Tex Y Scroll Speed");

            EditorGUILayout.Separator();

            effectLayer1Enabled = EditorGUILayout.Toggle("Effect Layer 1", effectLayer1Enabled);
            if (effectLayer1Enabled)
            {
                DrawEffectsLayer(properties, 1);
            }

            effectLayer2Enabled = EditorGUILayout.Toggle("Effect Layer 2", effectLayer2Enabled);
            if (effectLayer2Enabled)
            {
                DrawEffectsLayer(properties, 2);
        }
        effectLayer3Enabled = EditorGUILayout.Toggle("Effect Layer 3", effectLayer3Enabled);
        if (effectLayer3Enabled)
        {
            DrawEffectsLayer(properties, 3);
        }


        if (EditorGUI.EndChangeCheck())
            {
                string[] newKeys = new string[] {
                effectLayer1Enabled ? "EFFECTLAYER1ON" : "EFFECTLAYER1OFF",
                effectLayer2Enabled ? "EFFECTLAYER2ON" : "EFFECTLAYER2OFF",
                effectLayer3Enabled ? "EFFECTLAYER3ON" : "EFFECTLAYER3OFF",
            };

                material.shaderKeywords = newKeys;
                EditorUtility.SetDirty(material);
            }
        
    }
    public MaterialProperty GetByName(MaterialProperty[] properties, string name)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].name == name)
                return properties[i];
        }

        return null;
    }

    void DrawEffectsLayer(MaterialProperty[] properties, int layer)
    {
        GUIStyle style = EditorStyles.helpBox;
        style.margin = new RectOffset(20, 20, 0, 0);

        EditorGUILayout.BeginVertical(style);
        {
            TexturePropertySingleLine(new GUIContent("Effect Texture"), GetByName(properties, EffectName(layer, "Tex")));
            TexturePropertySingleLine(new GUIContent("Motion Texture"), GetByName(properties, EffectName(layer, "Motion")));

            ColorProperty(GetByName(properties, EffectName(layer, "Color")), "Tint Color");

            FloatProperty(GetByName(properties, EffectName(layer, "MotionSpeedYAxis")), "Motion Speed Y Axis");
            FloatProperty(GetByName(properties, EffectName(layer, "MotionSpeedXAxis")), "Motion Speed X Axis");
            FloatProperty(GetByName(properties, EffectName(layer, "ScrollSpeedXAxis")), "Scroll Speed X Axis");
            FloatProperty(GetByName(properties, EffectName(layer, "ScrollSpeedYAxis")), "Scroll Speed Y Axis");
            FloatProperty(GetByName(properties, EffectName(layer, "Rotation")), "Rotation Speed");

            Vector4 translation = GetByName(properties, EffectName(layer, "Translation")).vectorValue;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Positon");
                translation.x = EditorGUILayout.FloatField(translation.x);
                translation.y = EditorGUILayout.FloatField(translation.y);
            }
            EditorGUILayout.EndHorizontal();
            GetByName(properties, EffectName(layer, "Translation")).vectorValue = translation;

            Vector4 pivotScale = GetByName(properties, EffectName(layer, "PivotScale")).vectorValue;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Pivot");
                pivotScale.x = EditorGUILayout.FloatField(pivotScale.x);
                pivotScale.y = EditorGUILayout.FloatField(pivotScale.y);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Scale");
                pivotScale.z = EditorGUILayout.FloatField(pivotScale.z);
                pivotScale.w = EditorGUILayout.FloatField(pivotScale.w);
            }
            EditorGUILayout.EndHorizontal();
            GetByName(properties, EffectName(layer, "PivotScale")).vectorValue = pivotScale;

            BoolProperty(GetByName(properties, EffectName(layer, "Foreground")), "Foreground");
        }
        EditorGUILayout.EndVertical();
    }



    bool BoolProperty(MaterialProperty property, string name)
    {
        bool toggle = property.floatValue == 0 ? false : true;
        toggle = EditorGUILayout.Toggle(name, toggle);
        property.floatValue = toggle ? 1 : 0;

        return toggle;
    }

    string EffectName(int layer, string property)
    {
        return string.Format("_EffectsLayer{0}{1}", layer.ToString(), property);
    }
}

