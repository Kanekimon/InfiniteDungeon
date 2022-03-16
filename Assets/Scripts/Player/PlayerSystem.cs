using System.Collections.Generic;
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
        Attribute strength = new Attribute("str", GetRandom());
        Attribute dexterity = new Attribute("dex", GetRandom());
        Attribute constitution = new Attribute("con", GetRandom());
        Attribute wisdom = new Attribute("wis", GetRandom());
        Attribute intelligence = new Attribute("int", GetRandom());
        Attribute charisma = new Attribute("cha", GetRandom());
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

    private float GetRandom()
    {
        return UnityEngine.Random.Range(1f, 20f);
    }

}


