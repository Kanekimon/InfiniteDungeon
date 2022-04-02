using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FollowPlayerState : NpcBaseState
{
    float outOfRangeTimer = 0;

    public override void EnterState(NPCStateMachine stateMachine)
    {
        stateMachine.GetComponent<BaseLogic>().SetContextBehaviour(new FollowContext());
    }

    public override void FixedUpdateState(NPCStateMachine stateMachine)
    {

        if (stateMachine.DistanceToTarget < 5f)
        {
            stateMachine.gameObject.GetComponent<BaseLogic>().Break();
            stateMachine.SwitchState(stateMachine.attack);
        }
        else
        {
            if (stateMachine.GetComponent<BaseLogic>().attractors.Count > 0)
            {
                if (stateMachine.DistanceToTarget > stateMachine.GetComponent<BaseLogic>().range * 1.5f)
                {
                    outOfRangeTimer += Time.fixedDeltaTime;
                    Debug.Log("Out of range since: " + outOfRangeTimer);
                    if (outOfRangeTimer > 5f)
                        stateMachine.SwitchState(stateMachine.wander);
                }
                else
                    outOfRangeTimer = 0;
                    

                stateMachine.gameObject.GetComponent<BaseLogic>().ChooseDirection();
            }
            else
                stateMachine.SwitchState(stateMachine.wander);
        }
    }

    public override void OnCollision(NPCStateMachine stateMachine, Collider2D collision)
    {

    }

    public override void UpdateState(NPCStateMachine stateMachine)
    {

    }
}

