using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    GameObject owner;

    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    float currency;


    private void Awake()
    {
        this.owner = this.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ChangeCurrency(100);

    }


    /// <summary>
    /// Returns total currency count
    /// </summary>
    /// <returns>Currency</returns>
    public float GetCurrency()
    {
        return currency;
    }

    /// <summary>
    /// Adds or removes currency from inventory
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeCurrency(float amount)
    {
        if (amount < 0)
        {
            if ((currency - amount) < 0)
                return;
        }

        this.currency += amount;

        if (this.gameObject.CompareTag("Player"))
        {
            UiManager.Instance.ChangeCurrency(currency);

            //oldUiManager.Instance.ChangeCurrency(amount);
        }

    }


    /// <summary>
    /// Adds specific number of item to inventory
    /// </summary>
    /// <param name="i">Item to add</param>
    /// <param name="amount">Amount of item</param>
    public void AddItem(Item i, int amount)
    {
        //Debug.Log(i);
        try
        {
            Item item = null;

            if (i.ItemName.Equals("gold"))
            {
                ChangeCurrency(amount);
            }
            else
            {

                if (items.Any(a => a.Key.ItemId != i.ItemId))
                    item = items.Where(a => a.Key.ItemId == i.ItemId).FirstOrDefault().Key;

                if (item != null)
                    items[item] += amount;
                else
                    items[i] = amount;
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log(i.ItemName);
            Debug.LogException(ex);
        }
        UiManager.Instance.UpdateInventory();

    }

    /// <summary>
    /// Removes specifc amount of item from inventory
    /// if item amount is 0 afterwards the item will be completly removed
    /// </summary>
    /// <param name="i">Item</param>
    /// <param name="amount">Amount to remove</param>
    public void RemoveItem(Item i, int amount)
    {
        if (HasItem(i) && HasEnoughOfItem(i, amount))
        {
            Item fromInv = GetItemFromInventory(i);

            items[fromInv] -= amount;
            if (items[fromInv] == 0)
                items.Remove(fromInv);
        }
        UiManager.Instance.UpdateInventory();

    }


    public bool HasItem(Item i)
    {
        return items.Any(a => a.Key.ItemId == i.ItemId);
    }

    /// <summary>
    /// Returns the amount of a specifc item
    /// </summary>
    /// <param name="i">Item</param>
    /// <returns>Quantity of item or -1 if not in inventory</returns>
    public int GetItemAmount(Item i)
    {
        if(HasItem(i))
            return items.Where(a => a.Key.ItemId == i.ItemId).FirstOrDefault().Value;

        return -1;
    }

    /// <summary>
    /// Checks if inventory contains item with specific amount
    /// </summary>
    /// <param name="i">Item</param>
    /// <param name="amount">Check amount</param>
    /// <returns>False if item not in inventory or quantity to little / True otherwise</returns>
    public bool HasEnoughOfItem(Item i, int amount)
    {
        int itemAmount = GetItemAmount(i);
        if (itemAmount == -1 || itemAmount < amount)
            return false;
        return true;
    }

    /// <summary>
    /// Returns all Items
    /// </summary>
    /// <returns></returns>
    public Dictionary<Item, int> GetAllItems()
    {
        return items;
    }

    public Item GetItemFromInventory(Item i)
    {
        return items.Where(a => a.Key.ItemId == i.ItemId).FirstOrDefault().Key;
    }

}
