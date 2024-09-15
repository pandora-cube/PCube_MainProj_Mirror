using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public int currentChapter;
    public int currentStage;
    public int[] clearChapter;
    //public int setting; // 언어, 사운드, 키세팅 등등..
}

public class ProgressData : MonoBehaviour
{
    public static ProgressData Instance;

    PlayerData playerData = new PlayerData();
    string path;
    string fileName = "mirrorSaveFile";

    private void Awake()
    {
        #region 싱글톤
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(Instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/";
    }

    private void Start()
    {
        playerData.currentChapter = 1;
        playerData.currentStage = 1;

        SaveData();
        LoadData();
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData, true);
        print(data);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }
}
