using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] ParticleSystem selfDestruct;   //SelfDestrut시 사용할 효과를 저장할 변수
    [SerializeField] AudioClip selfDestructSFX; //SelfDestrut시 사용할 효과음을 저장할 변수

    UIManager uiManager;    //UIManger Script를 저장할 변수

    float moveDelay = 0.45f; //적 로봇이 움직일 때 생기는 지연시간

    // Start is called before the first frame update
    void Start()
    {
        PathFinder pathFinder = FindObjectOfType<PathFinder>(); //PathFinder Script를 저장할 변수
        List<Waypoint> path = pathFinder.GetPath(); //PathFinder에 있는 GetPaht함수를 이용해 적 로봇이 움직일 경로를 가져온다
        uiManager = FindObjectOfType<UIManager>();  //게임 내에 UIManger가 있으면 가져온다
        StartCoroutine(ThroughThePath(path));   //ThroughThePaht 코루틴을 실행한다
    }

    //코루틴은 단일쓰레드를 사용하는 유니티에서 멀티테스킹을 구현하기 위해 사용한다.
    //코루틴은 yield를 만나면 현재 함수를 정지하고 빠져나간뒤 일정시간이 지난뒤 돌아온다
    ///waypointList에 있는 waypoint에 따라 적 로봇을 움직이게 한다
    IEnumerator ThroughThePath(List<Waypoint> waypointList)
    {
        foreach(Waypoint waypoint in waypointList)
        {
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(moveDelay * Random.Range(0.03f, 3.5f));
        }
        SelfDestruct();
    }

    //적 로봇이 아군 기지에 도달하면 자폭을 하도록 한다
    //deathEffect와  selfDestructSFX를 실행시키고 uiManager에 있는 BaseAttacked를 호출한다
    //이후 스스로를 게임에서 제거한다
    private void SelfDestruct()
    {
        //현재 적 로봇의 위치에 회전시키지 않고 적 로봇을 부모로 가지는 selfDestruct 효과를 deathEffect에 담아 실행한다
        ParticleSystem deathEffect = Instantiate(selfDestruct, transform.position, Quaternion.identity, gameObject.transform.parent.transform);
        deathEffect.Play();
        //deathEffect의 duration(실행 주기)가 끝난후 deathEffect를 게임에서 제거한다.
        Destroy(deathEffect.gameObject, deathEffect.main.duration);
        AudioSource.PlayClipAtPoint(selfDestructSFX, Camera.main.transform.position);
        uiManager.SendMessage("BaseAttacked");

        Destroy(gameObject);
    }
}
