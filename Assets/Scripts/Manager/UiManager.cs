using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public Text currency;

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

    }

    public void SetUpCamera(Camera c)
    {
        //this.GetComponent<Canvas>().worldCamera = c;

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ChangeCurrency(float value)
    {
        float val = float.Parse(currency.text.Replace("Money: ", ""));

        val += value;
        currency.text = "Money: " + val;

    }

}
