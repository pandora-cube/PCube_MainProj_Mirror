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
        this.dialogScene = dialogScene; // �� ���� ��ȭ ����
        this.id = id; // �� ��ȭ�� �ο��� ���� ID
        this.dialogText = dialogText; // ��ȭ Text
        this.speakerID = speakerID; // ��ȭ�� ID
        this.playFunc = playFunc; // ���� ������ ���� ���, ���� X
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
    public static DialogSystem instance;

    [SerializeField] private TextAsset dialogJSONFile; // fix later...

    private DialogList dialogLists = new DialogList();
    private int currentID = 0;
    private int currentDialogScene = 0;
    private bool firstDialog = true;

    private PlayerInput playerInput;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    private DialogUIManager dialogUI => DialogUIManager.instance;
    
    private void Awake()
    {
        #region singleton
        if (instance == null) instance = this;
        else Destroy(instance);
        #endregion
    }

    private void Start()
    {
        JsonLoad();
    }

    void JsonLoad() // Json ���� Load
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

    public void StartDialog() // ���� currentID���� �� Ÿ�� ��ȭ ����
    {
        currentDialogScene = dialogLists.dialog[currentID].dialogScene;
        firstDialog = true;
        StartCoroutine(DialogProgress());
    }

    public IEnumerator DialogProgress()
    {
        PlayerState.canMove = false; // player ������ ����
        
        // �� ��ȭ �� (���� dialgScene ���� �ݺ�)
        while (currentID < dialogLists.dialog.Count && currentDialogScene == dialogLists.dialog[currentID].dialogScene)
        {
            var currentDialog = dialogLists.dialog[currentID];
            dialogUI.SetDialogUI(currentDialog); // ���� ��ȭ�� UI ǥ��

            // ���� ID�� Texts�� ���
            foreach(var currentText in dialogLists.dialog[currentID].dialogText)
            {
                yield return StartCoroutine(DialogTypingEffect(currentText));
            }

            currentID++; // ID ����
        }

        PlayerState.canMove = true; // ��ȭ ���� ó��
        dialogUI.HideDialogUI();
    }

    IEnumerator DialogTypingEffect(string currentText) // �� ���ھ� ��� ����
    {
        string showText = "";

        foreach(var word in currentText)
        {
            showText += word;
            dialogUI.UpdateText(showText);

            if (Input.GetMouseButton(0))
            {
                dialogUI.UpdateText(currentText);
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
}