using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject player;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Room r = RoomManager.Instance.InitialRoom();
        
        
        SpawnPlayer(r.center);

        UiManager.Instance.SetUpCamera(GetCamera());
    }

    public void SetPlayerPos(Vector2 pos)
    {
        this.player.transform.position = pos;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public Camera GetCamera()
    {
        return this.player.transform.Find("Camera").GetComponent<Camera>();
    }


    public void SpawnPlayer(Vector3 pos)
    {
         //Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        pos.z = 0;
        player.transform.position = pos;
    }

    public void SpawnEnemies()
    {
        int nOe = UnityEngine.Random.Range(0, 100);

        for(int i = 0; i < nOe; i++)
        {
            float x = Random.Range(1f, 100f);
            float y = Random.Range(1f, 100f);

            GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Enemo"));

            e.transform.position = new Vector3(x, y, 0);

        }
    }

}
