using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{


    public void CraftItem(Recipe r)
    {
        InventorySystem inv = this.GetComponent<InventorySystem>();

        if (inv == null)
            return;

        bool hasMats = true;

        foreach(RecipeMaterial rm in r.Materials)
        {
            hasMats &= inv.HasEnoughOfItem(rm.Material, rm.Amount);
        }

        if (hasMats)
            Debug.Log("Has enough mats");
        else
        {
            Debug.Log("insufficient mats");
            return;
        }

        foreach(RecipeMaterial rm in r.Materials)
        {
            inv.RemoveItem(rm.Material, rm.Amount);
        }

        Debug.Log(r.ResultItemName);
        Item crafted = ItemManager.Instance.GetItemByName(r.ResultItemName);
        inv.AddItem(crafted, r.ResultAmount);


    }


}

