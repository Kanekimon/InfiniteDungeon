using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

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
}
