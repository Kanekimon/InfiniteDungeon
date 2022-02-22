using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuUiManager : MonoBehaviour
{
    public Canvas Ui;


    public GameObject SaveGameWindow;
    public GameObject baseMenu;



    public void OpenSaveGames()
    {
        SaveGameWindow.SetActive(true);
        CloseBaseMenu();
    }

    public void CloseSaveGame()
    {
        SaveGameWindow?.SetActive(false);
    }

    public void OpenBaseMenu()
    {
        this.baseMenu.SetActive(true);
        CloseSaveGame();
    }
    public void CloseBaseMenu()
    {
        this.baseMenu.SetActive(false);
    }

}

