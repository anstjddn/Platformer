using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private float movespeed;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform groundcheckpoint;
    [SerializeField] private LayerMask groundMask;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        if (!IsGroundExist())
        {
            Turn();
        }

    }
   
    private void Move()
    {
        rb.velocity = new Vector2(transform.right.x*-movespeed, rb.velocity.y);
    }
    private bool IsGroundExist()
    {
        Debug.DrawRay(groundcheckpoint.position, Vector2.down, Color.red);
        return Physics2D.Raycast(groundcheckpoint.position, Vector2.down, 1f, groundMask);
    }
    private void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }
}
