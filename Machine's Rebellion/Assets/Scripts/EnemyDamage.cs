using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int barrier = 10;  //적 로봇이 가지는 barrier
    [SerializeField] ParticleSystem hitParticle;    //적 로봇이 공격 당할시 사용할 효과를 저장할 변수
    [SerializeField] ParticleSystem deathParticle;  //적 로봇이 파괴될시 사용할 효과를 저장할 변수
    [SerializeField] AudioClip hitSFX;  //적 로봇이 공격 당할시 사용할 효과음을 저장할 변수
    [SerializeField] AudioClip deathSFX;    //적 로봇이 파괴될시 사용할 효과음을 저장할 변수

    AudioSource myAudioSource;  //AudioSource를 저장할 변수
    UIManager uiManager;    //UIManger Script를 저장할 변수
    int destroyScore = 50;   //적 로봇을 파괴할 때 올라가는 점수

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();  //게임 내에 UIManger가 있으면 가져온다
        myAudioSource = GetComponent<AudioSource>();    //적 로봇이 가지고 있는 AudioSource를 가져온다
    }

    //Particle과 충돌할 때 HitProcess와 IsDestroy 함수를 실행한다
    private void OnParticleCollision(GameObject other)
    {
        HitProcess();
        IsDestroy();
    }

    //barrier를 1 감소시키고 hitParticle과 hitSFX를 실행시킨다
    private void HitProcess()
    {
        barrier--;
        hitParticle.Play();
        myAudioSource.PlayOneShot(hitSFX);
    }

    //barrier가 0보다 작아지면 적 로봇을 파괴한다
    //파괴되면 deathEffect와 deathSFX를 실행하고 uiManger가 가지고 있는 DestroyScore를 호출하면서 destroyScore를 넘겨준다
    //이후 스스로를 게임에서 제거한다
    private void IsDestroy()
    {
        if (barrier <= 0)
        {
            //현재 적 로봇의 위치에 회전시키지 않고 적 로봇을 부모로 가지는 deathParticle 효과를 deathEffect에 담아 실행한다
            ParticleSystem deathEffect =Instantiate(deathParticle, transform.position, Quaternion.identity, gameObject.transform.parent.transform);
            deathEffect.Play();
            //deathEffect의 duration(실행 주기)가 끝난후 deathEffect를 게임에서 제거한다.
            Destroy(deathEffect.gameObject, deathEffect.main.duration);
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            uiManager.SendMessage("DestroyScore", destroyScore);

            Destroy(gameObject);
        }
    }

}
