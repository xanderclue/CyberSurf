using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
public static class SaveLoader
{
    [System.Serializable]
    public struct serializableVec3
    {
        public float x, y, z;
        private serializableVec3(float nx, float ny, float nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }
        public override string ToString() { return string.Format("[{0}, {1}, {2}]", x, y, z); }
        public static implicit operator Vector3(serializableVec3 value) { return new Vector3(value.x, value.y, value.z); }
        public static implicit operator serializableVec3(Vector3 value) { return new serializableVec3(value.x, value.y, value.z); }
    }
    [System.Serializable]
    public struct serializableQuaternion
    {
        public float x, y, z, w;
        private serializableQuaternion(float nx, float ny, float nz, float nw)
        {
            x = nx;
            y = ny;
            z = nz;
            w = nw;
        }
        public override string ToString() { return string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w); }
        public static implicit operator Quaternion(serializableQuaternion value) { return new Quaternion(value.x, value.y, value.z, value.w); }
        public static implicit operator serializableQuaternion(Quaternion value) { return new serializableQuaternion(value.x, value.y, value.z, value.w); }
    }
    [System.Serializable]
    private struct serializableScore
    {
        public serializableVec3[] positions;
        public serializableQuaternion[] rotations;
        public int score;
        public float time;
        public int board;
        public string name;
        public int difficulty;
    }
    [System.Serializable]
    private struct serializableCurseLevelScores
    {
        public serializableScore[] curseScores;
    }
    [System.Serializable]
    private struct serializableContinuousScores
    {
        public serializableScore[] levels;
        public string name;
        public int difficulty;
    }
    [System.Serializable]
    private struct combinedScoreTypes
    {
        public serializableCurseLevelScores[] curseScores;
        public serializableContinuousScores[] continuousScores;
    }
    public static void Save()
    {
        combinedScoreTypes convertedScores = new combinedScoreTypes();
        int i, j, k;
        #region CurseScoresSaving
        ScoreManager.levelCurseScores[] unConvertedCurseScores = GameManager.instance.scoreScript.topCurseScores;
        convertedScores.curseScores = new serializableCurseLevelScores[unConvertedCurseScores.Length];
        for (i = 0; i < unConvertedCurseScores.Length; ++i)
        {
            convertedScores.curseScores[i].curseScores = new serializableScore[unConvertedCurseScores[i].curseScores.Length];
            for (j = 0; j < unConvertedCurseScores[i].curseScores.Length; ++j)
            {
                convertedScores.curseScores[i].curseScores[j].board = unConvertedCurseScores[i].curseScores[j].board;
                convertedScores.curseScores[i].curseScores[j].name = unConvertedCurseScores[i].curseScores[j].name;
                convertedScores.curseScores[i].curseScores[j].score = unConvertedCurseScores[i].curseScores[j].score;
                convertedScores.curseScores[i].curseScores[j].time = unConvertedCurseScores[i].curseScores[j].time;
                convertedScores.curseScores[i].curseScores[j].difficulty = (int)unConvertedCurseScores[i].curseScores[j].difficulty;
                if (null != unConvertedCurseScores[i].curseScores[j].positions)
                {
                    convertedScores.curseScores[i].curseScores[j].positions = new serializableVec3[unConvertedCurseScores[i].curseScores[j].positions.Length];
                    convertedScores.curseScores[i].curseScores[j].rotations = new serializableQuaternion[unConvertedCurseScores[i].curseScores[j].rotations.Length];
                    for (k = 0; k < unConvertedCurseScores[i].curseScores[j].positions.Length; ++k)
                        convertedScores.curseScores[i].curseScores[j].positions[k] = unConvertedCurseScores[i].curseScores[j].positions[k];
                    for (k = 0; k < unConvertedCurseScores[i].curseScores[j].rotations.Length; ++k)
                        convertedScores.curseScores[i].curseScores[j].rotations[k] = unConvertedCurseScores[i].curseScores[j].rotations[k];
                }
            }
        }
        #endregion
        #region ContinuousScoresSaving
        ScoreManager.continuousScores[] unconvertedContScores = GameManager.instance.scoreScript.topContinuousScores;
        convertedScores.continuousScores = new serializableContinuousScores[unconvertedContScores.Length];
        for (i = 0; i < unconvertedContScores.Length; ++i)
        {
            convertedScores.continuousScores[i].levels = new serializableScore[unconvertedContScores[i].levels.Length];
            convertedScores.continuousScores[i].name = unconvertedContScores[i].name;
            convertedScores.continuousScores[i].difficulty = (int)unconvertedContScores[i].difficulty;
            for (j = 0; j < unconvertedContScores[i].levels.Length; ++j)
            {
                convertedScores.continuousScores[i].levels[j].board = unconvertedContScores[i].levels[j].board;
                convertedScores.continuousScores[i].levels[j].score = unconvertedContScores[i].levels[j].score;
                convertedScores.continuousScores[i].levels[j].time = unconvertedContScores[i].levels[j].time;
                if (null != unconvertedContScores[i].levels[j].positions)
                {
                    convertedScores.continuousScores[i].levels[j].positions = new serializableVec3[unconvertedContScores[i].levels[j].positions.Length];
                    convertedScores.continuousScores[i].levels[j].rotations = new serializableQuaternion[unconvertedContScores[i].levels[j].rotations.Length];
                    for (k = 0; k < unconvertedContScores[i].levels[j].positions.Length; ++k)
                        convertedScores.continuousScores[i].levels[j].positions[k] = unconvertedContScores[i].levels[j].positions[k];
                    for (k = 0; k < unconvertedContScores[i].levels[j].rotations.Length; ++k)
                        convertedScores.continuousScores[i].levels[j].rotations[k] = unconvertedContScores[i].levels[j].rotations[k];
                }
            }
        }
        #endregion
        FileStream file = File.Create(Application.persistentDataPath + "/scores.gd");
        new BinaryFormatter().Serialize(file, convertedScores);
        file.Close();
    }
    public static ScoreManager.levelCurseScores[] LoadCurseScores()
    {
        if (File.Exists(Application.persistentDataPath + "/scores.gd"))
        {
            combinedScoreTypes temp;
            FileStream file = File.Open(Application.persistentDataPath + "/scores.gd", FileMode.Open);
            temp = (combinedScoreTypes)(new BinaryFormatter().Deserialize(file));
            file.Close();
            serializableCurseLevelScores[] unConvertedScores = temp.curseScores;
            ScoreManager.levelCurseScores[] convertedScores = new ScoreManager.levelCurseScores[unConvertedScores.Length];
            int i, j, k;
            for (i = 0; i < unConvertedScores.Length; ++i)
            {
                convertedScores[i].currentAmoutFilled = 0;
                convertedScores[i].curseScores = new ScoreManager.scoreStruct[unConvertedScores[i].curseScores.Length];
                for (j = 0; j < unConvertedScores[i].curseScores.Length; ++j)
                {
                    convertedScores[i].curseScores[j].board = unConvertedScores[i].curseScores[j].board;
                    convertedScores[i].curseScores[j].name = unConvertedScores[i].curseScores[j].name;
                    convertedScores[i].curseScores[j].score = unConvertedScores[i].curseScores[j].score;
                    convertedScores[i].curseScores[j].time = unConvertedScores[i].curseScores[j].time;
                    convertedScores[i].curseScores[j].difficulty = (GameDifficulties)unConvertedScores[i].curseScores[j].difficulty;
                    if (0 != convertedScores[i].curseScores[j].score)
                        ++convertedScores[i].currentAmoutFilled;
                    if (null != unConvertedScores[i].curseScores[j].positions)
                    {
                        convertedScores[i].curseScores[j].positions = new Vector3[unConvertedScores[i].curseScores[j].positions.Length];
                        convertedScores[i].curseScores[j].rotations = new Quaternion[unConvertedScores[i].curseScores[j].rotations.Length];
                        for (k = 0; k < unConvertedScores[i].curseScores[j].positions.Length; ++k)
                            convertedScores[i].curseScores[j].positions[k] = unConvertedScores[i].curseScores[j].positions[k];
                        for (k = 0; k < unConvertedScores[i].curseScores[j].rotations.Length; ++k)
                            convertedScores[i].curseScores[j].rotations[k] = unConvertedScores[i].curseScores[j].rotations[k];
                    }
                }
            }
            return convertedScores;
        }
        else
            return null;
    }
    public static ScoreManager.continuousScores[] LoadContinuousScores()
    {
        if (File.Exists(Application.persistentDataPath + "/scores.gd"))
        {
            combinedScoreTypes temp;
            FileStream file = File.Open(Application.persistentDataPath + "/scores.gd", FileMode.Open);
            temp = (combinedScoreTypes)(new BinaryFormatter().Deserialize(file));
            file.Close();
            serializableContinuousScores[] unConvertedScores = temp.continuousScores;
            ScoreManager.continuousScores[] convertedScores = new ScoreManager.continuousScores[unConvertedScores.Length];
            int i, j, k;
            for (i = 0; i < unConvertedScores.Length; ++i)
            {
                convertedScores[i].levels = new ScoreManager.scoreStruct[unConvertedScores[i].levels.Length];
                convertedScores[i].name = unConvertedScores[i].name;
                convertedScores[i].difficulty = (GameDifficulties)unConvertedScores[i].difficulty;
                if ("" != convertedScores[i].name)
                    ++GameManager.instance.scoreScript.curFilledCont;
                for (j = 0; j < unConvertedScores[i].levels.Length; ++j)
                {
                    convertedScores[i].levels[j].board = unConvertedScores[i].levels[j].board;
                    convertedScores[i].levels[j].score = unConvertedScores[i].levels[j].score;
                    convertedScores[i].levels[j].time = unConvertedScores[i].levels[j].time;
                    if (null != unConvertedScores[i].levels[j].positions)
                    {
                        convertedScores[i].levels[j].positions = new Vector3[unConvertedScores[i].levels[j].positions.Length];
                        convertedScores[i].levels[j].rotations = new Quaternion[unConvertedScores[i].levels[j].rotations.Length];
                        for (k = 0; k < unConvertedScores[i].levels[j].positions.Length; ++k)
                            convertedScores[i].levels[j].positions[k] = unConvertedScores[i].levels[j].positions[k];
                        for (k = 0; k < unConvertedScores[i].levels[j].rotations.Length; ++k)
                            convertedScores[i].levels[j].rotations[k] = unConvertedScores[i].levels[j].rotations[k];
                    }
                }
            }
            return convertedScores;
        }
        else
            return null;
    }
}