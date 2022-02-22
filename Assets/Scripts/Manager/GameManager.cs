using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject player;

    public bool GamePaused;

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

        if(StartParameters.newGame == true)
            RoomManager.Instance.InitialRoom();
        else
        {
            SaveStateManager.Instance.LoadGame();
        }
        Room r = RoomManager.Instance.GetCurrentRoom();
        SpawnPlayer(r.center);

        Time.timeScale = 1f;

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

    /// <summary>
    /// Returns the player object
    /// </summary>
    /// <returns>Player Gameobject</returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// Returns the Player Camera
    /// </summary>
    /// <returns>Player Camera</returns>
    public Camera GetCamera()
    {
        return this.player.transform.Find("Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// Spawns the player at a given position
    /// </summary>
    /// <param name="pos">Spawnpoint</param>
    public void SpawnPlayer(Vector3 pos)
    {
         //Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        pos.z = 0;
        player.transform.position = pos;
    }

    /// <summary>
    /// Spawns a random amount of enemies at random positions
    /// </summary>
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

    /// <summary>
    /// Exit back to the main menu
    /// </summary>
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
