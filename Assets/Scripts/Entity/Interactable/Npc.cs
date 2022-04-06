using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class Npc : Interactable
{
    public override string GetDescription()
    {
        return "Press [E] to interact";
    }

    public override void Interact(GameObject interactinWith)
    {
        UiManager.Instance.ToggleMenu("shop-window");
    }
}

