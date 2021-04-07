﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // 폭발효과 프리팹을 저장할 변수
    private int hitCount; // 총알이 맞은 횟수 저장
    private Rigidbody rb; // Rigidbody 컴포넌트 저장할 변수

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void OnCollisionEnter(Collision coll) // 충돌이 발생했을 때 한번 호출되는 콜백 함수
    {
        if(coll.collider.CompareTag("Bullet")) // 충동할 게임오브젝트의 태그를 비교
        {
            if(++hitCount ==3) // 총알의 충돌 횟수를 증가시키고 3발 이상 맞았는지 확인
            {
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel() // 폭발효과를 처리할 함수
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); // 폭발 효과 프리팹을 동적으로 생성
        Destroy(effect, 2);
        rb.mass = 1.0f; // 무게를 가볍게 바꿈
        rb.AddForce(Vector3.up * 1000.0f); // 위로 솟구치는 힘을 가함
    }
}