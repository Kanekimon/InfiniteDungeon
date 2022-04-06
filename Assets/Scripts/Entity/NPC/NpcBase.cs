using UnityEngine;

public class NpcBase : MonoBehaviour
{

    AttributeSystem aSystem;
    int entityId;
    float roomDepth;
    public Room r;
    public LootTable lootTable;


    public static event HandleKill OnEnemyKilled;
    public  delegate void HandleKill(int depth);


    private void Start()
    {
        aSystem = GetComponent<AttributeSystem>();

        if (aSystem.attributes.Count > 0)
        {
            aSystem.attributes.Add(new Attribute("hp", aSystem.GetAttributeValue("con") * Mathf.Max(aSystem.GetAttributeValue("dex"), aSystem.GetAttributeValue("str")) * Mathf.Max(roomDepth, 1)));
        }

    }

    private void OnDestroy()
    {
        if (aSystem.GetAttribute("hp").ChangableValue <= 0)
        {
            OnEnemyKilled?.Invoke((int)roomDepth);

            foreach (Loot loot in lootTable.GetLoot())
            {
                if (loot.Name.Equals("gold"))
                {
                    for (int i = 0; i < Random.Range(loot.Min, loot.Max + 1); i++)
                    {
                        SpawnLoot("gold");
                    }
                    //GameManager.Instance.GetPlayerSystem().InventorySystem.ChangeCurrency(UnityEngine.Random.Range(loot.Min, loot.Max + 1) * Mathf.Max(r.depth, 1f));
                }
                else
                {
                    int randoAmount = Random.Range(loot.Min, loot.Max + 1);
                    for (int i = 0; i < randoAmount; i++)
                    {
                        if (UnityEngine.Random.Range(0, 1f) < loot.BaseProbability)
                            SpawnLoot(loot.Name);
                    }



                    // GameManager.Instance.GetPlayerSystem().InventorySystem.AddItem(ItemManager.Instance.GetItemByName(loot.Name), UnityEngine.Random.Range(loot.Min, loot.Max + 1));
                }
            }

        }
    }

    void SpawnLoot(string name)
    {
        GameObject drop = Instantiate(Resources.Load<GameObject>("items/drop"));
        drop.transform.position = this.transform.position;
        drop.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"items/{name}");
        drop.name = name;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.gameObject.GetComponent<AttributeSystem>().ChangeHealth(-1);
    }


    public void SetDifficulty(Room r)
    {
        this.r = r;
        roomDepth = r.depth;
    }

    public int GetId()
    {
        return entityId;
    }


}
