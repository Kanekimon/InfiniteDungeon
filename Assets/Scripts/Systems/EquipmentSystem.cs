using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    private Dictionary<EquipmentType, Item> equipment = new Dictionary<EquipmentType,Item>();
    private GameObject owner;
    private InventorySystem inv;

    private void Awake()
    {
        this.owner = this.gameObject;
        inv = owner.GetComponent<InventorySystem>();

        equipment[EquipmentType.helment] = null;
        equipment[EquipmentType.shoulder] = null;
        equipment[EquipmentType.chest] = null;
        equipment[EquipmentType.legs] = null;
        equipment[EquipmentType.boots] = null;
        equipment[EquipmentType.weapon] = null;
        equipment[EquipmentType.shield] = null;

    }

    public void EquipItem(Item i)
    {
        if (!i.itemsTypes.Contains(ItemType.equipment))
            return;

        if (!equipment.ContainsKey(i.equipmentType))
        {
            equipment[i.equipmentType] = i;
            inv.RemoveItem(i, 1);
        }
        else
        {
            if (equipment[i.equipmentType] != null)
                inv.AddItem(equipment[i.equipmentType],1);
            equipment[i.equipmentType] = i;
            inv.RemoveItem(i, 1);
        }
        Debug.Log("Equip");

        UiManager.Instance.UpdateEquipment(equipment);
    }

    public void Unequip(EquipmentType et)
    {
        if(equipment.ContainsKey(et) && equipment[et] != null)
        {
            inv.AddItem(equipment[et],1);
            equipment[et] = null;
        }
        Debug.Log("Unequip");


        UiManager.Instance.UpdateEquipment(equipment);

    }

}

