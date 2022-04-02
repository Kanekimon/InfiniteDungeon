using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Item loot;

    public Vector2 randDir;
    public bool explode = false;
    public float duration = 1f;
    public bool move = true;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        randDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Debug.Log(name);
        this.loot = ItemManager.Instance.GetItemByName(name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!explode && move)
        {
            this.GetComponent<Rigidbody2D>().velocity = randDir.normalized * Random.Range(2f, 5f);
            if (duration <= 0f)
            {
                explode = true;
                move = false;
            }
        }
        else if (explode && move)
        {
            if (player != null)
            {
                if (Vector2.Distance(player.transform.position, this.transform.position) < 1f)
                {
                    player.gameObject.GetComponent<InventorySystem>().AddItem(loot, 1);
                    Destroy(this.gameObject);
                }
                else
                {
                    this.GetComponent<Rigidbody2D>().velocity = (player.transform.position - this.transform.position).normalized * Random.Range(2f, 5f);
                }
            }

        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        duration -= Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.gameObject.GetComponent<InventorySystem>().AddItem(loot, 1);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            move = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            move = false;
            player = null;
        }
    }

}
