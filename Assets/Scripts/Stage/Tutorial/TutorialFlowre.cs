using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlowre : Flowre
{
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private float tutorialAttackDamage = 0f;
    public bool OpenFlowre = false;

    protected override void Start()
    {
        attackDamage = tutorialAttackDamage;
        if (!playerStateMachine.isGhost) Close();
    }

    protected override void Update()
    {
        if(OpenFlowre) base.Update();
    }

    private void OnDestroy()
    {
        if(tutorial!= null) tutorial.Tuto4_flowre();
    }
}
