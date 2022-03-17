using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class AttributeUpgradeMenu : MonoBehaviour
    {
        VisualElement root;
        private static string Name = "upgrade-menu";

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            InitElements();
            InitStats();
        }

        void InitElements()
        {
            root.Q<Button>("close-update").clicked += () => { UiManager.Instance.ToggleMenu(Name); };

            root.Q<Button>("str-upgr").clicked += () => { IncreaseAttribute("str"); };
            root.Q<Button>("dex-upgr").clicked += () => { IncreaseAttribute("dex"); };
            root.Q<Button>("con-upgr").clicked += () => { IncreaseAttribute("con"); };
            root.Q<Button>("wis-upgr").clicked += () => { IncreaseAttribute("wis"); };
            root.Q<Button>("int-upgr").clicked += () => { IncreaseAttribute("int"); };
            root.Q<Button>("cha-upgr").clicked += () => { IncreaseAttribute("cha"); };



        }

        public void InitStats()
        {
            PlayerSystem ps = GameManager.Instance.GetPlayerSystem();

            foreach (Attribute att in ps.AttributeSystem.attributes)
            {
                if (root.Q<VisualElement>(att.Name) != null)
                {
                    root.Q<Label>($"{att.Name}-value").text = $"{att.Value:F2}";
                }
            }
        }


        public void IncreaseAttribute(string attr)
        {
            GameManager.Instance.GetPlayerSystem().AttributeSystem.UpgradeAttribute(attr);
            InitStats();
        }


    }
}
