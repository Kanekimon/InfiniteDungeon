using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ProjectilePoolManager : MonoBehaviour
{

    public static ProjectilePoolManager Instance;

    public GameObject pool;

    public List<GameObject> availabel = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"));
            g.transform.parent = pool.transform;
            g.name = $"Pro {i}";
            availabel.Add(g);

            g.SetActive(false);
        }
    }

    public GameObject GetProjectileFromPool()
    {
        if (availabel.Count > 0)
        {
            GameObject g = availabel.First();
            availabel.RemoveAt(0);
            return g;
        }
        return null;
    }

    public void AddToPool(GameObject g)
    {
        g.transform.parent = pool.transform;
        g.SetActive(false);
        if (!availabel.Contains(g))
            availabel.Add(g);
    }

}
