using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{

    public float Speed = 10f;

    public float timer;
    public float delay = 3f;
    public Vector2 direction;
    public GameObject targetObject;

    Rigidbody2D rb;
    public bool run = true;

    private TargetSystem targetSystem;
    private AttributeSystem attributeSystem;
    private FoVTargetSystem fov;
    private Attack attack;


    private Room r;
    private List<Vector2> nodes = new List<Vector2>();
    private Vector2 nextNode = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        targetSystem = this.GetComponent<TargetSystem>();
        attributeSystem = this.GetComponent<AttributeSystem>();
        r = this.GetComponent<NpcBase>().r;
        fov = this.GetComponent<FoVTargetSystem>();
        attack = this.GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ToggleMovement(bool state)
    {
        run = state;
    }

    private void FixedUpdate()
    {
        if (!run)
        {
            if (fov.player == null || (fov.player != null && Vector2.Distance((Vector2)fov.player.position, (Vector2)this.transform.position) > fov.combatRadius))
                run = true;
            else
            {
                attack.Shot(fov.player.transform.position);
            }
        }

        if (run)
        {

            if (fov.player != null)
            {
                if (Vector2.Distance((Vector2)fov.player.position, (Vector2)this.transform.position) > fov.combatRadius)
                {
                    GetPathToPlayer();
                    if (nodes.Count > 0)
                    {
                        nextNode = nodes.Last();
                        nodes.Remove(nodes.Last());
                    }
                }
                else
                {
                    run = false;
                }


            }

            if (nodes.Count == 0)
            {
                if (nextNode == Vector2.zero || Vector2.Distance(nextNode, (Vector2)this.transform.position) < 0.01f)
                {
                    GetNewPath();
                    if (nodes.Count > 0)
                    {
                        nextNode = nodes.Last();
                        nodes.Remove(nodes.Last());
                    }
                }
            }
            else
            {
                if (Vector2.Distance(nextNode, (Vector2)this.transform.position) < 0.1f)
                {
                    nextNode = nodes.Last();
                    nodes.Remove(nodes.Last());
                }
            }
            direction = MyIdeaOfNormalized((nextNode - (Vector2)this.transform.position).normalized);

            rb.MoveRotation(Quaternion.LookRotation(direction));
            rb.MovePosition(rb.position + direction * (attributeSystem.GetAttributeValue("dex") / 2) * Time.fixedDeltaTime * NPCManager.Instance.SpeedModifier);


            timer += Time.fixedDeltaTime;
        }

    }

    private Vector2 MyIdeaOfNormalized(Vector2 input)
    {
        Vector2 smallest = new Vector2(float.MaxValue, float.MaxValue);
        if (Mathf.Abs(Vector2.Distance(smallest, input)) > Mathf.Abs(Vector2.Distance(input, Vector2.up)))
            smallest = Vector2.up;
        if (Mathf.Abs(Vector2.Distance(smallest, input)) > Mathf.Abs(Vector2.Distance(input, Vector2.down)))
            smallest = Vector2.down;
        if (Mathf.Abs(Vector2.Distance(smallest, input)) > Mathf.Abs(Vector2.Distance(input, Vector2.left)))
            smallest = Vector2.left;
        if (Mathf.Abs(Vector2.Distance(smallest, input)) > Mathf.Abs(Vector2.Distance(input, Vector2.right)))
            smallest = Vector2.right;

        return smallest;
    }

    private void GetNewPath()
    {
        float currentX = this.transform.position.x;
        float currentY = this.transform.position.y;


        int newPosX = (int)Mathf.Clamp(UnityEngine.Random.Range(currentX - 10, currentX + 10), r.bounds.startX + 2, r.bounds.endX - 2);
        int newPosY = (int)Mathf.Clamp(UnityEngine.Random.Range(currentY - 10, currentY + 10), r.bounds.startY + 2, r.bounds.endY - 2);

        Vector2 targetPos = new Vector2(newPosX, newPosY);
        while (!RoomManager.Instance.IsTileWalkable(r, newPosX, newPosY))
        {

            newPosX = (int)Mathf.Clamp(UnityEngine.Random.Range(currentX - 10, currentX + 10), r.bounds.startX + 2, r.bounds.endX - 2);
            newPosY = (int)Mathf.Clamp(UnityEngine.Random.Range(currentY - 10, currentY + 10), r.bounds.startY + 2, r.bounds.endY - 2);

            targetPos = new Vector2(newPosX, newPosY);
        }

        nodes = RandomPathGenerator.GenerateRandomPath(r, this.transform.position, targetPos, PathMode.shortest);
    }

    private void GetPathToPlayer()
    {
        nodes = RandomPathGenerator.GenerateRandomPath(r, this.transform.position, fov.player.position, PathMode.shortest);
    }


    private Vector2 GetAverageVector()
    {
        List<GameObject> boids = this.transform.GetChild(0).GetComponent<BoidSystem>().nearby;
        Vector2 dir = Vector2.zero;

        if (boids.Count == 0)
            return dir;

        foreach (GameObject boid in boids)
        {
            dir += (Vector2)boid.transform.position;
        }

        return (dir / boids.Count).normalized;
    }

    public void SetTargetPos(Vector2 targetP)
    {
        //Debug.Log("Changing Direction");
        //direction = (targetP - (Vector2)this.transform.position).normalized;
        //targetPos = targetP;
    }


    private Vector2 GetRandomVector()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.white;
        foreach (Vector2 node in nodes)
        {
            if (fov.player != null)
                Gizmos.color = Color.red;
            Gizmos.DrawSphere(node, 0.1f);
        }
    }

}
