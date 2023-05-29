using BeeState;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Bee : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, patrol, Size }  // 사이즈를 맨 마지막에 두면 열거형 크기를 알수있게끔 만들어둔다
    public float detectRange;
     public float attackRange;
     public float movespeed;
     public Transform[] PatroPoints;

    public Vector3 returnPosition;
    public int patrolIndex = 0;
    public Transform player;

    private State curState;
    private StateBase[] states;

    private void Awake()
    {
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState();
        states[(int)State.Return] = new ReturnState();
        states[(int)State.Attack] = new AttackState();
    }
    private void Start()
    {
        curState = State.Idle;
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
    private void Update()
    {
        states[(int)curState].Update();
    }
    public void ChangeState(State state)
    {
        curState = state;
    }
}

namespace BeeState
{
   
    public class IdleState : StateBase
    {
        private Bee bee;
        private float idleTime;
        public IdleState(Bee bee)
        {
            this.bee = bee;

        }
     
        public override void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                idleTime = 0;
                bee.patrolIndex = (bee.patrolIndex + 1) % bee.PatroPoints.Length;
                bee.ChangeState(Bee.State.patrol);
            }

            // 플레이어와 가까워지면 다가가기
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(Bee.State.Trace);
            }

        }
    }
    public class TraceState : StateBase
    {
        private Bee bee;
        public override void Update()
        {

        }
    }
    public class ReturnState : StateBase
    {
        private Bee bee;
        public override void Update()
        {
            
        }
    }
    public class AttackState : StateBase
    {
        private Bee bee;
        public override void Update()
        {
            
        }
    }
    public class PatrolState : StateBase
    {
        private Bee bee;
        public override void Update()
        {
           
        }
    }
}
