using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class EscMenu : MonoBehaviour
    {
        VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            InitElements();
        }

        void InitElements()
        {
            root.Q<Button>("resume").clicked += () =>
            {
                UiManager.Instance.ToggleMenu("esc-menu");
            };

            root.Q<Button>("exit").clicked += () =>
            {
                GameManager.Instance.ExitToMainMenu();
            };
        }
    }
}
