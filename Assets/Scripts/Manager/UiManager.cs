using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public GameObject escMenu;
    public GameObject saveMenu;
    public Text currency;
    public Camera uiCam;
    public Camera playerCam;

    public GameObject currentOpen;

    public Dictionary<string, GameObject> windows = new Dictionary<string, GameObject>();

    public bool activeUiCam;

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
        playerCam = Camera.main;
        windows.Add("esc", escMenu);
        windows.Add("save", saveMenu);
    }

    public void SetUpCamera(Camera c)
    {
        //this.GetComponent<Canvas>().worldCamera = c;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu("esc");
        }
    }


    public void ToggleMenu(string menu)
    {
        if (windows.ContainsKey(menu))
        {
            if(currentOpen == null)
            {
                OpenMenu(menu);
            }
            else if(activeUiCam && windows[menu] == currentOpen)
            {
                CloseMenu();
            }
            else
            {
                ChangeWindow(menu);
            }
            uiCam.gameObject.SetActive(activeUiCam);
            GameManager.Instance.GamePaused = activeUiCam;
            GameManager.Instance.GetPlayer().SetActive(!activeUiCam);
            Time.timeScale = activeUiCam ? 0f : 1f;
        }

    }

    public void ChangeWindow(string menu)
    {
        currentOpen.SetActive(false);
        currentOpen = windows[menu];
        currentOpen.SetActive(true);
    }


    public void OpenMenu(string menu)
    {
        activeUiCam = true;
        currentOpen = windows[menu];
        currentOpen.SetActive(true);
        Camera.SetupCurrent(uiCam);
        uiCam.gameObject.transform.position = GameManager.Instance.GetPlayer().gameObject.transform.position;
    }

    public void CloseMenu()
    {
        activeUiCam = false;
        currentOpen = null;
        uiCam.gameObject.SetActive(false);
        Camera.SetupCurrent(playerCam);
        Time.timeScale = 1f;
    }


    public void ChangeCurrency(float value)
    {
        float val = float.Parse(currency.text.Replace("Money: ", ""));

        val += value;
        currency.text = "Money: " + val;

    }



}
