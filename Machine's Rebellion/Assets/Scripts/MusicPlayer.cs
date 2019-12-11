using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //Start이전에 실행된다
    void Awake()
    {
        //싱글톤 패턴
        //MusicPlayer가 1개 이상있을 경우 새로 생성된 MusicPlayer 파괴
        int numMusicPlayer = FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayer > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //Scene이 실행되는 동안 MusicPlayer 파괴 못하게 하기
            DontDestroyOnLoad(gameObject);
        }
    }
}
