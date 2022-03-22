using UnityEngine;

public class NpcBase : MonoBehaviour
{

    AttributeSystem aSystem;
    int entityId;
    float roomDepth;
    public Room r;

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
            GameManager.Instance.GetPlayerSystem().InventorySystem.ChangeCurrency(UnityEngine.Random.Range(1f, 10f) * Mathf.Max(r.depth, 1f));
            GameManager.Instance.GetPlayerSystem().InventorySystem.AddItem(new Item(0, "rock"), 1);
        }

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
