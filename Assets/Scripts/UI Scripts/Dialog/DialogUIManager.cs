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
        speakerID = dialog.speakerID; // ���� ��ȭ���� ID
        NameUI[speakerID].text = Speaker[speakerID];
        SpeakerUI[speakerID].SetActive(true);
        DialogUI.SetActive(true);

        foreach(var ui in SpeakerUI) // ���� ��ȭ�ڰ� �ƴ� �ι��� UI off
            if (ui != SpeakerUI[speakerID])
                ui.SetActive(false);
    }

    public void UpdateText(string text)
    {
        textUI[speakerID].text = text; // ���� �ؽ�Ʈ ���
    }

    public void HideDialogUI()
    {
        foreach (var ui in SpeakerUI) // ��� UI ���� ��
            ui.SetActive(false);
        DialogUI.SetActive(false); // �ǳڱ��� Off
    }

    void GetKey()
    {
        // ����ڰ� ������ Ű�� Ű ���� ����
        //if (playerInput != null) 
        //{
        //    attackKey = playerInput.actions["Attack"].GetBindingDisplayString();
        //    moveKey = playerInput.actions["Movement"].GetBindingDisplayString();
        //    interactKey = playerInput.actions["Interact"].GetBindingDisplayString();
        //}
    }

    IEnumerator SetTextforKey(string currentText) // ���� ���忡 keyCode�� �ִٸ� ġȯ
    {
        //if (currentText.Contains("{move}")) currentText = currentText.Replace("{move}", moveKey);
        //else if (currentText.Contains("{attack}")) currentText = currentText.Replace("{attack}", attackKey);
        //else if (currentText.Contains("{interact}")) currentText = currentText.Replace("{interact}", interactKey);

        yield return null;
    }
}