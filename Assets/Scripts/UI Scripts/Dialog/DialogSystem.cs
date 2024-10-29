using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] private TextAsset dialogJSONFile;
    //[SerializeField] private string filePath = "/Resources/DialogTexts.json";

    private string currentText;
    private int currentID = 0;
    private int currentDialogScene = 0;
    private int prevSpeaker, currentSpeaker;

    private bool firstDialog = true;

    private DialogList dialogLists = new DialogList();

    private PlayerInput playerInput;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    string attackKey, moveKey, interactKey;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        GetKey();
    }

    private void Start()
    {
        JsonLoad();
    }

    void JsonLoad()
    {
        if (dialogJSONFile != null)
        {
            string json = dialogJSONFile.text;
            dialogLists = JsonUtility.FromJson<DialogList>(json);

            if (dialogLists != null)
            {
                currentID = 0;
                StartCoroutine(DialogProgress());
            }
            else Debug.LogError("Dialog load failed");
        }
        else Debug.LogError("No JSON file assigned in the inspector.");


    }

    public IEnumerator DialogProgress()
    {
        firstDialog = true;
        PlayerState.canMove = false;
        currentDialogScene = dialogLists.dialog[currentID].dialogScene;

        while (currentID < dialogLists.dialog.Count && currentDialogScene == dialogLists.dialog[currentID].dialogScene)
        {
            currentSpeaker = dialogLists.dialog[currentID].speakerID;
            SetDialogUI();

            int index = 0;
            while (index < dialogLists.dialog[currentID].dialogText.Length)
            {
                currentText = dialogLists.dialog[currentID].dialogText[index];

                string playFunc = dialogLists.dialog[currentID].playFunc;
                if (playFunc != "None") yield return StartCoroutine(playFunc);

                yield return StartCoroutine(DialogTypingEffect());
                index++;
            }

            prevSpeaker = currentSpeaker;
            currentID++;
        }

        PlayerState.canMove = true;
        firstDialog = false;
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

    void GetKey()
    {
        if (playerInput != null)
        {
            attackKey = playerInput.actions["Attack"].GetBindingDisplayString();
            moveKey = playerInput.actions["Movement"].GetBindingDisplayString();
            interactKey = playerInput.actions["Interact"].GetBindingDisplayString();
        }
    }

    IEnumerator SetTextforKey()
    {
        if (currentText.Contains("{move}")) currentText = currentText.Replace("{move}", moveKey);
        else if (currentText.Contains("{attack}")) currentText = currentText.Replace("{attack}", attackKey);
        else if (currentText.Contains("{interact}")) currentText = currentText.Replace("{interact}", interactKey);

        yield return null;
    }
}
