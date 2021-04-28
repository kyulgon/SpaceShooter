using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //총알 발사와 재장전 오디오 클립을 저장할 구조체
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType // 무기타입
    {
        RIFLE=0,
        SHOTGUN
    }

    public WeaponType currWeapon = WeaponType.RIFLE; // 플레이어가 현재 들고 있는 무기를 저장할 변수

    public GameObject bullet; // 총알 프리팹
    public Transform firePos; // 총알 발사 좌표
    public ParticleSystem cartridge; // 탄피 추출 파티클
    public PlayerSfx playerSfx; // 오디오 클립을 저장할 변수

    private ParticleSystem muzzleFlash; // 총구화염 파티클
    private AudioSource _audio; // 오디오를 저장한 변수

    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(); // Children에 생성
        _audio = GetComponent<AudioSource>();
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
        FireSfx(); // 사운드 실행

    }

    private void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon]; // 현재 들고 있는 무기의 오디오 클립을 가져옴
        _audio.PlayOneShot(_sfx, 1.0f); // 사운드 실행
    }
}
