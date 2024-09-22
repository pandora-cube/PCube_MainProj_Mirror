using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public int currentChapter;
    public int currentStage;
    public int[] clearChapter;
}

public class ProgressData : MonoBehaviour
{
    public static ProgressData Instance;

    public PlayerData playerData = new PlayerData();
    string path;
    string fileName = "mirrorSaveFile";

    private void Awake()
    {
        #region �̱���
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(Instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/";

        playerData.currentChapter = 1;
        playerData.currentStage = 1;

        SaveData();
        LoadData();
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }
}
