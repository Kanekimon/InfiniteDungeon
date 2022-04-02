using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NpcBaseState
{
    public abstract void EnterState(NPCStateMachine stateMachine);
    public abstract void UpdateState(NPCStateMachine stateMachine);

    public abstract void FixedUpdateState(NPCStateMachine stateMachine);

    public abstract void OnCollision(NPCStateMachine stateMachine, Collider2D collision);

}

