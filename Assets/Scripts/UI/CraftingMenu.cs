using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenu : MonoBehaviour
{

    VisualElement root;
    List<Recipe> recipes = new List<Recipe>();
    ListView listView;
    public VisualTreeAsset itemSlot;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        recipes.Clear();

        recipes.Add(new Recipe() { ResultItemName = "Sword" , Materials = RandomMaterials() , ResultAmount = 1});
        recipes.Add(new Recipe() { ResultItemName = "Spear", Materials = RandomMaterials(), ResultAmount = 1 });
        recipes.Add(new Recipe() { ResultItemName = "Dagger", Materials = RandomMaterials(), ResultAmount = 1 });
        InitializeRecipeListView();
        AddItems();



    }


    public void AddItems()
    {
        VisualElement itemContainer = root.Q<VisualElement>("middle");

        for (int i = itemContainer.Children().Count(); i > 0; i--)
        {
            itemContainer.RemoveAt(i - 1);
        }

        InventorySystem inv = GameManager.Instance.GetPlayerSystem().InventorySystem;

        foreach (KeyValuePair<Item, int> item in inv.GetAllItems())
        {
            AddItemToUi(item.Key, item.Value);
        }


    }

    public void AddItemToUi(Item i, int amount)
    {
        VisualElement itemContainer = root.Q<VisualElement>("middle");
        TemplateContainer tmpContainer = itemSlot.Instantiate();
        VisualElement itemSlotElement = tmpContainer.Q<VisualElement>("item-container");
        itemSlotElement.name = i.ItemName;
        itemSlotElement.style.backgroundImage = GetBackgroundImage(i.ItemName);
        tmpContainer.Q<Label>("item-amount").text = "" + amount;
        itemContainer.Add(itemSlotElement);


        //itemSlotElement.RegisterCallback<MouseDownEvent>((e) => { ClickHandler(itemSlotElement, e, i); });
    }

    private List<RecipeMaterial> RandomMaterials()
    {
        List<RecipeMaterial> recipeMaterials = new List<RecipeMaterial>();
        int random  = UnityEngine.Random.Range(0, 3);

        if(random >= 0)
        {
            recipeMaterials.Add(new RecipeMaterial() { Material = ItemManager.Instance.GetItemByName("scrap"), Amount = UnityEngine.Random.Range(1,10) });
        }
        if(random >= 1)
        {
            recipeMaterials.Add(new RecipeMaterial() { Material = ItemManager.Instance.GetItemByName("wood"), Amount = UnityEngine.Random.Range(1, 10) });
        }
        if(random >= 2)
        {
            recipeMaterials.Add(new RecipeMaterial() { Material = ItemManager.Instance.GetItemByName("stone"), Amount = UnityEngine.Random.Range(1, 10) });
        }


        return recipeMaterials;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            AddRecipes(new Recipe() { ResultItemName = "Dildo" });
    }

    public void AddRecipes(Recipe r)
    {
        recipes.Add(r);
        root.Q<ListView>("crafting-recipes").Refresh();
    }


    public void InitializeRecipeListView()
    {

        Func<VisualElement> makeItem = () => new Label();

        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = recipes[i].ResultItemName;

        const int itemHeight = 25;

        listView = new ListView(recipes, itemHeight, makeItem, bindItem);
        listView.name = "crafting-recipes";
        listView.selectionType = SelectionType.Single;

        //listView.onItemsChosen += recipe_choosen => ChooseRecipes((Recipe)(recipe_choosen.FirstOrDefault()));
        listView.onSelectionChange += recipe_choosen =>
        {
            Recipe choosen = (Recipe)(recipe_choosen.FirstOrDefault());
            ChooseRecipes(choosen);

            root.Q<Button>("craft-button").clicked += () => GameManager.Instance.GetPlayer().GetComponent<CraftingSystem>().CraftItem(choosen);

        };
        listView.style.flexGrow = 1.0f;

        root.Q<VisualElement>("recipes-mid").Add(listView);

    }


    void ChooseRecipes(Recipe r)
    {

        Debug.Log($"Recipe: {r.ResultItemName}");

        int counter = 0;
        foreach(RecipeMaterial rm in r.Materials)
        {
            VisualElement slot = root.Q<VisualElement>("material-" + counter);
            slot.style.backgroundImage = GetBackgroundImage(rm.Material.ItemName);
            slot.Q<Label>($"material-{counter}-amount").text = ""+rm.Amount;
            counter++;
        }
    }


    public Background GetBackgroundImage(string name)
    {
        return Background.FromSprite(Resources.Load<Sprite>($"items/{name}"));
    }

}

