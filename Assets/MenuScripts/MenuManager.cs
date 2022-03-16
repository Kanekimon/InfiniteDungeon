using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void StartGame()
    {
        StartParameters.newGame = true;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Loads a savestate and starts the game with it
    /// </summary>
    /// <param name="saveGame">index of savegame</param>
    public void LoadGame(int saveGame)
    {
        StartParameters.saveGame = saveGame;
        StartParameters.newGame = false;
        SceneManager.LoadScene("GameScene");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
