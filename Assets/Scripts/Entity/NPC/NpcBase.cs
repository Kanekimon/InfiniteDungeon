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
        }

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
