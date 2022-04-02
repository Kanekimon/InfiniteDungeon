using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StrafeState : NpcBaseState
{
    public override void EnterState(NPCStateMachine stateMachine)
    {
        stateMachine.GetComponent<BaseLogic>().SetContextBehaviour(new StrafeContext());
    }

    public override void FixedUpdateState(NPCStateMachine stateMachine)
    {
       
    }

    public override void OnCollision(NPCStateMachine stateMachine, Collider2D collision)
    {
        
    }

    public override void UpdateState(NPCStateMachine stateMachine)
    {
        throw new NotImplementedException();
    }
}

