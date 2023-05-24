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
        RaycastHit2D hit =Physics2D.Raycast(transform.position, Vector2.down,1.5f,groundLayer);     // 시작위치, 레이저로 확인할 위치, 체크길이
        // 결과는 레이캐스트 2d로 나온다. 따라서 hit만들어준다.
        // 레이저를 일자로 쏘는방식말고 여러캐스트 방식이있다.
        // 레이캐스트 올은 범위에 있는애들 전부 훍는다
        // 레이캐스트 레이저만 범위에 먼저 땋는애만 
        if(hit.collider != null)
        {
        
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);
        }
        else
        {
            isGrounded = false;
            anim.SetBool("IsGrounded", false);
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green);
        }
    }



}
