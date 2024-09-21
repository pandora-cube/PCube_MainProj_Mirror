using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//public enum Speaker { Player, Glasya }

[System.Serializable]
public class Dialog 
{
    public int dialogScene;
    public int id;
    public string[] dialogText;
    public int speakerID;
    public string playFunc;

    public Dialog(int dialogScene, int id, string[] dialogText, int speakerID, string playFunc)
    {

        this.dialogScene = dialogScene;
        this.id = id;
        this.dialogText = dialogText;
        this.speakerID = speakerID;
        this.playFunc = playFunc;
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
    private int currentDialogScene = 0;
    private int prevSpeaker, currentSpeaker;

    private bool firstDialog = true;

    [SerializeField] string filePath = "/Scripts/Json/DialogTexts.json";
    private DialogList dialogLists = new DialogList();
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        JsonLoad();
    }

    void JsonSaveText(int id, string dialogText, int speakerID)
    {
        //dialogLists.dialog.Add(new Dialog(id, dialogText, speaker)); // ��ȭ �ؽ�Ʈ �߰�
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

            if (dialogLists != null)
            {
                currentID = 0;
                StartCoroutine(DialogProgress());
            }
            else Debug.Log("text Load failed");
        }
        else Debug.Log("Not found json file");
    }

    IEnumerator DialogProgress()
    {
        firstDialog = true;
        playerController.canMove = false;
        currentDialogScene = dialogLists.dialog[currentID].dialogScene;

        while (currentDialogScene == dialogLists.dialog[currentID].dialogScene) // ���� ����Ǵ� �ؽ�Ʈ�� ���� ������ ǥ��
        {
            currentSpeaker = dialogLists.dialog[currentID].speakerID;
            SetDialogUI();

            int index = 0;
            while (index < dialogLists.dialog[currentID].dialogText.Length)
            {
                currentText = dialogLists.dialog[currentID].dialogText[index];
                
                //��� ǥ�� ���� & ���콺 �Է� ó��
                yield return StartCoroutine(DialogTypingEffect());
                index++;
            }
            
            prevSpeaker = currentSpeaker;
            currentID++;
        }

        string playFunc = dialogLists.dialog[currentID - 1].playFunc;
        if (playFunc != "None") Invoke(playFunc, 0f);

        playerController.canMove = true;
        
        SpeakerUI[currentSpeaker].SetActive(false);
        DialogUI.SetActive(false);
    }

    IEnumerator DialogTypingEffect()
    {
        string showText = "";

        for (int i = 0; i < currentText.Length; i++)
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
