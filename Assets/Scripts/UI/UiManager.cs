using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class UiManager : MonoBehaviour
    {

        public static UiManager Instance;
        VisualElement root;
        Dictionary<string, GameObject> uiWindows = new Dictionary<string, GameObject>();
        GameObject currentActive = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);


            foreach (Transform menu in this.transform)
            {
                if (menu != null)
                {
                    Debug.Log($"Window Name: " + menu.name);
                    uiWindows.Add(menu.name, menu.gameObject);
                }
            }

        }

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void Start()
        {


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentActive != null)
                    ToggleMenu(currentActive.name);
                else
                    ToggleMenu("esc-menu");
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                ToggleMenu("upgrade-menu");
            }
        }

        public void ToggleMenu(string menuName)
        {
            GameObject window = uiWindows[menuName];
            bool newstate = !window.activeInHierarchy;


            if (newstate)
            {
                if (currentActive != null)
                {
                    ToggleMenu(currentActive.name);
                }
                currentActive = window;
            }
            else
                currentActive = null;

            window.SetActive(newstate);

            //VisualElement men = root.Q<VisualElement>(menuName);
            //men.style.display = men.style.display != DisplayStyle.Flex ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void ChangeCurrency(float value)
        {
            Label money = root.Q<Label>("money");
            string currentCurrency = money.text.Replace("Money: ", "");
            Debug.Log(currentCurrency);
            if (currentCurrency == string.Empty)
                currentCurrency = "0";
            float val = float.Parse(currentCurrency);

            val += value;
            money.text = $"Money: {val:00}";
        }

        public void ChangeDepth(Room room)
        {
            root.Q<Label>("level-value").text = "" + room.depth;
        }


    }
}
