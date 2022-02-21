using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold
    }

    float holdTime;

    public InteractionType interactionType;

    public abstract string GetDescription();
    public abstract void Interact(GameObject interactinWith);
    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;
    public float GetHoldTime() => holdTime;
}

