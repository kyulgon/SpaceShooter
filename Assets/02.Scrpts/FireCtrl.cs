using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; // 총알 프리팹
    public Transform firePos; // 총알 발사 좌표
    public ParticleSystem cartridge; // 탄피 추출 파티클
    private ParticleSystem muzzleFlash; // 총구화염 파티클

    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(); // Children에 생성
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // 마우스 클릭시
        {
            Fire();
        }
    }

    void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation); // Bullet 프리팹을 동적으로 생성
        cartridge.Play(); // 탄피 파티클 실행
        muzzleFlash.Play(); // 총구화염 파티클실행
    }
}
