using Assets.Scripts.Enum;
using UnityEngine;

internal class Door : Interactable
{

    public Direction dir;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override string GetDescription()
    {
        return $"Press [E] to proceed {dir.ToString()}";
    }

    public override void Interact(GameObject interactinWith)
    {
        RoomManager.Instance.MoveToNextRoom(dir);
    }

    public void SetDir(Direction d)
    {
        dir = d;
    }
}

