﻿using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

public class MeleeAttack : Attack
{
    Vector3 orgPos;
    public GameObject attackPoint;

    bool attack = false;

    private void Start()
    {
        orgPos = transform.localPosition;
        attackPoint = this.transform.Find("AttackPoint").gameObject;
    }

    private void Update()
    {

    }


    public override void AttackAction(Vector2 targetCoords)
    {
        Direction = (targetCoords - (Vector2)this.transform.position).normalized;

        this.transform.position = this.transform.parent.position + (new Vector3(Direction.x, Direction.y) * AttackRange);
        if (attackPoint != null)
        {
            GameObject owner = this.transform.parent.GetComponent<AttackSystem>().gameObject;
            float damage = this.BaseDamage + owner.GetComponent<AttributeSystem>().GetAttributeValue("str");
            RaycastHit2D hit = Physics2D.BoxCast(attackPoint.transform.position, new Vector2(this.transform.lossyScale.x, this.transform.lossyScale.y), 0f, Direction, this.transform.lossyScale.y, LayerMask.GetMask(owner.GetComponent<AttackSystem>().Targets.ToArray()));
            DrawBoxCast(new Vector2(this.transform.lossyScale.x, this.transform.lossyScale.y), 0f, attackPoint.transform.position, Direction, this.transform.lossyScale.y, hit);
            if (hit.collider != null)
            {
                GameObject hitG = hit.collider.gameObject;

                if (!hitG.CompareTag("Wall"))
                {
                    Debug.Log($"Hit with: {hitG.tag}");
                    AttributeSystem att = null;
                    hitG.TryGetComponent(out att);
                    if (att != null)
                    {
                        Attribute health = att.GetAttribute("hp");
                        att.ChangeHealth(-damage);
                        if (hitG.CompareTag("Player"))
                            UiManager.Instance.SetHp(health.BaseValue, health.ChangableValue);
                    }
                }
            }

        }
        attack = true;




        StartCoroutine(Retract());
    }

    void DrawBoxCast(Vector2 size, float angle, Vector2 origen, Vector2 direction, float distance, RaycastHit2D hit)
    {
        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.red;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
    }

    IEnumerator Retract()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 parentPos = this.transform.parent.position;

        this.transform.position = orgPos + parentPos;
    }







    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject hit = collision.gameObject;

        GameObject owner = this.transform.parent.GetComponent<AttackSystem>().gameObject;

        float damage = this.BaseDamage + owner.GetComponent<AttributeSystem>().GetAttributeValue("str");


        if (hit.tag != "Wall" && owner != null && owner.GetComponent<AttackSystem>().Targets.Contains(hit.tag))
        {
            Debug.Log($"Hit with: {hit.tag}");
            AttributeSystem att = null;
            hit.TryGetComponent(out att);
            if (att != null)
            {
                Attribute health = att.GetAttribute("hp");
                att.ChangeHealth(-damage);
                if (hit.CompareTag("Player"))
                    UiManager.Instance.SetHp(health.BaseValue, health.ChangableValue);
            }
        }
        //ProjectilePoolManager.Instance.AddToPool(this.gameObject);

    }
}
