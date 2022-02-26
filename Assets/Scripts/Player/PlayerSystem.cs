using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerSystem : MonoBehaviour
{

    public AttributeSystem AttributeSystem;
    public InventorySystem InventorySystem;


    private void Start()
    {

    }



    public void InitPlayer()
    {
        AttributeSystem = this.GetComponent<AttributeSystem>();
        InventorySystem = this.GetComponent<InventorySystem>();
        AddAttributes();
    }

    public void LoadAttributes(List<Attribute> attr)
    {
        foreach (Attribute a in attr)
        {
            AttributeSystem.AddAttribute(a);
        }
    }


    void AddAttributes()
    {
        Attribute strength = new Attribute("str", 10f);
        Attribute dexterity = new Attribute("dex", 10f);
        Attribute constitution = new Attribute("con", 10f);
        Attribute wisdom = new Attribute("wis", 10f);
        Attribute intelligence = new Attribute("int", 10f);
        Attribute charisma = new Attribute("cha", 10f);
        Attribute health = new Attribute("hp", constitution.Value * Mathf.Max(strength.Value, dexterity.Value) * 0.5f);
        Attribute speed = new Attribute("sp", dexterity.Value);


        AttributeSystem.AddAttribute(strength);
        AttributeSystem.AddAttribute(dexterity);
        AttributeSystem.AddAttribute(constitution);
        AttributeSystem.AddAttribute(wisdom);
        AttributeSystem.AddAttribute(intelligence);
        AttributeSystem.AddAttribute(charisma);
        AttributeSystem.AddAttribute(health);
        AttributeSystem.AddAttribute(speed);
    }


    private void OnDestroy()
    {
        GameManager.Instance.ExitToMainMenu();
    }

}

