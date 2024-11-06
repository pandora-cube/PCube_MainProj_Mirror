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
        this.dialogScene = dialogScene; // 한 번의 대화 범위
        this.id = id; // 각 대화에 부여된 고유 ID
        this.dialogText = dialogText; // 대화 Text
        this.speakerID = speakerID; // 발화자 ID
        this.playFunc = playFunc; // 연출 때문에 넣은 기능, 아직 X
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

    void JsonLoad() // Json 파일 Load
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

    public void StartDialog() // 현재 currentID부터 한 타임 대화 시작
    {
        currentDialogScene = dialogLists.dialog[currentID].dialogScene;
        firstDialog = true;
        StartCoroutine(DialogProgress());
    }

    public IEnumerator DialogProgress()
    {
        PlayerState.canMove = false; // player 움직임 제어
        
        // 한 대화 씬 (같은 dialgScene 동안 반복)
        while (currentID < dialogLists.dialog.Count && currentDialogScene == dialogLists.dialog[currentID].dialogScene)
        {
            var currentDialog = dialogLists.dialog[currentID];
            dialogUI.SetDialogUI(currentDialog); // 현재 대화의 UI 표시

            // 현재 ID의 Texts을 출력
            foreach(var currentText in dialogLists.dialog[currentID].dialogText)
            {
                yield return StartCoroutine(DialogTypingEffect(currentText));
            }

            currentID++; // ID 증가
        }

        PlayerState.canMove = true; // 대화 종료 처리
        dialogUI.HideDialogUI();
    }

    IEnumerator DialogTypingEffect(string currentText) // 한 글자씩 출력 연출
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