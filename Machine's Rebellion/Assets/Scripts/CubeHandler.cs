using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//CubeHandler는 유니티 에디터에서 Cube를 움직이기 쉽게 하기위해 사용합니다
//CubeHandler는 게임을 빌드할 때는 포함하지 않습니다
//
[ExecuteInEditMode] //유니티 에디터에서 실행 되도록하는 Attribute
[SelectionBase] //Cube를 선택할 때 부모부터 선택되도록 하는 Attribute
[RequireComponent(typeof(Waypoint))]    //Waypoint Script가 있어야 CubeHandler가 실행되도록 하는 Attribute
public class CubeHandler : MonoBehaviour
{

    Waypoint waypoint;  //Cube의 Waypoint를 저장할 변수

    //Start보다 먼저 실행된다
    private void Awake()
    {
        //Cube의 Waypoint를 가져온다
        waypoint = GetComponent<Waypoint>();
    }

    // Update is called once per frame
    void Update()
    {
        SnapCube();
        UpdateLabel();
    }

    //Cube를 움직일 때 girdSize만큼 움직이도록 한다
    private void SnapCube()
    {
        int gridSize = waypoint.GetGridSize();
        transform.position = new Vector3(waypoint.GetGridPos().x * gridSize, 0f, waypoint.GetGridPos().y * gridSize);
    }

    //Cube의 위치를 Cube의 윗면에 Text로 표시한다
    private void UpdateLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();

        string labelText = (waypoint.GetGridPos().x) + "," + (waypoint.GetGridPos().y);

        textMesh.text = labelText;
        gameObject.name = labelText;
    }
}
