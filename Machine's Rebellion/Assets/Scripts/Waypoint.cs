using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    Vector2Int gridPos; //Cube를 X, Y(Vector3에서는 Z)를 Int로 저장할 Vector2

    const int gridSize = 10;    //한번에 움직이는 정도를 저장하는 상수

    public bool isPlaceable = true; //Tower를 생성할 수 있는지 지정하는 bool
    public bool isExplored = false; //이미 탐색했는지 저장하는 bool
    public Waypoint exploredFrom;   //지금 Waypoint를 탐색하기 전에 있던 Waypoint

    //gridSize를 반환해주는 함수
    public int GetGridSize()
    {
        return gridSize;
    }

    //Cube의 위치(Vector3, float)를 Int로 반올림해서 Vector2Int로 반환한다
    public Vector2Int GetGridPos()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x / gridSize),
        Mathf.RoundToInt(transform.position.z / gridSize));
    }

    //Mouse가 Cube위에 올라오면 실행하는 함수
    private void OnMouseOver()
    {
        //마우스 좌클릭을 하면 지금 Waypoint가 Tower를 생성할 수 있으면 게임 속의 TowerFactory를 가져와 CreatTower함수를 실행한다
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaceable)
            {
                FindObjectOfType<TowerFactory>().CreateTower(this);
            }
        }
    }

}
