using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float horizontalMvm;
    [SerializeField] float verticalMvm;
    private Animator animator;
    private Rigidbody2D playerRB;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool onGround = true;
    [SerializeField] private bool onLadder = false;
    [SerializeField] private bool falling = false;
    [SerializeField] private bool facingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //InvokeRepeating("printVelocity", 1, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalMvm = Input.GetAxis("Horizontal");
        verticalMvm = 0;
        if (onLadder)
        {
            verticalMvm = Input.GetAxis("Vertical");
            if (verticalMvm != 0)
                animator.SetBool("moving", true);
        }
        Vector2 move = new Vector2(horizontalMvm, verticalMvm);
        transform.Translate(move * speed * Time.deltaTime);
        
        
        if(horizontalMvm!=0 || (onLadder && verticalMvm != 0))
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        if (horizontalMvm < 0)
        {
            facingRight = false;
        }
        if (horizontalMvm > 0)
        {
            facingRight = true;
        }
        spriteRenderer.flipX = !facingRight;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            animator.SetTrigger("jumping_trig");
            onGround = false;
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (playerRB.velocity.y < 0 && !falling)
        {
            falling = true;
            animator.SetTrigger("falling_trig");
        }
        if (onGround)
            falling = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            animator.SetBool("onGround", true);
            onGround = true;
            falling = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = false;
            animator.SetBool("onGround", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            playerRB.gravityScale = 0;
            playerRB.velocity = Vector2.zero;
            onLadder = true;
            animator.SetBool("onLadder", true);
            falling = false;
        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            playerRB.gravityScale = 1;
            onLadder = false;
            animator.SetBool("onLadder", false);
        }
    }
}
