using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FixedJoystick joystick;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private float moveSpeed = 10;
    Vector2 moviment;
    private float xMax, yMin;
    private Vector3 worldStartPosition;
    public Animator animator;
    private AttackArea attackArea;
    private bool attacking = false;
    private float attackTime = 0.5f;
    private float timer = 0f;
    private Direction attackDirection;

    // Start is called before the first frame update
    void Start()
    {
        SetLimits();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        attackArea = transform.GetChild(0).gameObject.GetComponent<AttackArea>();

   
    }
    // Update is called once per frame
    void Update()
    {
        //joystick.DeadZone = 0.5f;
        moviment.x = joystick.Horizontal;
        moviment.y = joystick.Vertical;
        if (moviment.x > 0f)
        {
            // Moving right
            attackDirection= Direction.Right;
            sp.flipX = false; // No flip
        }
        else if (moviment.x < 0f)
        {
            // Moving left
            attackDirection = Direction.Left;
            sp.flipX = true; // Flip the sprite horizontally
        }
        else if(moviment.y > 0f)
        {
            // Moving up
            attackDirection = Direction.Up;
            sp.flipX = false; // No flip
        }
        else if(moviment.y < 0f)
        {
            // Moving down
            attackDirection = Direction.Down;
            sp.flipX = false; // No flip
        }
        animator.SetFloat("Horizontal", moviment.x);
        animator.SetFloat("Vertical", moviment.y);
        animator.SetFloat("Speed", moviment.sqrMagnitude);
        animator.SetBool("attacking", attacking);
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
       
    }
    private void FixedUpdate()
    {
        movePlayer();
        if (Input.GetKeyDown(KeyCode.Space))
        {
             Attack(attackDirection);
        }
        if(attacking)
        {
            timer += Time.deltaTime;
            if (timer >= attackTime)
            {
                attacking = false;
                timer = 0f;
                attackArea.StopAttack();
            }
        }   
    }
    void movePlayer()
    {

        Vector2 movement = moviment.normalized;
        rb.velocity = movement * moveSpeed;
        Vector3 worldStartPosition = LevelManager.Instance.WorldMapPositionInitial;
        // Restrict player movement within the map
        float clampedX = Mathf.Clamp(rb.position.x, worldStartPosition.x, xMax);
        float clampedY = Mathf.Clamp(rb.position.y, yMin, worldStartPosition.y);
        rb.position = new Vector2(clampedX, clampedY);

        // Prevent rotation
        rb.rotation = 0f;
    }

    public void SetLimits()
    {
        Vector3 maxTile = LevelManager.Instance.MaxTile;

        float playerWidth = sp.bounds.size.x / 2f;
        float playerHeight = sp.bounds.size.y / 2f;
        xMax = maxTile.x - playerWidth;
        yMin = maxTile.y + playerHeight;
    }

    private void Attack(Direction direction)
    {
        attacking = true;
        attackArea.Attack(direction);
        //attackArea.StopAttack();

    }

    public void AttackButton()
    {
        Attack(attackDirection);
    }
   

}
