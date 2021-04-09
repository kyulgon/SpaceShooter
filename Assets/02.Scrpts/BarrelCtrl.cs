using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // 폭발효과 프리팹을 저장할 변수
    public Mesh[] meshes; // 찌그러진 드럼통의 메쉬를 저장할 배열
    public Texture[] textures; // 드럼통의 텍스처를 저장할 배열
    public float expRadius = 10.0f; // 폭발 반경
    public AudioClip expSfx; // 폭팔음 오디오 클립 저장할 변수

    private int hitCount; // 총알이 맞은 횟수 저장
    private Rigidbody rb; // Rigidbody 컴포넌트 저장할 변수
    private MeshFilter meshFilter; // 외형을 변경하기 위한 MeshFilter
    private MeshRenderer _renderer; // 이미지 바꿀때 사용
    private AudioSource _audio; // 오디오 저장

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _audio = GetComponent<AudioSource>();

        // 난수를 발생시켜 불규칙적인 텍스처를 적용
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
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
        IndirectDamage(transform.position); // 폭발력 생성

        int idx = Random.Range(0, meshes.Length); // meshs 개수만큼 랜덤 수 뽑아냄
        meshFilter.sharedMesh = meshes[idx]; // 찌그러진 메쉬를 적용
        GetComponent<MeshCollider>().sharedMesh = meshes[idx]; // 변경된 메쉬에 따라 수정

        _audio.PlayOneShot(expSfx, 1.0f); // 폭발음 실행
    }

    private void IndirectDamage(Vector3 pos) // 폭발력을 주변에 전달하는 함수
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 9); // 주변에 있는 드럼통을 모두 추출

        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>(); // 폭발 범위에 포함된 드럼통의 리지드바드 컴포넌트 추출
            _rb.mass = 1.0f; // 드럼통의 무게를 가볍게 함
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f); // 폭발력 전달 
        }
    }
}
