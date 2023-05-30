using BeeState;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class Bee : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, patrol, Size }  // ����� �� �������� �θ� ������ ũ�⸦ �˼��ְԲ� �����д�
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

            // �÷��̾�� ��������� �ٰ�����
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
            // �÷��̾� �Ѿư���
            Vector2 dir = (bee.player.position - bee.transform.position).normalized; // ũ�Ⱑ 1�κ��ͷ� �����
                                                                                     // ������ ������ õõ�� �m�ƿ��� �ָ������������� ������ �´� ������ ũ�⶧���� ���� ũ�Ⱑ 1�κ��ͷ� ������
            bee.transform.Translate(dir * bee.movespeed * Time.deltaTime);
            // �÷��̾ �־������� 
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
            // �����ڸ��ε��ư���
            Vector2 dir = (bee.returnPosition - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.movespeed * Time.deltaTime);

            // �����ڸ� ���ư����� 
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
            //�����ϱ� 
            if (lastAttackTime > 3)
            {
                Debug.Log("����");
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
