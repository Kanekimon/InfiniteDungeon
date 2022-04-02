using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public bool Random;
    public bool start;

    public float LastExecutionTime;

    public int spawnCount;

    public GameObject container;
    public GameObject poolContainer;


    public int reps;

    float rando;
    float zero;
    float pool;

    float timer;
    public float delay;
    int counter;
    bool finishedSim = true;

    public List<GameObject> tiles = new List<GameObject>();
    public List<GameObject> active = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"));
            g.transform.parent = poolContainer.transform;
            g.SetActive(false);
            tiles.Add(g);
        }

    }

    private void Testing()
    {
        finishedSim = false;


        if (container != null)
            Destroy(container);

        container = new GameObject("Container");


        if (Random)
        {
            ReAddActiveToPool();
        }

        System.Diagnostics.Stopwatch stp = new System.Diagnostics.Stopwatch();
        stp.Start();
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject e = null;
            if (Random)
            {
                e = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"));
                e.transform.parent = container.transform;
            }
            else
            {
                e = GetFromPool();
                active.Add(e);
            }

            int x = UnityEngine.Random.Range(0, 1000);
            int y = UnityEngine.Random.Range(0, 1000);

            e.transform.position = new Vector3(x, y, 0);

        }
        stp.Stop();
        LastExecutionTime = stp.ElapsedMilliseconds;


        if (Random)
            rando += LastExecutionTime;
        else
            pool += LastExecutionTime;




        finishedSim = true;
    }

    public void ReAddActiveToPool()
    {
        if (active.Count > 0)
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                GameObject e = active[i];
                e.SetActive(false);
                active.RemoveAt(i);
                tiles.Add(e);
            }
        }
    }

    public GameObject GetFromPool()
    {
        GameObject f = tiles.First();
        tiles.RemoveAt(0);
        f.SetActive(true);
        return f;
    }


    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (timer > delay && finishedSim)
            {
                Random = !Random;
                if (counter < reps)
                {
                    Testing();
                    counter++;
                }

                else
                {
                    start = false;
                    Debug.Log($"Time with random {rando / reps} miliseconds on average");
                    Debug.Log($"Time without random {pool / reps} miliseconds on average");
                }


                timer = 0;
            }

            timer += Time.deltaTime;
        }
    }
}
