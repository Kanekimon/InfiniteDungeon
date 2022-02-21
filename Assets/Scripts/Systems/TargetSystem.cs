using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public LayerMask layermask;
    public Vector2 targetDirection;
    public float Range;

    public float DistanceToTarget;
    public bool obstacleInDirection;

    private NpcMovement move;
    private Attack attack;



    public List<RaycastHit2D> hits = new List<RaycastHit2D>();

    private void Start()
    {
        move = this.GetComponent<NpcMovement>();
        attack = this.GetComponent<Attack>();
        CheckRayHits();
    }

    private void Update()
    {
        CheckRayHits();

        GameObject player = CheckIfTargetIsInRange();
        
        GameObject target = player != null ? player : move.targetObject;

        if (target != null)
        {
            if (target.tag == "Player")
            {
                obstacleInDirection = false;
                move.targetObject = target;
                DistanceToTarget = Vector2.Distance((Vector2)target.transform.position, (Vector2)this.transform.position);
                if (DistanceToTarget <= Range)
                {
                    Vector2 normShotDir = ((Vector2)target.transform.position);
                    attack.Shot(normShotDir);
                    move.ToggleMovement(false);
                }
                else
                {
                    move.ToggleMovement(true);
                    if (DistanceToTarget >= Range * 1.5)
                        move.targetObject = null;
                }
            }
            else
            {
                obstacleInDirection = true;
            }
        }

        if (CheckForObstacles())
        {
            move.ChangeDirection();
        }

    }

    public bool CheckForObstacles()
    {
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Wall")
                return true;
        }

        return false;
    }

    public GameObject CheckIfTargetIsInRange()
    {

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                return hit.collider.gameObject;
        }

        return null;
    }

    private void CheckRayHits()
    {
        hits.Clear();
        hits.Add(Physics2D.Raycast(transform.position, targetDirection, 10, layermask));
        hits.Add(Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 45) * targetDirection, 10, layermask));
        hits.Add(Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -45) * targetDirection, 10, layermask));
    }




    private void OnDrawGizmos()
    {

    }

}
