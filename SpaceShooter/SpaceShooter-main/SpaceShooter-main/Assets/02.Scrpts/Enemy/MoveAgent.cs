using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints; // 순찰 지점들을 저장하기 위한 List 타입 변수
    public int nextIdx; // 다음 순찰 지점의 배열의 Index

    private NavMeshAgent agent; // NavMeshAgent 컴포넌트를 저장할 변수

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;


    private bool _patrolling; // 순찰 여부를 판단하는 변수
    public bool patrolling // patrolling 프로퍼티 정의(getter, setter)
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }            
    }


    private Vector3 _traceTarget; // 추적 대상의 위치를 저장하는 변수
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set 
        { 
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent.autoBraking = false; // 목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.speed = patrolSpeed;

        var group = GameObject.Find("WayPointGroup"); // 하이러키 뷰의 WayPointGroup 게임오브젝트 찾기

        if(group != null)
        {
            // WayPointGroup 하위에 있는 모든 Transform 컴포넌트를 추출한 후
            // List 타입의 wayPoints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);

            wayPoints.RemoveAt(0); // 배열의 첫 번째 항목 삭제
        }

        // MoveWayPoint();
        this.patrolling = true;
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale) // 최단거리 경로 계산이 끝나지 않았으면 다음을 수행하지 않음
            return;

        // 다음 목적지를 wayPoints 배열에서 추출한 위치로 다음 목적지를 지정
        agent.destination = wayPoints[nextIdx].position;
        // 내비게이션 기능을 활성화해서 이동을 시작함
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos) // 주인공을 추적할 때 이동시키는 함수
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop() // 순찰 및 추적을 정지시키는 함수
    {
        agent.isStopped = true;

        agent.velocity = Vector3.zero; // 바로 정지하기 위해 속도를 0으로 설정
        _patrolling = false;
    }

    void Update()
    {
        if (!_patrolling) return; // 순찰 모드가 아닐 경우 이후 로직을 수행하지 않음

        // NavMeshAgent가 이동하고 있고 목적지에 도착했는지 여부를 계산
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance < 0.5f)
        {
            nextIdx = ++nextIdx % wayPoints.Count; // 다음 목적지의 배열 첨자를 계산
            MoveWayPoint(); // 다음 목적지로 이동 명령을 수행
        }

    }

   

    
}
