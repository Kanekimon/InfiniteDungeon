using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataObject
{
    public Vector2 Position;
    public float Distance;
    public float Dot;
}

public class BaseLogic : MonoBehaviour
{

    public Vector2[] Directions;
    public int numberOfRays = 12;
    public GameObject entity;
    Vector2 choosenDirection;

    List<DataObject> dataObjects = new List<DataObject>();


    public List<GameObject> attractors = new List<GameObject>();

    public LayerMask targets;
    public LayerMask avoid;

    Rigidbody2D rb;

    public float range = 10f;

    public Vector2 TargetPoint;

    public float delay;
    public float timer;

    public float speed;

    Vector2 entityPos;

    float[] DangerMap;
    float[] InterestMap;

    public bool DrawGizmos;
    public bool ShowDanger;
    public bool ShowInterest;

    public bool breaking = false;
    public bool knockedBack = false;

    public IContextController contextBehaviour;


    // Start is called before the first frame update
    void Start()
    {
        entity = this.gameObject;
        rb = this.GetComponent<Rigidbody2D>();
        this.transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>().radius = range;
    }

    // Update is called once per frame
    void Update()
    {
        entityPos = this.transform.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isInLayer = targets == (targets | (1 << collision.gameObject.layer));

        if (isInLayer)
        {
            attractors.Add(collision.gameObject);
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    bool isInLayer = targets == (targets | (1 << collision.gameObject.layer));

    //    if (isInLayer)
    //    {
    //        attractors.Remove(collision.gameObject);
    //    }
    //}

    public void RemoveAttractor(GameObject g)
    {
        if(attractors.Contains(g))
            attractors.Remove(g);
    }



    Vector2 GetDirectionNormalized(Vector2 from, Vector2 to)
    {
        return (from - to).normalized;
    }


    public void GenerateFile()
    {
        List<DataObject> sorted = dataObjects.OrderBy(x => x.Distance).ToList();
        StringBuilder str = new StringBuilder();

        foreach (DataObject obj in sorted)
        {
            str.AppendLine($"{obj.Position};{obj.Distance};{obj.Dot}");
        }

        string url = @"D:\Log\Compare.csv";


        StreamWriter writer = new StreamWriter(url);
        writer.Write(str.ToString());
        writer.Dispose();
    }


    public void Break()
    {
        if(!breaking)
            breaking = true;
    }

    public void GetKnockedBack(Vector2 knock)
    {
        
        this.choosenDirection = knock;
    }

    public void SetContextBehaviour(IContextController beh)
    {
        this.contextBehaviour = beh;
    }

    private void OnDrawGizmos()
    {
        if (ShowInterest)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(entityPos, (Directions[i] * InterestMap[i]));
                Gizmos.color = Color.white;
            }
        }

        if (ShowDanger)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(entityPos, (Directions[i] * DangerMap[i]));
                Gizmos.color = Color.white;

            }
        }

    }

    public Vector2 ChooseDirection()
    {
        breaking = false;
        UpdateDirections();
        UpdateDangerMap();
        UpdateInterestsMap();

        choosenDirection = Vector2.zero;

        for (int i = 0; i < numberOfRays; i++)
        {
            if (DangerMap[i] > 0)
            {
                InterestMap[i] = Mathf.Clamp01(InterestMap[i] - DangerMap[i]);
                int opIndex = (numberOfRays-1) - (i);
                try
                {
                    InterestMap[opIndex] = Mathf.Clamp01(InterestMap[opIndex] + DangerMap[i]);
                }
                catch(System.Exception ex)
                {
                    Debug.Log($"org i {i} mod i {opIndex}");
                    Debug.LogException(ex);
                }
            }

            choosenDirection += Directions[i] * InterestMap[i];

        }

        TargetPoint = choosenDirection.normalized + entityPos;
        //

        return choosenDirection.normalized;

    }

    void UpdateDirections()
    {
        Directions = new Vector2[numberOfRays];

        float angle = 360 / numberOfRays;

        for (int i = 0; i < numberOfRays; i++)
        {
            Directions[i] = (Vector2)(Quaternion.AngleAxis(angle * i, Vector3.forward) * Vector2.up);
        }
    }

    void UpdateDangerMap()
    {
        DangerMap = new float[numberOfRays];

        for (int i = 0; i < numberOfRays; i++)
        {
            Vector2 rotated = Directions[i] + entityPos;
            RaycastHit2D hit = Physics2D.Raycast(entityPos, (rotated - entityPos), range, avoid, 0);
            Color c = Color.green;
            if (hit.collider != null)
            {

                if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject != this.transform.GetChild(0).gameObject)
                {
                    float distance = hit.distance;
                    DangerMap[i] = 1 - (Mathf.Clamp(distance, 0, range) / range);
                }
                else
                    DangerMap[i] = 0;
            }

            //Debug.DrawRay(entityPos, (rotated - entityPos) * range, c);
        }
    }

    void UpdateInterestsMap()
    {
        InterestMap = contextBehaviour.UpdateInterestMap(entityPos, numberOfRays, Directions, this);
    }


    public GameObject ClosestAttractor()
    {

        GameObject closest = null;
        float minDistance = float.MaxValue;
        foreach (GameObject attractor in attractors)
        {
            if (attractor == null)
                continue;

            if (closest == null)
                closest = attractor;
            else
            {
                float dist = Vector2.Distance((Vector2)attractor.transform.position, entityPos);
                if (minDistance > dist)
                {
                    minDistance = dist;
                    closest = attractor;
                }
            }

        }

        return closest;
    }

    private void FixedUpdate()
    {
        if (timer > delay && !breaking)
        {
            rb.velocity = choosenDirection * speed * Time.fixedDeltaTime;
            timer = 0;
        }
        if (breaking && !knockedBack)
            rb.velocity = Vector2.zero;
        timer += Time.fixedDeltaTime;
    }
}
