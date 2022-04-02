using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState : NpcBaseState
{
    public override void EnterState(NPCStateMachine stateMachine)
    {

    }

    public override void FixedUpdateState(NPCStateMachine stateMachine)
    {
        
    }

    public override void OnCollision(NPCStateMachine stateMachine, Collider2D collision)
    {

    }

    public override void UpdateState(NPCStateMachine stateMachine)
    {
        if (stateMachine.DistanceToTarget <= 5)
            Debug.Log("Pew pew");
        else
            stateMachine.SwitchState(stateMachine.followPlayer);
    }
}
