using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WanderState : NpcBaseState
{
    public override void EnterState(NPCStateMachine stateMachine)
    {
        stateMachine.GetComponent<BaseLogic>().SetContextBehaviour(new WanderContext());
    }

    public override void FixedUpdateState(NPCStateMachine stateMachine)
    {
        stateMachine.gameObject.GetComponent<BaseLogic>().ChooseDirection();
    }

    public override void OnCollision(NPCStateMachine stateMachine, Collider2D collision)
    {
        Debug.Log("Collided");
        stateMachine.SwitchState(stateMachine.followPlayer);
    }


    public override void UpdateState(NPCStateMachine stateMachine)
    {
       
    }
}

