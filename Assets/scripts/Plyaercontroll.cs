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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("IsGrounded", true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("IsGrounded", false);
    }

}
