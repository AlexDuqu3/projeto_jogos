using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;
public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
public class AttackArea : MonoBehaviour
{
    Vector2 attackOffset;
    GameObject player;
    private int damage = 100;
    Collider2D attackCollider;
    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        attackCollider = GetComponent<Collider2D>();
        attackCollider.enabled = false;
        attackOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
             collision.GetComponent<Enemy>().dealDamage(damage);
        }
    }
 
    public void Attack(Direction facingDirection)
    {
        Vector3 offset = Vector3.zero;
        attackCollider.enabled = true;
        switch (facingDirection)
        {
            case Direction.Right:
                offset = Vector3.zero;
                break;

            case Direction.Left:
                 offset = new Vector3(-attackCollider.bounds.size.x * 0.5f * player.transform.localScale.x, 0f, 0f);
                break;

            //case Direction.Up:
            //    offset = new Vector3(0f, attackCollider.bounds.size.y * 0.5f * player.transform.localScale.y, 0f);
            //    break;

            //case Direction.Down:
            //    offset = new Vector3(0f, -attackCollider.bounds.size.y * 0.5f * player.transform.localScale.y, 0f);
            //    break;
        }

        // Apply the offset to the collider position
        attackCollider.transform.position = player.transform.position + offset;

    }

    public void StopAttack()
    {
        attackCollider.enabled = false;
    }

    
}
