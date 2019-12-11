using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] TowerHandler towerPrefab;  //생성할 Tower를 저장할 변수
    [SerializeField] int towerLimit = 5;    //최대생성 가능한 Tower 수를 저장하는 변수

    Queue<TowerHandler> towers = new Queue<TowerHandler>(); //생성된 Tower를 저장할 Queue

    //입력받은 Waypoint에 생성된 Tower의 수가 towerLimit보다 작으면 Tower를 생성한다
    //Tower의 수가 towerLimit보다 크면 가장 오래된 Tower를 Waypoint로 옮긴다
    public void CreateTower(Waypoint foundation)
    {
        
        int numTowers = towers.Count;

        if (numTowers < towerLimit)
        {
            CreateNewTower(foundation);
        }
        else
        {
            MoveOldestTower(foundation);
        }
    }

    //입력받은 Waypoint에 새Tower를 생성한다
    private void CreateNewTower(Waypoint foundation)
    {
        
        TowerHandler newTower = Instantiate(towerPrefab, foundation.transform.position, Quaternion.identity, transform);
        newTower.foundation = foundation;
        foundation.isPlaceable = false; //현재 Waypoint에 Tower를 더 생성하지 못하도록 한다
        
        towers.Enqueue(newTower);
    }

    //입력받은 Waypoint로 가장 오래된 타워를 옮긴다
    private void MoveOldestTower(Waypoint newFoundation)
    {
        TowerHandler oldTower = towers.Dequeue();

        oldTower.foundation.isPlaceable = true; //이전 Waypoint에 Tower를 생성할 수 있도록 한다
        newFoundation.isPlaceable = false; //현재 Waypoint에 Tower를 더 생성하지 못하도록 한다

        oldTower.foundation = newFoundation;
        oldTower.transform.position = newFoundation.transform.position;

        towers.Enqueue(oldTower);
    }


}
