using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f; // 총알의 파괴력
    public float speed = 1000.0f; // 총알 발사 속도
    
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed); // Z축으로 날아가기
    }

    void Update()
    {
        
    }
}
