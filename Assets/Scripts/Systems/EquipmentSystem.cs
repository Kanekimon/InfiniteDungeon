using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    private Dictionary<EquipmentType, Item> equipment = new Dictionary<EquipmentType, Item>();
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
                Unequip(i.equipmentType);
            equipment[i.equipmentType] = i;
            inv.RemoveItem(i, 1);
        }
        if (i.equipmentType == EquipmentType.weapon)
        {
            //GameObject weap = Instantiate(Resources.Load<GameObject>($"items/{i.ItemName}/{i.ItemName}_prefab"));
            //weap.name = "weapon";
            //weap.transform.SetParent(transform, false);
        }

        Debug.Log("Equip");

        UiManager.Instance.UpdateEquipment(equipment);
    }

    public void Unequip(EquipmentType et)
    {
        if (equipment.ContainsKey(et) && equipment[et] != null)
        {
            if (et == EquipmentType.weapon)
            {
                Transform deleteThis = null;

                foreach (Transform c in this.transform)
                {
                    if (c.name.Contains("weapon"))
                        deleteThis = c;

                }
                if (deleteThis != null)
                    Destroy(deleteThis.gameObject);
            }


            inv.AddItem(equipment[et], 1);
            equipment[et] = null;
        }



        UiManager.Instance.UpdateEquipment(equipment);

    }

}

