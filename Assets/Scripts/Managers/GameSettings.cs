using UnityEngine;
using Xander.BoolConversion;
public static class GameSettings
{
    public static bool SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key + "_BOOL", value.Int());
        return value;
    }
    public static bool GetBool(string key, bool defaultValue = false) { return SetBool(key, PlayerPrefs.GetInt(key + "_BOOL", defaultValue.Int()).Bool()); }
    public static bool GetBool(string key, ref bool value) { return value = GetBool(key, value); }
    public static void ResetBool(string key)
    {
        key += "_BOOL";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasBool(string key) { return PlayerPrefs.HasKey(key + "_BOOL"); }

    public static int SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key + "_INT", value);
        return value;
    }
    public static int GetInt(string key, int defaultValue = 0) { return SetInt(key, PlayerPrefs.GetInt(key + "_INT", defaultValue)); }
    public static int GetInt(string key, ref int value) { return value = GetInt(key, value); }
    public static void ResetInt(string key)
    {
        key += "_INT";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasInt(string key) { return PlayerPrefs.HasKey(key + "_INT"); }

    public static uint SetUint(string key, uint value)
    {
        PlayerPrefs.SetInt(key + "_UINT", (int)value);
        return value;
    }
    public static uint GetUint(string key, uint defaultValue = 0u) { return SetUint(key, (uint)PlayerPrefs.GetInt(key + "_UINT", (int)defaultValue)); }
    public static uint GetUint(string key, ref uint value) { return value = GetUint(key, value); }
    public static void ResetUint(string key)
    {
        key += "_UINT";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasUint(string key) { return PlayerPrefs.HasKey(key + "_UINT"); }

    public static float SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key + "_FLOAT", value);
        return value;
    }
    public static float GetFloat(string key, float defaultValue = 0.0f) { return SetFloat(key, PlayerPrefs.GetFloat(key + "_FLOAT", defaultValue)); }
    public static float GetFloat(string key, ref float value) { return value = GetFloat(key, value); }
    public static void ResetFloat(string key)
    {
        key += "_FLOAT";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasFloat(string key) { return PlayerPrefs.HasKey(key + "_FLOAT"); }

    public static string SetString(string key, string value)
    {
        PlayerPrefs.SetString(key + "_STRING", value);
        return value;
    }
    public static string GetString(string key, string defaultValue = "") { return SetString(key, PlayerPrefs.GetString(key + "_STRING", defaultValue)); }
    public static string GetString(string key, ref string value) { return value = GetString(key, value); }
    public static void ResetString(string key)
    {
        key += "_STRING";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasString(string key) { return PlayerPrefs.HasKey(key + "_STRING"); }

    public static char SetChar(string key, char value)
    {
        PlayerPrefs.SetInt(key + "_CHAR", value);
        return value;
    }
    public static char GetChar(string key, char defaultValue = '\0') { return SetChar(key, (char)PlayerPrefs.GetInt(key + "_CHAR", defaultValue)); }
    public static char GetChar(string key, ref char value) { return value = GetChar(key, value); }
    public static void ResetChar(string key)
    {
        key += "_CHAR";
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasChar(string key) { return PlayerPrefs.HasKey(key + "_CHAR"); }

    public static Color SetColor(string key, Color value)
    {
        key += "_COLOR_";
        PlayerPrefs.SetFloat(key + 'R', value.r);
        PlayerPrefs.SetFloat(key + 'G', value.g);
        PlayerPrefs.SetFloat(key + 'B', value.b);
        PlayerPrefs.SetFloat(key + 'A', value.a);
        return value;
    }
    public static Color GetColor(string key, Color defaultValue)
    {
        Color ret;
        ret.r = PlayerPrefs.GetFloat(key + "_COLOR_R", defaultValue.r);
        ret.g = PlayerPrefs.GetFloat(key + "_COLOR_G", defaultValue.g);
        ret.b = PlayerPrefs.GetFloat(key + "_COLOR_B", defaultValue.b);
        ret.a = PlayerPrefs.GetFloat(key + "_COLOR_A", defaultValue.a);
        return SetColor(key, ret);
    }
    public static Color GetColor(string key) { return GetColor(key, Color.clear); }
    public static Color GetColor(string key, ref Color value) { return value = GetColor(key, value); }
    public static void ResetColor(string key)
    {
        key += "_COLOR_";
        if (PlayerPrefs.HasKey(key + 'R')) PlayerPrefs.DeleteKey(key + 'R');
        if (PlayerPrefs.HasKey(key + 'G')) PlayerPrefs.DeleteKey(key + 'G');
        if (PlayerPrefs.HasKey(key + 'B')) PlayerPrefs.DeleteKey(key + 'B');
        if (PlayerPrefs.HasKey(key + 'A')) PlayerPrefs.DeleteKey(key + 'A');
    }
    public static bool HasColor(string key)
    {
        key += "_COLOR_";
        return
            PlayerPrefs.HasKey(key + 'R') &&
            PlayerPrefs.HasKey(key + 'G') &&
            PlayerPrefs.HasKey(key + 'B') &&
            PlayerPrefs.HasKey(key + 'A');
    }

    public static T SetEnum<T>(string key, T value) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        PlayerPrefs.SetInt(key + "_ENUM_" + typeof(T).ToString().ToUpper(), System.Convert.ToInt32(value));
        return value;
    }
    public static T GetEnum<T>(string key, T defaultValue) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        return SetEnum(key, (T)(object)PlayerPrefs.GetInt(key + "_ENUM_" + typeof(T).ToString().ToUpper(), System.Convert.ToInt32(defaultValue)));
    }
    public static T GetEnum<T>(string key) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        return SetEnum(key, (T)(object)PlayerPrefs.GetInt(key + "_ENUM_" + typeof(T).ToString().ToUpper(), 0));
    }
    public static T GetEnum<T>(string key, ref T value) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        return value = GetEnum(key, value);
    }
    public static void ResetEnum<T>(string key) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        key += "_ENUM_" + typeof(T).ToString().ToUpper();
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
    }
    public static bool HasEnum<T>(string key) where T : struct, System.IFormattable, System.IConvertible, System.IComparable
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(typeof(T).ToString() + " is not an enum");
        return PlayerPrefs.HasKey(key + "_ENUM_" + typeof(T).ToString().ToUpper());
    }

    public static Vector3 SetVector3(string key, Vector3 value)
    {
        key += "_VECTOR3_";
        PlayerPrefs.SetFloat(key + 'X', value.x);
        PlayerPrefs.SetFloat(key + 'Y', value.y);
        PlayerPrefs.SetFloat(key + 'Z', value.z);
        return value;
    }
    public static Vector3 GetVector3(string key, Vector3 defaultValue)
    {
        Vector3 ret;
        ret.x = PlayerPrefs.GetFloat(key + "_VECTOR3_X", defaultValue.x);
        ret.y = PlayerPrefs.GetFloat(key + "_VECTOR3_Y", defaultValue.y);
        ret.z = PlayerPrefs.GetFloat(key + "_VECTOR3_Z", defaultValue.z);
        return SetVector3(key, ret);
    }
    public static Vector3 GetVector3(string key) { return GetVector3(key, Vector3.zero); }
    public static Vector3 GetVector3(string key, ref Vector3 value) { return value = GetVector3(key, value); }
    public static void ResetVector3(string key)
    {
        key += "_VECTOR3_";
        if (PlayerPrefs.HasKey(key + 'X')) PlayerPrefs.DeleteKey(key + 'X');
        if (PlayerPrefs.HasKey(key + 'Y')) PlayerPrefs.DeleteKey(key + 'Y');
        if (PlayerPrefs.HasKey(key + 'Z')) PlayerPrefs.DeleteKey(key + 'Z');
    }
    public static bool HasVector3(string key)
    {
        key += "_VECTOR3_";
        return
            PlayerPrefs.HasKey(key + 'X') &&
            PlayerPrefs.HasKey(key + 'Y') &&
            PlayerPrefs.HasKey(key + 'Z');
    }

    public static Vector2 SetVector2(string key, Vector2 value)
    {
        key += "_VECTOR2_";
        PlayerPrefs.SetFloat(key + 'X', value.x);
        PlayerPrefs.SetFloat(key + 'Y', value.y);
        return value;
    }
    public static Vector2 GetVector2(string key, Vector2 defaultValue)
    {
        Vector2 ret;
        ret.x = PlayerPrefs.GetFloat(key + "_VECTOR2_X", defaultValue.x);
        ret.y = PlayerPrefs.GetFloat(key + "_VECTOR2_Y", defaultValue.y);
        return SetVector2(key, ret);
    }
    public static Vector2 GetVector2(string key) { return GetVector2(key, Vector2.zero); }
    public static Vector2 GetVector2(string key, ref Vector2 value) { return value = GetVector2(key, value); }
    public static void ResetVector2(string key)
    {
        key += "_VECTOR2_";
        if (PlayerPrefs.HasKey(key + 'X')) PlayerPrefs.DeleteKey(key + 'X');
        if (PlayerPrefs.HasKey(key + 'Y')) PlayerPrefs.DeleteKey(key + 'Y');
    }
    public static bool HasVector2(string key)
    {
        key += "_VECTOR2_";
        return
            PlayerPrefs.HasKey(key + 'X') &&
            PlayerPrefs.HasKey(key + 'Y');
    }

    public static Quaternion SetQuaternion(string key, Quaternion value)
    {
        key += "_QUATERNION_";
        PlayerPrefs.SetFloat(key + 'X', value.x);
        PlayerPrefs.SetFloat(key + 'Y', value.y);
        PlayerPrefs.SetFloat(key + 'Z', value.z);
        PlayerPrefs.SetFloat(key + 'W', value.w);
        return value;
    }
    public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
    {
        Quaternion ret;
        ret.x = PlayerPrefs.GetFloat(key + "_QUATERNION_X", defaultValue.x);
        ret.y = PlayerPrefs.GetFloat(key + "_QUATERNION_Y", defaultValue.y);
        ret.z = PlayerPrefs.GetFloat(key + "_QUATERNION_Z", defaultValue.z);
        ret.w = PlayerPrefs.GetFloat(key + "_QUATERNION_W", defaultValue.w);
        return SetQuaternion(key, ret);
    }
    public static Quaternion GetQuaternion(string key) { return GetQuaternion(key, Quaternion.identity); }
    public static Quaternion GetQuaternion(string key, ref Quaternion value) { return value = GetQuaternion(key, value); }
    public static void ResetQuaternion(string key)
    {
        key += "_QUATERNION_";
        if (PlayerPrefs.HasKey(key + 'X')) PlayerPrefs.DeleteKey(key + 'X');
        if (PlayerPrefs.HasKey(key + 'Y')) PlayerPrefs.DeleteKey(key + 'Y');
        if (PlayerPrefs.HasKey(key + 'Z')) PlayerPrefs.DeleteKey(key + 'Z');
        if (PlayerPrefs.HasKey(key + 'W')) PlayerPrefs.DeleteKey(key + 'W');
    }
    public static bool HasQuaternion(string key)
    {
        key += "_QUATERNION_";
        return
            PlayerPrefs.HasKey(key + 'X') &&
            PlayerPrefs.HasKey(key + 'Y') &&
            PlayerPrefs.HasKey(key + 'Z') &&
            PlayerPrefs.HasKey(key + 'W');
    }

    public static void Save() { PlayerPrefs.Save(); }
    public static void ResetAll() { PlayerPrefs.DeleteAll(); }
}