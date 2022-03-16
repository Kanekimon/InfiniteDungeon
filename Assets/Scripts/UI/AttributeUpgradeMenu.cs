using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public void InitStats()
        {
            PlayerSystem ps = GameManager.Instance.GetPlayerSystem();

            foreach(Attribute att in ps.AttributeSystem.attributes)
            {
                if(root.Q<VisualElement>(att.Name) != null)
                {
                    root.Q<Label>($"{att.Name}-value").text = ""+att.Value;
                }
            }


        }


    }
}
