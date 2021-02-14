using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyPrefabs;    //생성할 적 로봇을 저장할 변수
    [SerializeField] AudioClip spawnedSFX;  //적 로봇을 생성할 때 실행할 효과음을 저장하는 변수

    UIManager uiManager;    //UIManger Script를 저장할 변수
    SceneLoader sceneLoader; //SceneLoader Script를 저장할 변수
    float spawnDelay = 2f;  //적 로봇을 생성할 때 줄 지연시간

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();  //게임 내에 UIManger가 있으면 가져온다
        sceneLoader = FindObjectOfType<SceneLoader>(); //게임 내에 SceneLoader가 있으면 가져온다
        StartCoroutine(SpawnEnemies()); //SpawnEnemies 코루틴을 실행한다
    }

    //코루틴은 단일쓰레드를 사용하는 유니티에서 멀티테스킹을 구현하기 위해 사용한다.
    //코루틴은 yield를 만나면 현재 함수를 정지하고 빠져나간뒤 일정시간이 지난뒤 돌아온다
    ///uiManager의 Barrier가 0이 될때 까지 적 로봇을 계속 생성한다
    IEnumerator SpawnEnemies()
    {
        while (uiManager.GetBarrier() > 0)    //Barrier가 0이 될 때 까지 반복
        {
            GetComponent<AudioSource>().PlayOneShot(spawnedSFX);
            Instantiate(enemyPrefabs, transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnDelay * Random.Range(0.01f, 2f));
        }

        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        foreach(EnemyMovement enemy in enemies)
        {
            Destroy(enemy);
        }

        sceneLoader.LoadNextScene();
    }

}
