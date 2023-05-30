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
     public Transform[] patrolPoints;

    public Vector3 returnPosition;
    public int patrolIndex = 0;
    public Transform player;

    private StateBase[] states;
    private State curState;
   

    private void Awake()
    {
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.patrol] = new AttackState(this);
    }
    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
        
    }
    private void Update()
    {
        states[(int)curState].Update();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
 
    public void ChangeState(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
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

        public override void Enter()
        {
            Debug.Log("Idle Enter");
        }

        public override void Exit()
        {
            Debug.Log("Idle Exit");
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                idleTime = 0;
               
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
       public Bee bee;

        public TraceState(Bee bee)
        {
            this.bee = bee;

        }

        public override void Enter()
        {
            Debug.Log("Trace Enter");
        }

        public override void Exit()
        {
            Debug.Log("Trace Exit");
        }

        public override void Update()
        {
            // 플레이어 쫓아가기
            Vector2 dir = (bee.player.position - bee.transform.position).normalized; // 크기가 1인벡터로 만들기
                                                                                     // 가까이 있으면 천천히 쫗아오고 멀리떨어져있으면 빠르게 온다 벡터의 크기때문에 따라서 크기가 1인벡터로 만들자
            bee.transform.Translate(dir * bee.movespeed * Time.deltaTime);
            // 플레이어가 멀어졌을때 
            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.detectRange)
            {
                bee.ChangeState(Bee.State.Return);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.attackRange)
            {
                bee.ChangeState(Bee.State.Attack);
            }
        }
    }
    public class ReturnState : StateBase
    {
        private Bee bee;
        public ReturnState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            Debug.Log("Return Enter");
        }

        public override void Exit()
        {
            Debug.Log("Return Exit");
        }

        public override void Update()
        {
            // 원래자리로돌아가기
            Vector2 dir = (bee.returnPosition - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.movespeed * Time.deltaTime);

            // 원래자리 돌아갔으면 
            if (Vector2.Distance(bee.transform.position, bee.returnPosition) < 0.02f)
            {
                bee.ChangeState(Bee.State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(Bee.State.Trace);
            }

        }
    }
    public class AttackState : StateBase
    {
        private Bee bee;
        private float lastAttackTime;
        public AttackState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            Debug.Log("Attack Enter");
        }

        public override void Exit()
        {
            Debug.Log("Attack Exit");
        }

        public override void Update()
        {
            //공격하기 
            if (lastAttackTime > 3)
            {
                Debug.Log("공격");
                lastAttackTime += Time.deltaTime;
            }
            lastAttackTime = 0;

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.attackRange)
            {
                bee.ChangeState(Bee.State.Trace);
            }
        }
    }
    public class PatrolState : StateBase
    {
        private Bee bee;
        public PatrolState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Enter()
        {
            Debug.Log("Patrol Enter");
            bee.patrolIndex = (bee.patrolIndex + 1) % bee.patrolPoints.Length;
        }

        public override void Exit()
        {
            Debug.Log("Patrol Exit");
        }

        public override void Update()
        {
            Vector2 dir = (bee.patrolPoints[bee.patrolIndex].position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.movespeed * Time.deltaTime);
            if (Vector2.Distance(bee.transform.position, bee.patrolPoints[bee.patrolIndex].position) < 0.02f)
            {
                bee.ChangeState(Bee.State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(Bee.State.Trace);
            }
        }
    }
}
