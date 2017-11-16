using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
public static class SaveLoader
{
    [System.Serializable]
    private struct SerializedVector3
    {
        public float x, y, z;
        private SerializedVector3(float nx, float ny, float nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }
        public override string ToString() => $"<{x}, {y}, {z}>";
        public static implicit operator Vector3(SerializedVector3 value) => new Vector3(value.x, value.y, value.z);
        public static implicit operator SerializedVector3(Vector3 value) => new SerializedVector3(value.x, value.y, value.z);
    }
    [System.Serializable]
    private struct SerializedQuaternion
    {
        public float x, y, z, w;
        private SerializedQuaternion(float nx, float ny, float nz, float nw)
        {
            x = nx;
            y = ny;
            z = nz;
            w = nw;
        }
        public override string ToString() => $"{{{x}, {y}, {z}, {w}}}";
        public static implicit operator Quaternion(SerializedQuaternion value) => new Quaternion(value.x, value.y, value.z, value.w);
        public static implicit operator SerializedQuaternion(Quaternion value) => new SerializedQuaternion(value.x, value.y, value.z, value.w);
    }
    [System.Serializable]
    private struct SerializedScoreData
    {
        public SerializedVector3[] positions;
        public SerializedQuaternion[] rotations;
        public string name;
        public float time;
        public int score, board, difficulty;
    }
    [System.Serializable]
    private struct SerializedCursedScores
    {
        public SerializedScoreData[] cursedScores;
    }
    private struct SerializedRaceScores
    {
        public SerializedScoreData[] raceScores;
    }
    [System.Serializable]
    private struct SerializedContinuousScores
    {
        public SerializedScoreData[] levels;
        public string name;
        public int difficulty;
    }
    [System.Serializable]
    private struct CombinedScores
    {
        public SerializedCursedScores[] cursedScores;
        public SerializedContinuousScores[] continuousScores;
        public SerializedRaceScores[] raceScores;
    }
    public static void Save()
    {
        CombinedScores convertedScores = new CombinedScores();
        int i, j, k;
        #region CurseScoresSaving
        ScoreManager.CursedScores[] unconvertedCursedScores = GameManager.instance.scoreScript.topCursedScores;
        convertedScores.cursedScores = new SerializedCursedScores[unconvertedCursedScores.Length];
        for (i = 0; i < unconvertedCursedScores.Length; ++i)
        {
            convertedScores.cursedScores[i].cursedScores = new SerializedScoreData[unconvertedCursedScores[i].cursedScores.Length];
            for (j = 0; j < unconvertedCursedScores[i].cursedScores.Length; ++j)
            {
                convertedScores.cursedScores[i].cursedScores[j].board = unconvertedCursedScores[i].cursedScores[j].board;
                convertedScores.cursedScores[i].cursedScores[j].name = unconvertedCursedScores[i].cursedScores[j].name;
                convertedScores.cursedScores[i].cursedScores[j].score = unconvertedCursedScores[i].cursedScores[j].score;
                convertedScores.cursedScores[i].cursedScores[j].time = unconvertedCursedScores[i].cursedScores[j].time;
                convertedScores.cursedScores[i].cursedScores[j].difficulty = (int)unconvertedCursedScores[i].cursedScores[j].difficulty;
                if (null != unconvertedCursedScores[i].cursedScores[j].positions)
                {
                    convertedScores.cursedScores[i].cursedScores[j].positions = new SerializedVector3[unconvertedCursedScores[i].cursedScores[j].positions.Length];
                    convertedScores.cursedScores[i].cursedScores[j].rotations = new SerializedQuaternion[unconvertedCursedScores[i].cursedScores[j].rotations.Length];
                    for (k = 0; k < unconvertedCursedScores[i].cursedScores[j].positions.Length; ++k)
                        convertedScores.cursedScores[i].cursedScores[j].positions[k] = unconvertedCursedScores[i].cursedScores[j].positions[k];
                    for (k = 0; k < unconvertedCursedScores[i].cursedScores[j].rotations.Length; ++k)
                        convertedScores.cursedScores[i].cursedScores[j].rotations[k] = unconvertedCursedScores[i].cursedScores[j].rotations[k];
                }
            }
        }
        #endregion
        #region ContinuousScoresSaving
        ScoreManager.ContinuousScores[] unconvertedContScores = GameManager.instance.scoreScript.topContinuousScores;
        convertedScores.continuousScores = new SerializedContinuousScores[unconvertedContScores.Length];
        for (i = 0; i < unconvertedContScores.Length; ++i)
        {
            convertedScores.continuousScores[i].levels = new SerializedScoreData[unconvertedContScores[i].levels.Length];
            convertedScores.continuousScores[i].name = unconvertedContScores[i].name;
            convertedScores.continuousScores[i].difficulty = (int)unconvertedContScores[i].difficulty;
            for (j = 0; j < unconvertedContScores[i].levels.Length; ++j)
            {
                convertedScores.continuousScores[i].levels[j].board = unconvertedContScores[i].levels[j].board;
                convertedScores.continuousScores[i].levels[j].score = unconvertedContScores[i].levels[j].score;
                convertedScores.continuousScores[i].levels[j].time = unconvertedContScores[i].levels[j].time;
                if (null != unconvertedContScores[i].levels[j].positions)
                {
                    convertedScores.continuousScores[i].levels[j].positions = new SerializedVector3[unconvertedContScores[i].levels[j].positions.Length];
                    convertedScores.continuousScores[i].levels[j].rotations = new SerializedQuaternion[unconvertedContScores[i].levels[j].rotations.Length];
                    for (k = 0; k < unconvertedContScores[i].levels[j].positions.Length; ++k)
                        convertedScores.continuousScores[i].levels[j].positions[k] = unconvertedContScores[i].levels[j].positions[k];
                    for (k = 0; k < unconvertedContScores[i].levels[j].rotations.Length; ++k)
                        convertedScores.continuousScores[i].levels[j].rotations[k] = unconvertedContScores[i].levels[j].rotations[k];
                }
            }
        }
        #endregion
        #region RaceScoresSaving
        ScoreManager.RaceScores[] unconvertedRaceScores = GameManager.instance.scoreScript.topRaceScores;
        convertedScores.raceScores = new SerializedRaceScores[unconvertedRaceScores.Length];
        for (i = 0; i < unconvertedRaceScores.Length; ++i)
        {
            convertedScores.raceScores[i].raceScores = new SerializedScoreData[unconvertedRaceScores[i].racescores.Length];
            for (j = 0; j < unconvertedRaceScores[i].racescores.Length; ++j)
            {
                convertedScores.raceScores[i].raceScores[j].board = unconvertedRaceScores[i].racescores[j].board;
                convertedScores.raceScores[i].raceScores[j].name = unconvertedRaceScores[i].racescores[j].name;
                convertedScores.raceScores[i].raceScores[j].score = unconvertedRaceScores[i].racescores[j].score;
                convertedScores.raceScores[i].raceScores[j].time = unconvertedRaceScores[i].racescores[j].time;
                convertedScores.raceScores[i].raceScores[j].difficulty = (int)unconvertedRaceScores[i].racescores[j].difficulty;
                if (null != unconvertedRaceScores[i].racescores[j].positions)
                {
                    convertedScores.raceScores[i].raceScores[j].positions = new SerializedVector3[unconvertedRaceScores[i].racescores[j].positions.Length];
                    convertedScores.raceScores[i].raceScores[j].rotations = new SerializedQuaternion[unconvertedRaceScores[i].racescores[j].rotations.Length];
                    for (k = 0; k < unconvertedRaceScores[i].racescores[j].positions.Length; ++k)
                        convertedScores.raceScores[i].raceScores[j].positions[k] = unconvertedRaceScores[i].racescores[j].positions[k];
                    for (k = 0; k < unconvertedRaceScores[i].racescores[j].rotations.Length; ++k)
                        convertedScores.raceScores[i].raceScores[j].rotations[k] = unconvertedRaceScores[i].racescores[j].rotations[k];
                }
            }
        }
        #endregion
        FileStream file = File.Create(Application.persistentDataPath + "/scores.gd");
        new BinaryFormatter().Serialize(file, convertedScores);
        file.Close();
    }
    public static ScoreManager.CursedScores[] LoadCurseScores()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/scores.gd"))
            {
                CombinedScores temp;
                FileStream file = File.Open(Application.persistentDataPath + "/scores.gd", FileMode.Open);
                try { temp = (CombinedScores)(new BinaryFormatter().Deserialize(file)); } catch { return null; } finally { file.Close(); }
                SerializedCursedScores[] unconvertedScores = temp.cursedScores;
                ScoreManager.CursedScores[] convertedScores = new ScoreManager.CursedScores[unconvertedScores.Length];
                int i, j, k;
                for (i = 0; i < unconvertedScores.Length; ++i)
                {
                    convertedScores[i].currentAmoutFilled = 0;
                    convertedScores[i].cursedScores = new ScoreManager.ScoreData[unconvertedScores[i].cursedScores.Length];
                    for (j = 0; j < unconvertedScores[i].cursedScores.Length; ++j)
                    {
                        convertedScores[i].cursedScores[j].board = unconvertedScores[i].cursedScores[j].board;
                        convertedScores[i].cursedScores[j].name = unconvertedScores[i].cursedScores[j].name;
                        convertedScores[i].cursedScores[j].score = unconvertedScores[i].cursedScores[j].score;
                        convertedScores[i].cursedScores[j].time = unconvertedScores[i].cursedScores[j].time;
                        convertedScores[i].cursedScores[j].difficulty = (GameDifficulties)unconvertedScores[i].cursedScores[j].difficulty;
                        if (0 != convertedScores[i].cursedScores[j].score)
                            ++convertedScores[i].currentAmoutFilled;
                        if (null != unconvertedScores[i].cursedScores[j].positions)
                        {
                            convertedScores[i].cursedScores[j].positions = new Vector3[unconvertedScores[i].cursedScores[j].positions.Length];
                            convertedScores[i].cursedScores[j].rotations = new Quaternion[unconvertedScores[i].cursedScores[j].rotations.Length];
                            for (k = 0; k < unconvertedScores[i].cursedScores[j].positions.Length; ++k)
                                convertedScores[i].cursedScores[j].positions[k] = unconvertedScores[i].cursedScores[j].positions[k];
                            for (k = 0; k < unconvertedScores[i].cursedScores[j].rotations.Length; ++k)
                                convertedScores[i].cursedScores[j].rotations[k] = unconvertedScores[i].cursedScores[j].rotations[k];
                        }
                    }
                }
                return convertedScores;
            }
            else
                return null;
        }
        catch
        {
            return null;
        }
    }
    public static ScoreManager.ContinuousScores[] LoadContinuousScores()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/scores.gd"))
            {
                CombinedScores temp;
                FileStream file = File.Open(Application.persistentDataPath + "/scores.gd", FileMode.Open);
                try { temp = (CombinedScores)(new BinaryFormatter().Deserialize(file)); } catch { return null; } finally { file.Close(); }
                SerializedContinuousScores[] unConvertedScores = temp.continuousScores;
                ScoreManager.ContinuousScores[] convertedScores = new ScoreManager.ContinuousScores[unConvertedScores.Length];
                int i, j, k;
                for (i = 0; i < unConvertedScores.Length; ++i)
                {
                    convertedScores[i].levels = new ScoreManager.ScoreData[unConvertedScores[i].levels.Length];
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
        catch
        {
            return null;
        }
    }

    public static ScoreManager.RaceScores[] LoadRaceScores()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/scores.gd"))
            {
                CombinedScores temp;
                FileStream file = File.Open(Application.persistentDataPath + "/scores.gd", FileMode.Open);
                try { temp = (CombinedScores)(new BinaryFormatter().Deserialize(file)); } catch { return null; } finally { file.Close(); }
                SerializedRaceScores[] unconvertedScores = temp.raceScores;
                ScoreManager.RaceScores[] convertedScores = new ScoreManager.RaceScores[unconvertedScores.Length];
                int i, j, k;
                Debug.Log("Add Race loader");
                for (i = 0; i < unconvertedScores.Length; ++i)
                {
                    convertedScores[i].currentAmoutFilled = 0;
                    convertedScores[i].racescores = new ScoreManager.ScoreData[unconvertedScores[i].raceScores.Length];
                    for (j = 0; j < unconvertedScores[i].raceScores.Length; ++j)
                    {
                        convertedScores[i].racescores[j].board = unconvertedScores[i].raceScores[j].board;
                        convertedScores[i].racescores[j].name = unconvertedScores[i].raceScores[j].name;
                        convertedScores[i].racescores[j].score = unconvertedScores[i].raceScores[j].score;
                        convertedScores[i].racescores[j].time = unconvertedScores[i].raceScores[j].time;
                        convertedScores[i].racescores[j].difficulty = (GameDifficulties)unconvertedScores[i].raceScores[j].difficulty;
                        if (0 != convertedScores[i].racescores[j].score)
                            ++convertedScores[i].currentAmoutFilled;
                        if (null != unconvertedScores[i].raceScores[j].positions)
                        {
                            convertedScores[i].racescores[j].positions = new Vector3[unconvertedScores[i].raceScores[j].positions.Length];
                            convertedScores[i].racescores[j].rotations = new Quaternion[unconvertedScores[i].raceScores[j].rotations.Length];
                            for (k = 0; k < unconvertedScores[i].raceScores[j].positions.Length; ++k)
                                convertedScores[i].racescores[j].positions[k] = unconvertedScores[i].raceScores[j].positions[k];
                            for (k = 0; k < unconvertedScores[i].raceScores[j].rotations.Length; ++k)
                                convertedScores[i].racescores[j].rotations[k] = unconvertedScores[i].raceScores[j].rotations[k];
                        }
                    }
                }
                return convertedScores;
            }
            else
                return null;
        }
        catch
        {
            return null;
        }
    }
}