﻿public static class ItemFactory
{
    public static Item CreateItem(Item i)
    {
        if (i == null)
            return null;

        Item c = new Item(i.ItemId, i.ItemName, i.equipmentType);
        c.properties = i.properties;
        c.itemsTypes = i.itemsTypes;
        return c;
    }


}

