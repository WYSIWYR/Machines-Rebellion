using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint, endWaypoint;   //적 로봇이 움직일 시작 위치와 끝 위치를 저장할 Waypoint
    Waypoint searchCenter;  //Breadth First Search 알고리즘에서 중앙 Waypoint를 저장할 변수

    List<Waypoint> path = new List<Waypoint>(); //적 로봇이 움직일 경로(Waypoint)를 저장할 List
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>(); //Key를 gridPos로, Value를 Waypoint로 저장하는 Dictionary
    Queue<Waypoint> queue = new Queue<Waypoint>();  //Breadth First Search 알고리즘에서 Waypoint를 저장할 Queue

    //searchCenter를 기준으로 상우하좌를 탐색할 때 사용할 Vector2Int 배열
    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
        
    };

    bool isRunning = true;  //Breadth First Search 알고리즘이 끝났는지 확인하는 bool

    //설정된 path가 없을 경우 path를 계산해 반환한다
    public List<Waypoint> GetPath()
    {
        if (path.Count == 0)
        {
            CalculatePath();
        }

        return path;
    }

    private void CalculatePath()
    {
        LoadBlocks();
        BreadthFirstSearch();
        CreatePath();
    }

    //게임에 존재하는 모든 Cube를 가져와 List에 Waypoint를 저장한다
    private void LoadBlocks()
    {
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach(Waypoint waypoint in waypoints)
        {
            Vector2Int gridPos = waypoint.GetGridPos();
            
            if (!grid.ContainsKey(gridPos))
            {
                grid.Add(gridPos, waypoint);
            }
            
        }
    }

    //Queue를 이용해 시작 위치에서 부터 끝 위치까 Breath First Search 알고리즘을 실행한다
    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);
        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            IfCenterIsEnd();
            ExplorNeighbors();
        }
    }

    //searchCenter가 endWaypoint에 도달 했으면 알고리즘을 종료한다
    private void IfCenterIsEnd( )
    {
        if (searchCenter == endWaypoint)
        {
            isRunning = false;
        }
    }

    //현재 위치를 기준으로 상우하좌에 있는 Cube를 가져와 탐색한다
    private void ExplorNeighbors( )
    {
        if(!isRunning) { return; }

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = searchCenter.GetGridPos() + direction;
            if(grid.ContainsKey(neighborCoordinates))
            {
                SearchNewNeighbor(neighborCoordinates);
            }
        }
    }

    //가져온 위치에 있는 WayPoint가 탐색되지 않았거나 queue에 없으면 Waypoint를 queue에 넣고 
    //searchCenter를 Waypoint가 어느 Waypoint에서 탐색 되었는지 알게 exploredFrom에 넣어준다
    private void SearchNewNeighbor(Vector2Int neighborCoordinates)
    {
        Waypoint neighbor = grid[neighborCoordinates];
        if(!(neighbor.isExplored || queue.Contains(neighbor)))
        {
            queue.Enqueue(neighbor);
            neighbor.exploredFrom = searchCenter;
        }
    }

    //exploredFrom을 이용해 끝 위치에서 부터 역방향으로 path에 넣고 List의 Reverse를 통해 반전시킨다
    private void CreatePath()
    {
        SetAsPath(endWaypoint);

        Waypoint previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
        {
            SetAsPath(previous);
            previous = previous.exploredFrom;
        }

        SetAsPath(startWaypoint);
        path.Reverse();
    }

    //path에 Waypoint를 추가하고 적 로봇이 다닐 path에 Tower를 설치하지 못하도록 한다
    private void SetAsPath(Waypoint waypoint)
    {
        path.Add(waypoint);
        waypoint.isPlaceable = false;
    }
}
