using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{

    public NpcBaseState currentState;

    public WanderState wander = new WanderState();
    public AttackState attack = new AttackState();
    public FollowPlayerState followPlayer = new FollowPlayerState();
    public StrafeState trafe = new StrafeState();
    public DieState die = new DieState();


    public GameObject Target;
    public float DistanceToTarget;


    private void Start()
    {
        currentState = wander;
        currentState.EnterState(this);

    }


    private void Update()
    {
        if(Target != null)
            DistanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);

        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(NpcBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);

        Debug.Log("Entered State: " + state);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isInLayer = this.GetComponent<BaseLogic>().targets == (this.GetComponent<BaseLogic>().targets | (1 << collision.gameObject.layer));

        if (isInLayer)
        {
            if(Target == null)
                Target = collision.gameObject;
            currentState.OnCollision(this, collision);
        }

        if (Target != null && Target != collision.gameObject && isInLayer)
        {
            if(Vector2.Distance(this.transform.position, Target.transform.position) > Vector2.Distance(this.transform.position, collision.transform.position))
            {
                Target = collision.gameObject;
            }
            DistanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);

        }



        
    }



}

