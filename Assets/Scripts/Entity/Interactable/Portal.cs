using System;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class Portal : Interactable
    {
        public override string GetDescription()
        {
            throw new NotImplementedException();
        }

        public override void Interact(GameObject interactinWith)
        {
            RoomManager.Instance.LeaveHub();
        }
    }
}
