using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryWindow : MonoBehaviour
{
    public VisualTreeAsset itemSlot;
    public VisualTreeAsset craftingContent;
    public VisualTreeAsset equipmentContent;

    public static InventoryWindow Instance;

    delegate void ButtonCallBack();



    VisualElement root;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        AddItems();
    }


    public void AddItems()
    {
        VisualElement itemContainer = root.Q<VisualElement>("middle");

        for (int i = itemContainer.Children().Count(); i > 0; i--)
        {
            itemContainer.RemoveAt(i - 1);
        }

        InventorySystem inv = GameManager.Instance.GetPlayerSystem().InventorySystem;

        foreach (KeyValuePair<Item, int> item in inv.GetAllItems())
        {
            AddItemToUi(item.Key, item.Value);
        }


    }

    public void SetContentArea(string menuName)
    {
        root.Q<VisualElement>("content-container").Children().Last().RemoveFromHierarchy();
        if (menuName.Equals("crafting"))
        {
            root.Q<VisualElement>("content-container").Add(craftingContent.Instantiate().Q<VisualElement>("recipes-container"));
            CraftingMenu.Instance.SetupCrafting();
        }
        if (menuName.Equals("equipment"))
        {
            root.Q<VisualElement>("content-container").Add(equipmentContent.Instantiate().Q<VisualElement>("recipes-container"));
        }
    }

    public void AddItemToSlot(string slotName, Item i)
    {
        VisualElement slot = root.Q<VisualElement>(slotName);

        if (slot == null)
            return;

        if (i != null)
        {
            slot.style.backgroundImage = GetBackgroundImage(i.ItemName);
            slot.RegisterCallback<MouseDownEvent>((e) =>
            {
                if (e.button == 1)
                {
                    System.Action action = () =>
                    {
                        GameManager.Instance.GetPlayer().GetComponent<EquipmentSystem>().Unequip(i.equipmentType);
                        root.Q<VisualElement>("contextmenu").style.display = DisplayStyle.None;
                    };
                    SetUpContextMenu(e.mousePosition, i, "Unequip", action);
                }
                else if (e.button == 0)
                {
                    if (e.clickCount == 2)
                    {
                        GameManager.Instance.GetPlayer().GetComponent<EquipmentSystem>().Unequip(i.equipmentType);
                        root.Q<VisualElement>("contextmenu").style.display = DisplayStyle.None;
                    }

                }


            });
        }

        else
        {
            slot.style.backgroundImage = null;
        }

    }

    public Background GetBackgroundImage(string name)
    {
        return Background.FromSprite(Resources.Load<Sprite>($"items/{name}/{name}_icon"));
    }

    public void AddItemToUi(Item i, int amount)
    {
        VisualElement itemContainer = root.Q<VisualElement>("middle");
        TemplateContainer tmpContainer = itemSlot.Instantiate();
        VisualElement itemSlotElement = tmpContainer.Q<VisualElement>("item-container");
        itemSlotElement.name = i.ItemName;
        itemSlotElement.style.backgroundImage = GetBackgroundImage(i.ItemName);
        tmpContainer.Q<Label>("item-amount").text = "" + amount;
        itemContainer.Add(itemSlotElement);


        itemSlotElement.RegisterCallback<MouseDownEvent>((e) => { ClickHandler(itemSlotElement, e, i); });
    }

    private void ClickHandler(VisualElement itemSlotElement, MouseDownEvent e, Item i)
    {
        if (e.button == 1)
        {
            System.Action action = () =>
            {
                GameManager.Instance.GetPlayer().GetComponent<EquipmentSystem>().EquipItem(i);
                root.Q<VisualElement>("contextmenu").style.display = DisplayStyle.None;
            };
            SetUpContextMenu(e.mousePosition, i, "Equip", action);

        }
        else if (e.button == 0)
        {
            if (e.clickCount == 2)
            {
                GameManager.Instance.GetPlayer().GetComponent<EquipmentSystem>().EquipItem(i);
                root.Q<VisualElement>("contextmenu").style.display = DisplayStyle.None;
            }
            else if (e.clickCount == 1)
            {
                itemSlotElement.RegisterCallback<MouseMoveEvent>((m) =>
                {
                    GameObject.Find("Overlay").GetComponent<DragHelper>().Drag(itemSlotElement, m);
                });


            }
        }
    }

    void SetUpContextMenu(Vector3 mousePos, Item i, string buttonText, System.Action callback)
    {
        root.Q<VisualElement>("contextmenu").style.display = DisplayStyle.Flex;
        root.Q<VisualElement>("contextmenu").transform.position = mousePos;
        root.Q<Label>("context-header").text = i.ItemName;
        Button b = root.Q<Button>("equip");
        b.text = buttonText;
        b.clickable = null;
        b.clicked += callback;
    }

}
