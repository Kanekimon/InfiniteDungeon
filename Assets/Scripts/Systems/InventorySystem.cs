using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        if(amount < 0)
        {
            if ((currency - amount) < 0)
                return;
        }

        this.currency += amount;

        if (this.gameObject.CompareTag("Player"))
        {
            UiManager.Instance.ChangeCurrency(amount);
        }

    }


    /// <summary>
    /// Adds specific number of item to inventory
    /// </summary>
    /// <param name="i">Item to add</param>
    /// <param name="amount">Amount of item</param>
    public void AddItem(Item i, int amount)
    {
        if (items.ContainsKey(i))
            items[i] += amount;
        else
            items[i] = amount;
    }

    /// <summary>
    /// Removes specifc amount of item from inventory
    /// if item amount is 0 afterwards the item will be completly removed
    /// </summary>
    /// <param name="i">Item</param>
    /// <param name="amount">Amount to remove</param>
    public void RemoveItem(Item i, int amount)
    {
        if (items.ContainsKey(i) && HasEnoughOfItem(i, amount))
        {
            items[i] -= amount;
            if (items[i] == 0)
                items.Remove(i);
        }
    }

    /// <summary>
    /// Returns the amount of a specifc item
    /// </summary>
    /// <param name="i">Item</param>
    /// <returns>Quantity of item or -1 if not in inventory</returns>
    public int GetItemAmount(Item i)
    {
        if (items.ContainsKey(i))
            return items[i];

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





}
