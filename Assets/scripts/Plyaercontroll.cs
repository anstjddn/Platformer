using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plyaercontroll : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float MovePower;
    [SerializeField] private float JumpPower;
    private Vector2 movedir;
    [SerializeField] private float Maxspeed;
    private Animator anim;
    private SpriteRenderer render;
    [SerializeField] private bool isGrounded;
    [SerializeField] LayerMask groundLayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Move();
    }
    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if(movedir.x<0 && rb.velocity.x>-Maxspeed)
            rb.AddForce(Vector2.right * movedir.x * MovePower, ForceMode2D.Force);
        else if (movedir.x > 0 && rb.velocity.x < Maxspeed)
        rb.AddForce(Vector2.right * movedir.x * MovePower, ForceMode2D.Force);
        
    }
    private void OnMove(InputValue value)
    {
        movedir = value.Get<Vector2>();
        anim.SetFloat("Movespeed", Mathf.Abs(movedir.x));
        if (movedir.x > 0) render.flipX = false;
        else if (movedir.x < 0) render.flipX = true;
    }

    private void OnJump(InputValue value)
    {
        Jump();
        
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        
    }
   /*private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        anim.SetBool("IsGrounded",true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        anim.SetBool("IsGrounded", false);
    }*/
   private void GroundCheck()
    {
        RaycastHit2D hit =Physics2D.Raycast(transform.position, Vector2.down,1.5f);
        if(hit.collider != null)
        {
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
            
        }
        else
        {
            isGrounded = false;
            anim.SetBool("IsGrounded", false);
        }
    }



}
