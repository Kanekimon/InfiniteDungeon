using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttributeSystem : MonoBehaviour
{
    public Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();

    public void AddAttribute(Attribute a)
    {
        attributes[a.Name] = a;
    }

    public float GetAttributeValue(string name)
    {
        return attributes[name].GetValue();
    }


    public void ChangeAttribute(string attribute, float value)
    {
        if(attributes.ContainsKey(attribute))
        {
            attributes[attribute].ChangeValue(value);
        }
    }
}

