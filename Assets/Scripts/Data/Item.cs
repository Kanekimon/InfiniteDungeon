public class Item
{
    private int itemId;
    private string itemName;

    public int ItemId { get { return itemId; } }
    public string ItemName { get { return itemName; } }

    public Item(int id, string name)
    {
        this.itemId = id;
        this.itemName = name;
    }

}

