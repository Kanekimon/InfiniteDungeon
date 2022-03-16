using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttributeSystem : MonoBehaviour
{
    public List<Attribute> attributes = new List<Attribute>();

    public void AddAttribute(Attribute a)
    {
        if (!attributes.Contains(a))
            attributes.Add(a);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && this.gameObject.CompareTag("Player"))
        {
            UpgradeAttribute("dex");
        }
    }


    public Attribute GetAttribute(string name)
    {
        return attributes.Where(a => a.Name.Equals(name)).First();
    }

    public float GetAttributeValue(string name)
    {
        return attributes.Where(a => a.Name.Equals(name)).First()?.Value ?? -1;
    }

    public float ChangeAttributeValue(string name, float amount)
    {
        return GetAttribute(name).ChangeValue(amount);
    }

    public void ChangeHealth(float amount)
    {
        Attribute hp = GetAttribute("hp");
        hp.ChangeValue(amount);
        if (hp.ChangableValue <= 0)
            Die();
    }


    public void UpgradeAttribute(string name)
    {

        Attribute a = GetAttribute(name);

        if (a != null)
        {
            float price = GetUpgradePrice(name);
            if (GameManager.Instance.GetPlayer().GetComponent<InventorySystem>().GetCurrency() > price)
            {
                AttributeModifier am = a.GetAttributeModifier(ModifierType.upgrade);
                GameManager.Instance.GetPlayer().GetComponent<InventorySystem>().ChangeCurrency(-price);
                if (am == null)
                {
                    am = new AttributeModifier(ModifierType.upgrade, 1);
                    AddModifierToAttribute(name, am);
                }
                else
                    am.Value *= 1.5f;
            }
        }


    }

    public void AddModifierToAttribute(string name, AttributeModifier am)
    {
        GetAttribute(name).AddModifier(am);
    }

    public float GetUpgradePrice(string name)
    {
        Attribute a = GetAttribute(name);
        if (a != null)
        {
            AttributeModifier am = a.GetAttributeModifier(ModifierType.upgrade);

            if (am == null)
                return 10f;
            else
                return am.Value * 10f;
        }
        return -1f;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}

