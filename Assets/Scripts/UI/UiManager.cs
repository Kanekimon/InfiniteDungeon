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
        public GameObject WindowParent;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);


            foreach (Transform menu in WindowParent.transform)
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

            if (Input.GetKeyDown(KeyCode.I))
            {
                if(ToggleMenu("inventory-menu"))
                    InventoryWindow.Instance.SetContentArea("equipment");


            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if(ToggleMenu("inventory-menu"))
                    InventoryWindow.Instance.SetContentArea("crafting");
            }
        }

        public bool ToggleMenu(string menuName)
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
            return newstate;
            //VisualElement men = root.Q<VisualElement>(menuName);
            //men.style.display = men.style.display != DisplayStyle.Flex ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void SetHp(float maxhp, float currenthp)
        {
            VisualElement container = root.Q<VisualElement>("hp-container");
            VisualElement bar = root.Q<VisualElement>("hp-bar");
            Label hp_value = root.Q<Label>("hp-value");

            float percHealth = (currenthp / maxhp) * 100;

            bar.style.width = new Length(percHealth, LengthUnit.Percent);

            Color color = Color.green;

            if (percHealth < 20f)
                color = Color.red;
            else if (percHealth >= 20f && percHealth < 75f)
                color = Color.yellow;

            bar.style.backgroundColor = color;

            container.style.backgroundColor = Color.Lerp(color, Color.black, .5f);


            hp_value.text = $"{currenthp} / {maxhp}";
        }


        public void SetExp(float maxExp, float currentExp, int currentLevel)
        {
            VisualElement container = root.Q<VisualElement>("exp-container");
            VisualElement bar = root.Q<VisualElement>("exp-bar");
            Label exp_value = root.Q<Label>("exp-value");

            Label level = root.Q<Label>("exp-label");
            level.text = $"Exp ( Current Level: {currentLevel} )";

            float percHealth = (currentExp / maxExp) * 100;

            bar.style.width = new Length(percHealth, LengthUnit.Percent);

            Color color = Color.cyan;

            bar.style.backgroundColor = color;

            container.style.backgroundColor = Color.Lerp(color, Color.black, .5f);



            exp_value.text = $"{currentExp} / {maxExp}";
        }


        public void ChangeCurrency(float value)
        {
            Label money = root.Q<Label>("money");
            string currentCurrency = money.text.Replace("Money: ", "");
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


        public void UpdateInventory()
        {
            if (currentActive != null && currentActive.name.Contains("inventory"))
            {
                currentActive.GetComponent<InventoryWindow>().AddItems();
            }
        }


        public void UpdateEquipment(Dictionary<EquipmentType, Item> equipment)
        {
            foreach (KeyValuePair<EquipmentType, Item> item in equipment)
            {
                string slotName = $"slot-{item.Key}";
                uiWindows["inventory-menu"].GetComponent<InventoryWindow>().AddItemToSlot(slotName, item.Value);
            }
        }

    }
}
