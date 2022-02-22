using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerTargeting : MonoBehaviour
    {

        public LayerMask mask;
        public float interActionRange;
        public TMPro.TextMeshProUGUI interactionText;


        private void Start()
        {
            this.interactionText = GameObject.Find("InteractionText").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RelMouseCoords(), interActionRange, mask);
            if (hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                bool successfulHit = false;
                if (interactable != null)
                {
                    HandleInteraction(interactable);
                    interactionText.transform.position = hit.point;
                    interactionText.text = interactable.GetDescription();
                    successfulHit = true;
                }

                if (!successfulHit)
                    interactionText.text = "";
            }

            if (Input.GetMouseButton(0))
            {
                this.GetComponent<Attack>().Shot(RelMouseCoords());
            }
            

        }



        void HandleInteraction(Interactable interactable)
        {
            KeyCode key = KeyCode.E;
            switch (interactable.interactionType)
            {
                case Interactable.InteractionType.Click:
                    // interaction type is click and we clicked the button -> interact
                    if (Input.GetKeyDown(key))
                    {
                        interactable.Interact(this.gameObject);
                    }
                    break;
                case Interactable.InteractionType.Hold:
                    if (Input.GetKey(key))
                    {
                        // we are holding the key, increase the timer until we reach 1f
                        interactable.IncreaseHoldTime();
                        if (interactable.GetHoldTime() > 1f)
                        {
                            interactable.Interact(this.gameObject);
                            interactable.ResetHoldTime();
                        }
                    }
                    else
                    {
                        interactable.ResetHoldTime();
                    }
                    break;
                // helpful error for us in the future
                default:
                    throw new System.Exception("Unsupported type of interactable.");
            }
        }


        public Vector2 RelMouseCoords()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        }


        private void OnDrawGizmos()
        {
            if (!GameManager.Instance.GamePaused)
            {
                Vector2 mouse = Vector2.ClampMagnitude(RelMouseCoords(), interActionRange);

                //mouse.z = 0;
                Gizmos.DrawRay(this.transform.position, mouse);
            }
        }
    }
}
