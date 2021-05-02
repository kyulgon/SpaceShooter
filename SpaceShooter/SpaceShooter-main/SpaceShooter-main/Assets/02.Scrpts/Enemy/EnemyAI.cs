using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State // 적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.PATROL; // 기본 상태는 패트롤

    private Transform playerTr; // 주인공의 위치를 저장할 변수
    private Transform enemyTr; // 적 캐릭터의 위치를 저장할 변수
    private Animator animator; // Animator 컴포넌트를 저장할 변수

    public float attackDist = 5.0f; // 공격 사정거리
    public float traceDist = 10.0f; // 추적 사정거리

    public bool isDie = false; // 사망 여부를 판단하 변수

    private WaitForSeconds ws; // 코루틴에서 사용할 지연시간 변수
    private MoveAgent moveAgent; // 이동을 제어하는 MoveAgent 클래스를 저장할 변수
    private EnemyFire enemyFire; // 총알 발사를 제어하는 EnemyFire 클래스를 저장할 변수

    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hasSpeed = Animator.StringToHash("Speed");

    private void Awake()
    {
        //  주인공 게임오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null) // 주인공의 Transform 컴포넌트 추출
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>(); // 적 캐릭터의 Transform 컴포넌트 추출
        animator = GetComponent<Animator>(); // Animator 컴포넌트 추출
        moveAgent = GetComponent<MoveAgent>(); // 이동을 제어하는 MoveAgent 클래스를 추출
        enemyFire = GetComponent<EnemyFire>(); // 총알 발사를 제어하는 EnemyFire 클래스를 추출

        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState() // 적 캐릭터의 상태를 검사하는 코루틴
    {
        while(!isDie) // 적 캐릭터가 사망하지 전까지 도는 무한루프
        {
            if (state == State.DIE) // 상태가 사망이면 종류
                yield break;

            // 주인공과 적 캐릭터 간의 거리를 계산
            float dist = (playerTr.position - enemyTr.position).sqrMagnitude;

            if(dist <= attackDist * attackDist) // 공격 사정거리 이내인 경우
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist * traceDist) // 추적 사정거리 이내인 경우
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            yield return ws; // 0.3초동안 대기하는 동안 제어권을 양보
        }
    }

    IEnumerator Action() // 상태에 따라 적 캐릭터의 행동을 처리하는 코루틴 함수
    {
        while(!isDie) // 적 캐릭터가 사망할 때까지 무한루프
        {
            yield return ws;

            switch (state) // 상태에 따라 분기 처리
            {
                case State.PATROL:
                    enemyFire.isFire = false; // 총알 발사 정지
                    moveAgent.patrolling = true; // 순찰모드 활성화
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    enemyFire.isFire = false; // 총알 발사 정지
                    // 주인공의 위치를 넘겨 추적모드로 변경
                    moveAgent.traceTarget = playerTr.position; 
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // 순찰 및 추적을 정지
                    moveAgent.Stop(); 
                    animator.SetBool(hashMove, false);
                    // 총알 발사 시작
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    break;

                case State.DIE:
                    moveAgent.Stop(); // 순찰 및 추적을 정지
                    break;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Speed 파라미터에 이동 속도를 전달
        animator.SetFloat(hasSpeed, moveAgent.speed);
    }
}
