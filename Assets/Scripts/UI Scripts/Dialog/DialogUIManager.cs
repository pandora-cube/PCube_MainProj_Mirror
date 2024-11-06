using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogUIManager : MonoBehaviour
{
    public static DialogUIManager instance;

    [SerializeField] TextMeshProUGUI[] textUI;
    [SerializeField] TextMeshProUGUI[] NameUI;
    [SerializeField] GameObject[] SpeakerUI;
    [SerializeField] GameObject DialogUI;
    [SerializeField] string[] Speaker;

    int speakerID;
    //string attackKey, moveKey, interactKey;
    private void Awake()
    {
        #region singleton
        if (instance == null) instance = this;
        else Destroy(instance);
        #endregion
        //GetKey();
    }

    public void SetDialogUI(Dialog dialog)
    {
        speakerID = dialog.speakerID; // 현재 발화자의 ID
        NameUI[speakerID].text = Speaker[speakerID];
        SpeakerUI[speakerID].SetActive(true);
        DialogUI.SetActive(true);

        foreach(var ui in SpeakerUI) // 현재 발화자가 아닌 인물의 UI off
            if (ui != SpeakerUI[speakerID])
                ui.SetActive(false);
    }

    public void UpdateText(string text)
    {
        textUI[speakerID].text = text; // 현재 텍스트 출력
    }

    public void HideDialogUI()
    {
        foreach (var ui in SpeakerUI) // 모든 UI 종료 후
            ui.SetActive(false);
        DialogUI.SetActive(false); // 판넬까지 Off
    }

    void GetKey()
    {
        // 사용자가 설정한 키로 키 네임 저장
        //if (playerInput != null) 
        //{
        //    attackKey = playerInput.actions["Attack"].GetBindingDisplayString();
        //    moveKey = playerInput.actions["Movement"].GetBindingDisplayString();
        //    interactKey = playerInput.actions["Interact"].GetBindingDisplayString();
        //}
    }

    IEnumerator SetTextforKey(string currentText) // 현재 문장에 keyCode가 있다면 치환
    {
        //if (currentText.Contains("{move}")) currentText = currentText.Replace("{move}", moveKey);
        //else if (currentText.Contains("{attack}")) currentText = currentText.Replace("{attack}", attackKey);
        //else if (currentText.Contains("{interact}")) currentText = currentText.Replace("{interact}", interactKey);

        yield return null;
    }
}