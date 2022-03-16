using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUiManager : MonoBehaviour
{
    private Button start;
    private Button load;
    private VisualElement root;

    [SerializeField]
    public VisualElement menu;

    public VisualElement saveStates;
    [SerializeField]
    public VisualTreeAsset saveState;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
    }

    private void Start()
    {
        start = root.Q<Button>("NewGame");
        load = root.Q<Button>("LoadGame");
        menu = root.Q<VisualElement>("start-menu");
        saveStates = root.Q<VisualElement>("save-states");

        if (menu == null)
            Debug.Log("Menu null");
        if (saveState == null)
            Debug.Log("Save Null");


        start.clickable.clicked += () =>
        {
            MenuManager.Instance.StartGame();
        };

        load.clickable.clicked += () =>
        {
            Debug.Log("Load");
            menu.style.display = DisplayStyle.None;
            saveStates.style.display = DisplayStyle.Flex;

            LoadSaves();
        };

        root.Q<Button>("back-menu").clickable.clicked += () =>
        {
            menu.style.display = DisplayStyle.Flex;
            saveStates.style.display = DisplayStyle.None;

        };
    }

    private void LoadSaves()
    {
        List<int> saves = SaveGameLoader.GetSaveGames();

        for (int i = 0; i < saves.Count; i++)
        {
            TemplateContainer tc = saveState.Instantiate();
            Button b = tc.Q<Button>("save-state");
            b.clickable.clicked += () =>
                {
                    MenuManager.Instance.LoadGame(i);
                };
            b.text = $"Save Game #{i}"; 
            
            root.Q<VisualElement>("save-games").Add(tc);
        }
    }

}

