using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed;
    Rigidbody2D rb;
    Vector2 movement;
    float angle;

    PlayerTargeting pTarget;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pTarget = GetComponent<PlayerTargeting>();
        SetRotation();
    }

    // Update is called once per frame
    void Update()
    {

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        SetRotation();


        //this.transform.rotation = Quaternion.LookRotation(RelMouseCoords());

    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * this.GetComponent<AttributeSystem>().GetAttributeValue("dex"));
    }

    private void SetRotation()
    {
        Vector2 mR = pTarget.RelMouseCoords();
        float angle = Mathf.Atan2(mR.y, mR.x) * Mathf.Rad2Deg - 90;
        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = quaternion;
        transform.GetChild(0).rotation = Quaternion.identity;
    }


    public Vector3 GetDirection()
    {
        Vector3 norm = pTarget.RelMouseCoords();
        norm.z = 0;
        return norm.normalized;
    }
}
