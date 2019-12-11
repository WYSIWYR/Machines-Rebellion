using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text barrierText;  //barrier를 표시할 Text를 저장하는 변수
    [SerializeField] Text scoreText;    //score를 표시할 Text를 저장하는 변수

    int barrier = 10;   //아군 기지의 barrier
    int damagePerAttack = 1;    //적 로봇이 공격하면 감소할 barrier의 양
    int score;  //적 로봇을 파괴하면 증가하는 score를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        InitText();
    }

    //Text를 기본 값으로 초기화 한다
    private void InitText()
    {
        barrierText.text = barrier.ToString();
        scoreText.text = score.ToString();
    }

    //아군 기지가 공격 받으면 damagePerAttack만큼 barrier를 감소시키고 text를 업데이트 한다
    private void BaseAttacked()
    {
        barrier -= damagePerAttack;
        barrierText.text = barrier.ToString();
    }

    //적 로봇을 파괴하면 destroyScroe를 score만큼 증가시키고 text를 업데이트 한다
    private void DestroyScore(int destroyScore)
    {
        score += destroyScore;
        scoreText.text = score.ToString();
    }

    //score를 반환해 준다
    public int GetScore()
    {
        return score;
    }

    //barrier를 반환해 준다
    public int GetBarrier()
    {
        return barrier;
    }
}
