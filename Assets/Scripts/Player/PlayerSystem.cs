using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSystem : MonoBehaviour
{

    public AttributeSystem AttributeSystem;
    public InventorySystem InventorySystem;
    private int level;
    private int currentExp;
    private int expNeeded;
    public int UpgradePoints;


    private void OnEnable()
    {
        NpcBase.OnEnemyKilled += KillEnemy;
    }

    private void OnDisable()
    {
        NpcBase.OnEnemyKilled -= KillEnemy;

    }

    private void Start()
    {
        currentExp = 0;
        level = 1;
        expNeeded = CalculateExpNeeded();
        UiManager.Instance.SetExp(expNeeded, currentExp, level);
    }



    public void InitPlayer()
    {
        AttributeSystem = this.GetComponent<AttributeSystem>();
        InventorySystem = this.GetComponent<InventorySystem>();
        AddAttributes();

        InventorySystem.AddItem(ItemManager.Instance.GetItem(1), 1);
        InventorySystem.AddItem(ItemManager.Instance.GetItem(2), 1);
        InventorySystem.AddItem(ItemManager.Instance.GetItem(3), 1);

        InventorySystem.AddItem(ItemManager.Instance.GetItem(4), 100);
        InventorySystem.AddItem(ItemManager.Instance.GetItem(5), 100);
        InventorySystem.AddItem(ItemManager.Instance.GetItem(6), 100);
        this.GetComponent<Animator>().enabled = true;

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

        UiManager.Instance.SetHp(health.Value, health.ChangableValue);
    }


    private void OnDestroy()
    {
        GameManager.Instance.ExitToMainMenu();
    }

    private float GetRandom()
    {
        return UnityEngine.Random.Range(10f, 15f);
    }


    public void KillEnemy(int depth)
    {
        currentExp += depth + 50;

        if(currentExp >= expNeeded)
        {
            LevelUp();
        }

        UiManager.Instance.SetExp(expNeeded, currentExp, level);
    }

    public void LevelUp()
    {
        level += 1;
        int o = Mathf.Abs(expNeeded - currentExp);
        currentExp = 0 + o;
        expNeeded = CalculateExpNeeded();
        UpgradePoints += 3;
    }

    public int CalculateExpNeeded()
    {
        return (100 * level) + (level - 1 * (int)((float)expNeeded * 0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger &&  collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with : " + collision.gameObject.name);
            Vector2 knockVec = (collision.transform.position - this.transform.position).normalized;

            collision.gameObject.GetComponent<BaseLogic>().knockedBack = true;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockVec * 100f;
            //collision.gameObject.GetComponent<BaseLogic>().knockedBack = false;
            this.transform.GetChild(0).GetComponent<CameraShake>().Shake(0.2f, 0.15f);
            collision.gameObject.GetComponent<AttributeSystem>().ChangeHealth(-AttributeSystem.GetAttributeValue("str"));
        }
    }


}


