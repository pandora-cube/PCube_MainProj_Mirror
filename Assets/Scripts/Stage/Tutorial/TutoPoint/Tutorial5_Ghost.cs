using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5_Ghost : TutoEnterTrigger
{
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;
    [SerializeField] private TutorialMirror mirror;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thisTutoPlaying && collision.CompareTag("Player"))
        {
            PlayTutorial(); // ��ȭ ����
            ApplyToGhostTimeLimit(); 
        }
    }

    void ApplyToGhostTimeLimit()
    {
        playerGhostHealthManager.SetGhostTimeLimit(60f); // ghost ���� 60�ʷ� ����
        mirror.tuto6 = true; // tuto6 �����ϵ��� ����
        StartCoroutine(CheckTuto6StatusAfterDelay(20f)); // 20�� ���� �ſ��� ������� ������ ���
    }

    private IEnumerator CheckTuto6StatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (mirror.tuto6)
        {
            playerGhostHealthManager.Die(); // 20�� ���� �ſ��� ������� ������ ���
        }
    }

}
