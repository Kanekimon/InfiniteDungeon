using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FoVTargetSystem : MonoBehaviour
{
    public float viewRadius;
    public float combatRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Transform player = null;
    public bool DetectTargets = true;

    public bool ShowRays = false;

    private void Start()
    {
        player = null;
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (DetectTargets)
        {
            yield return new WaitForSeconds(delay);

            AvoidObstacles();
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        player = null;
        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {

            Transform target = targetInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(this.GetComponent<NpcMovement>().direction, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);
                if (Physics2D.Raycast(transform.position, target.position, dstToTarget, obstacleMask).collider == null)
                {
                    Debug.Log("add");
                    player = target;
                }
            }

        }
    }

    void AvoidObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.GetComponent<NpcMovement>().direction, 2f, obstacleMask);
        //ShowRays = hit.collider != null;
        if (hit.collider != null)
        {
            //ShowRays = true;
            //StartAvoidance();
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }


    void StartAvoidance()
    {

        Debug.Log("Avoiding");
        Vector2 fowPos = this.transform.position;

        Vector2 baseDir = this.GetComponent<NpcMovement>().direction;
        RaycastHit2D oFa = new RaycastHit2D();
        int smallestIndex = -1;

        int perAngle = (int)(viewAngle / 2) / 10;

        for (int i = -(int)(viewAngle / 2); i < (int)(viewAngle / 2); i += perAngle)
        {
            Vector2 rotAngle = Quaternion.Euler(0, 0, i) * baseDir;

            RaycastHit2D hit = Physics2D.Raycast(fowPos, (Vector2)fowPos + rotAngle.normalized, 100f, obstacleMask);

            if (hit.collider == null)
                this.GetComponent<NpcMovement>().SetTargetPos((Vector2)fowPos + rotAngle.normalized);

            if (smallestIndex == -1)
            {
                oFa = hit;
                smallestIndex = i;
            }
            else
            {
                if (oFa.distance < hit.distance)
                {
                    smallestIndex = i;
                    oFa = hit;
                }
            }
        }

        if (smallestIndex != -1)
            this.GetComponent<NpcMovement>().SetTargetPos((Vector2)fowPos + (Vector2)(Quaternion.Euler(0, 0, smallestIndex) * baseDir));

    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.white;

        Vector3 fowPos = this.transform.position;
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(fowPos, player.position);
        }


        if (ShowRays)
        {
            Vector2 baseDir = this.GetComponent<NpcMovement>().direction;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(fowPos, (Vector2)fowPos + baseDir);

            Gizmos.color = Color.magenta;


            RaycastHit2D oFa = new RaycastHit2D();
            int smallestIndex = -1;

            int perAngle = (int)(viewAngle/2) / 10;

            for (int i = -(int)(viewAngle/2); i < (int)(viewAngle/2); i += perAngle)
            {
                Vector2 rotAngle = Quaternion.Euler(0, 0, i) * baseDir;
                Gizmos.DrawLine(fowPos, rotAngle.normalized + (Vector2)fowPos);

                RaycastHit2D hit = Physics2D.Raycast(fowPos, (Vector2)fowPos + rotAngle.normalized, 100f, obstacleMask);

                if (hit.collider == null)
                    this.GetComponent<NpcMovement>().SetTargetPos((Vector2)fowPos + rotAngle.normalized);

                if (smallestIndex == -1)
                {
                    oFa = hit;
                    smallestIndex = i;
                }
                else
                {
                    if(oFa.distance < hit.distance)
                    {
                        smallestIndex = i;
                        oFa = hit;
                    }
                }
            }

            if(smallestIndex != -1)
                this.GetComponent<NpcMovement>().SetTargetPos((Vector2)fowPos + (Vector2)( Quaternion.Euler(0, 0, smallestIndex) * baseDir));

            Gizmos.DrawSphere((Vector2)fowPos + (Vector2)(Quaternion.Euler(0, 0, smallestIndex) * baseDir), 0.5f);
        }


    }

}

