using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;
    private Transform tr;

    public float moveSpeed = 10.0f; // 이동속도 변수
    public float rotSpeed = 80.0f; // 회전속도 변수

    public PlayerAnim playerAnim; // 인스펙터 뷰에 표시할 애니메이션 클래스 변수
    public Animation anim; // Animation 컴포넌트를 저장하기 위한 변수


    void Start()
    {
        // 사용할 컴포넌트 할당
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // 클립을 idle로 설정해 실행
        anim.clip = playerAnim.idle; 
        anim.Play();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");


        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); // 전후좌우 이동방향 벡터 계산

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self); // 이동

        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r); // Vetor3.up 축을 기준으로 회전

        // 키보드 입력값을 기준으로 동작할 애니메이션 수행
        if(v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f); // 전진 애니메이션
        }
        else if ( v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f); // 후진 애니메이션
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f); // 오른쪽 애니메이션
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f); // 왼쪽 애니메이션
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f); // 정지 시 Idle 애니메이션
        }
    }
}
