using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowreAnimationController : EnemyBaseAnimationController<FlowreAnimationController.FlowreAnimationStates>
{
    public enum FlowreAnimationStates
    {
        flowreClosed,
        flowreIdle,
        flowreAttack
    }
}
