using Assets.Scripts.Player;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed;
    Rigidbody2D rb;
    Vector2 movement;
    float angle;
    Animator animator;

    PlayerTargeting pTarget;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pTarget = GetComponent<PlayerTargeting>();
        animator = GetComponent<Animator>();
        //SetRotation();
    }

    // Update is called once per frame
    void Update()
    {

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        GetComponent<SpriteRenderer>().flipX = movement.x > 0;

        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Horizontal", movement.x);

        animator.SetFloat("Speed", movement.magnitude);
        //SetRotation();


        //this.transform.rotation = Quaternion.LookRotation(RelMouseCoords());

    }


    private void FixedUpdate()
    {
        Vector2 motionVector = rb.position + movement * Time.fixedDeltaTime * this.GetComponent<AttributeSystem>().GetAttributeValue("dex");
        rb.MovePosition(motionVector);
      

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
