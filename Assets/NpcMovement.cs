using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        targetSystem = this.GetComponent<TargetSystem>();
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
        if (run)
        {
            if (timer > delay)
            {
                delay = Random.Range(0.1f, 2f);
                direction = (targetObject == null ? GetRandomVector() : ((Vector2)targetObject.transform.position) - (Vector2)this.transform.position);
                direction.Normalize();
                timer = 0;

                this.GetComponent<TargetSystem>().targetDirection = direction;

            }
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);

            timer += Time.fixedDeltaTime;
        }
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

    private Vector2 AvoidObstacle()
    {
        if (this.GetComponent<TargetSystem>().obstacleInDirection)
            return GetRandomVector();
        return Vector2.zero;
    }

    public void ChangeDirection()
    {
        this.direction = direction * -1;
    }



    private Vector2 GetDirection()
    {
        Vector2 dir = GetAverageVector() + AvoidObstacle() + GetRandomVector();
        return dir;
    }

    private Vector2 GetRandomVector()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector2 dir0 = direction * 10f;
        Gizmos.DrawRay(this.transform.position, dir0);
    }

}
