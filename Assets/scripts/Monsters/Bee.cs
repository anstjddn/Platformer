using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Rendering;

public class Bee : MonoBehaviour
{
     public enum State { Idle, Trace, Return, Attack, patrol, Size}  // 사이즈를 맨 마지막에 두면 열거형 크기를 알수있게끔 만들어둔다
     public float detectRange;
     public float attackRange;
     public float movespeed;
     public Transform[] PatroPoints;

    public Vector3 returnPosition;
    public int patrolIndex = 0;
    public Transform player;

    private State curState;
    private StateBase<Bee>[] states;

    private void Awake()
    {
        states = new StateBase<Bee>[(int)State.Size];
    }
    private void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void ChangeState(StateBase<Bee> state)
    {

    }
}

namespace BeeState
{
   
    public class IdleState : StateBase<Bee>
    {
        private float idleTime;
        public IdleState(Bee owner) : base(owner)
        {
        }
        public override void Enter()
        {
            idleTime = 0;
        }

        public override void Exit()
        {
           
        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                idleTime = 0;
                owner.patrolIndex = (owner.patrolIndex + 1) % owner.PatroPoints.Length;
                //curState = State.patrol;
            }

            // 플레이어와 가까워지면 다가가기
            else if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.detectRange)
            {
                //curState = State.Trace;
            }
        }
    }
    public class TraceState : StateBase<Bee>
    {
        public TraceState(Bee owner) : base(owner)
        {
        }
        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Setup()
        {

        }

        public override void Update()
        {

        }
    }
}
