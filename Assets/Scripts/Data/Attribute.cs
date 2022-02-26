using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Attribute
{
    [SerializeField]
    public float baseValue;
    [SerializeField]
    private string name;
    [SerializeField]
    private float value;
    [SerializeField]
    private float changableValue;

    private List<AttributeModifier> modifiers = new List<AttributeModifier>();
    

    public float BaseValue { get { return baseValue; } set { baseValue = value; } }
    public string Name { get { return name; } set { name = value; } }

    public float ChangableValue { get { return changableValue; } }

    public float Value
    {
        get
        {
            value = BaseValue;
            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (AttributeModifier attributeModifier in modifiers)
                {
                    if (attributeModifier.ModifierType == ModifierType.flat || attributeModifier.ModifierType == ModifierType.upgrade)
                        value += attributeModifier.Value;
                    else if (attributeModifier.ModifierType == ModifierType.mult)
                        value *= attributeModifier.Value;
                }
            }


            return value;
        }
    }




    public AttributeModifier GetAttributeModifier(ModifierType mod)
    {
        return modifiers.Where(a => a.ModifierType == mod).FirstOrDefault();
    }


    public void AddModifier(AttributeModifier am)
    {
        modifiers.Add(am);
    }

    public float ChangeValue(float amount)
    {
        changableValue += amount;
        return changableValue;
    }

    public Attribute(string n, float bValue)
    {
        name = n;
        baseValue = bValue;
        changableValue = Value;
    }
}

