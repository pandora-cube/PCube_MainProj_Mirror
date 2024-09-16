using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//public enum Speaker { Player, Glasya }

[System.Serializable]
public class Dialog 
{
    public int id;
    public string dialogText;
    public int speakerID;

    public Dialog(int id, string dialogText, int speakerID)
    {
        this.id = id;
        this.dialogText = dialogText;
        this.speakerID = speakerID;
    }

}

[System.Serializable]
public class DialogList
{
    public List<Dialog> dialog;

    public DialogList()
    {
        dialog = new List<Dialog>();
    }
}

public class DialogSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textUI;
    [SerializeField] TextMeshProUGUI[] NameUI;
    [SerializeField] GameObject[] SpeakerUI;
    [SerializeField] GameObject DialogUI;
    [SerializeField] string[] Speaker;

    private string currentText;
    private int currentID = 0;
    private int prevSpeaker, currentSpeaker;

    private bool firstDialog = true;

    [SerializeField] string filePath = "/Scripts/Json/DialogTexts.json";
    private DialogList dialogLists = new DialogList();

    private void Start()
    {
        JsonLoad();
    }

    void JsonSaveText(int id, string dialogText, int speakerID)
    {
        //dialogLists.dialog.Add(new Dialog(id, dialogText, speaker)); // 대화 텍스트 추가
        string path = Application.dataPath + filePath;
        string json = JsonUtility.ToJson(dialogLists, true);
        File.WriteAllText(path, json);
    }

    void JsonLoad()
    {
        string path = Application.dataPath + filePath;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            dialogLists = JsonUtility.FromJson<DialogList>(json);

            if (dialogLists != null) StartCoroutine(DialogProgress());
            else Debug.Log("text Load failed");
        }
        else Debug.Log("Not found json file");
    }

    IEnumerator DialogProgress()
    {
        firstDialog = true;

        while (currentID < dialogLists.dialog.Count) // 다음 진행되는 텍스트가 없을 때까지 표시
        {
            //current 리스트 저장
            currentText = dialogLists.dialog[currentID].dialogText;
            currentSpeaker = dialogLists.dialog[currentID].speakerID;

            //current 발화자 UI ON
            SetDialogUI();

            //대사 표시 연출 & 마우스 입력 처리
            yield return StartCoroutine(DialogTypingEffect());

            prevSpeaker = currentSpeaker;
            currentID++;
        }

        SpeakerUI[currentSpeaker].SetActive(false);
        DialogUI.SetActive(false);
    }

    IEnumerator DialogTypingEffect()
    {
        string showText = "";

        for (int i = 0; i< currentText.Length; i++)
        {
            showText += currentText[i];
            textUI[currentSpeaker].text = showText;

            if (Input.GetMouseButton(0))
            {
                textUI[currentSpeaker].text = currentText;
                break;
            }

            yield return new WaitForSeconds(0.08f);
        }

        yield return new WaitForSeconds(0.15f);

        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                yield return new WaitForSeconds(0.15f);
                break;
            }
            yield return null;
        }
    }

    void SetDialogUI()
    {
        if (firstDialog)
        {
            NameUI[currentSpeaker].text = Speaker[currentSpeaker];
            DialogUI.SetActive(true);
            SpeakerUI[currentSpeaker].SetActive(true);

            firstDialog = false;
        }
        else if (prevSpeaker != currentSpeaker)
        {
            NameUI[currentSpeaker].text = Speaker[currentSpeaker];

            SpeakerUI[prevSpeaker].SetActive(false);
            SpeakerUI[currentSpeaker].SetActive(true);
        }
    }
}
