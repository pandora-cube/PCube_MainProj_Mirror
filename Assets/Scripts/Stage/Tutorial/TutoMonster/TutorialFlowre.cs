using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlowre : FlowreBehaviourController
{
    [SerializeField] private TutorialMirror tutorial;
    [SerializeField] private int tutorialAttackDamage = 0;
    public bool OpenFlowre = false;

    protected override void Start()
    {
        base.Start();
        attackDamage = tutorialAttackDamage;
    }

    protected override void Update()
    {
        if (OpenFlowre) base.Update();
    }

    private void OnDestroy()
    {
        if (tutorial != null) tutorial.Tuto4_flowre();
    }

    public override IEnumerator TriggerAttackAnimation()
    {
        yield return base.TriggerAttackAnimation();  
    }
}
