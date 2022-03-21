using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryWindow : MonoBehaviour
{
    public VisualTreeAsset itemSlot;

    VisualElement root;

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


    public void AddItemToUi(Item i, int amount)
    {
        VisualElement itemContainer = root.Q<VisualElement>("middle");

        TemplateContainer tmpContainer = itemSlot.Instantiate();

        VisualElement itemSlotElement = tmpContainer.Q<VisualElement>("item-container");

        itemSlotElement.style.backgroundImage = Background.FromSprite(Resources.Load<Sprite>($"items/{i.ItemName}"));

        tmpContainer.Q<Label>("item-amount").text = "" + amount;

        itemContainer.Add(itemSlotElement);
    }

}
