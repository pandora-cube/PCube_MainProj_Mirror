using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlowre : FlowreBehaviourController
{
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private int tutorialAttackDamage = 0;
    public bool OpenFlowre = false;

    protected override void Start()
    {
        attackDamage = tutorialAttackDamage;
        if (PlayerStateMachine.instance.isGhost) Close();
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
