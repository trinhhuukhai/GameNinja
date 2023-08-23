using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;


    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;


    // Update is called once per frame
    void FixedUpdate()
    {

        if (isDeath)
        {
            return;
        }

        isGrounded = CheckGrounded();

        //-1:left - 0 - 1:right
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
     
        if (isGrounded)
        {

            if (isJumping)
            {
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }


            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f && !isJumping)
            {
                ChangeAnim("run");
              
            }
            //attack

            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            //throw

            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //Moving

        if (Mathf.Abs(horizontal) > 0.1f)
        {
         
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);

            //horizontal > 0 -> tra ve 0, nguoc lai -> 180
            transform.rotation = Quaternion.Euler(new Vector3(0,horizontal > 0 ? 0 : 180,0));

        }

        //idle
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }

    //goi sau khi object duoc khoi tao +> reset thong so, dua ve dau tien
    public override void OnInit()
    {
        base.OnInit();
        isDeath = false;
        isAttack = false;

        transform.position = savePoint;

        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

     
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        return hit.collider != null;
    }

    private void Attack() {
      
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);

    }

    private void Throw() {
       
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
       
    }

    private void Jump() {
        ChangeAnim("jump");
        isJumping = true;
       
        rb.AddForce(jumpForce * Vector2.up);
    }



    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack() {
    
        attackArea.SetActive(true);
    }

    private void DeActiveAttack() {
        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
            
        }

        if(collision.tag == "DeathZone")
        {
            isDeath = true;
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
    }

 
}
