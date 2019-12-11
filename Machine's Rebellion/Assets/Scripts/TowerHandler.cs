using System;
using UnityEngine;

public class TowerHandler : MonoBehaviour
{
    [SerializeField] Transform objectToPan; //회전할 물체의 Trasform(위치, 회전, 스케일 정보를 가지고 있다)을 저장한다
    [SerializeField] ParticleSystem bulletParticle; //적 로봇을 공격할 때 실행할 bulletParticle을 저장할 변수
    [SerializeField] GameObject towers; //tower들을 관리할 부모를 저장할 변수

    Transform targetEnemy;  //공격할 적 로봇을 저장하는 변수

    float attackRange = 30f;    //사격 범위를 저장하는 변수

    public Waypoint foundation; //생성된 Tower의 Waypoint를 저장하는 변수

    // Update is called once per frame
    void Update()
    {
        SetTarget();

        //범위 내에 적 로봇이 존재할 때만 공격한다
        if (targetEnemy)
        {
            objectToPan.LookAt(targetEnemy);
            AttackEnemy();
        }

        else
        {
            Shoot(false);
        }
    }

    //현재 게임에 생성되어 있는 적 로봇을 모두 가져와 가장 가까운 적을 Target으로 설정한다
    private void SetTarget()
    {
        EnemyDamage[] targetEnemies = FindObjectsOfType<EnemyDamage>();

        if(targetEnemies.Length == 0) { return; }

        Transform closestEnemy = targetEnemies[0].transform;

        //재 게임에 생성되어 있는 모든 적 로봇을 비교해 가장 가까운 적을 정한다
        foreach (EnemyDamage enemy in targetEnemies)
        {
            closestEnemy = GetClosest(closestEnemy, enemy.transform);
        }

        targetEnemy = closestEnemy;
    }

    //적 로봇 A, B와 Tower의 거리를 측정해 더 가까운 적 로봇을 반환해 준다
    private Transform GetClosest(Transform EnemyA, Transform EnemyB)
    {
        float distanceToA = Vector3.Distance(transform.position, EnemyA.position);
        float distanceToB = Vector3.Distance(transform.position, EnemyB.position);

        if (distanceToA < distanceToB)
        {
            return EnemyA;
        }

        else
        {
            return EnemyB;
        }
    }

    //Target과 Tower사이의 거리가 사격 범위 내에 있으면 사격한다
    private void AttackEnemy()
    {
        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);

        if (distanceToEnemy <= attackRange)
        {
            Shoot(true);
        }
        else
        {
            Shoot(false);
        }
    }

    //공격할 때 자연스러움을 주기 위해 Particle의 Emission을 끄고 킨다
    //Emission은 Particle이 실행 도중에 false가 되면 현재 Particle만 실행하고 더 실행하지 않는다
    private void Shoot(bool isActive)
    {
        ParticleSystem.EmissionModule emissionModule = bulletParticle.emission;
        emissionModule.enabled = isActive;
    }
}
