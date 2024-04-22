using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.MiniGames.MiniGames;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaceMapGenerator : MapGenerator
{
    public RaceRoadItem[] roadList;
    public Dictionary<int, List<GameObject>> roads=new Dictionary<int, List<GameObject>>();
    public List<RaceRoadItem> roadItems = new();
    public List<KeyValuePair<int, int>> roadState=new List<KeyValuePair<int, int>>();
    public GameObject startObject;
    public GameObject endObject;

    public int roadLength = 20; // Yol uzunluğu

    private int currentDirection=0;
    
    private Vector3 lastEnd = new Vector3(0, 0, 0);
    private int lastTurn=-1 ;
    

    public override MiniGameMapGenerationVo SetMap()
    {
        SeperateRoads();
        GenerateMap();
        return GetMapInfo();
    }

    private void SeperateRoads()
    {
        for (int i = 0; i < roadList.Length; i++)
        {
            if (roads.ContainsKey(roadList[i].fromDirection))
            {
                roads[roadList[i].fromDirection].Add(roadList[i].gameObject);
            }
            else
            {
                roads[roadList[i].fromDirection] = new List<GameObject>();
                roads[roadList[i].fromDirection].Add(roadList[i].gameObject);
            }
        }

        roads[3] = new List<GameObject>(){startObject};
        roads[4] = new List<GameObject>(){endObject};

    }

    void GenerateMap()
    {
        CreateRoad(3);
        while (roadItems.Count<20)
        {
            int straightCount = Random.Range(0, 3);
            
            for (int j = 0; j < straightCount; j++)
            {
                CreateRoad(0);
            }

            lastTurn *= -1;
            CreateRoad(lastTurn);
            
        }
        CreateRoad(4);

    }
    

    private void CreateRoad(int direction)
    {
        int roadIndex = Random.Range(0, roads[direction].Count);
        GameObject selectedRoad = roads[direction][roadIndex];
        GameObject newRoad = Instantiate(selectedRoad,new Vector3(0,2000,0) ,Quaternion.Euler(0,90*currentDirection,0),transform);
        RaceRoadItem roadItem = newRoad.GetComponent<RaceRoadItem>();
        Vector3 diff = roadItem.startPos.position - lastEnd;
        newRoad.GetComponent<Transform>().localPosition -= diff;
        roadItems.Add(roadItem);
        lastEnd = roadItem.endPos.position;
        currentDirection += roadItem.direction;
        roadState.Add(new KeyValuePair<int, int>(direction,roadIndex));
    }
    
    private MiniGameMapGenerationVo GetMapInfo()
    {
        MiniGameMapGenerationVo vo = new MiniGameMapGenerationVo();
        vo.mapItems = roadState;
        vo.positions = roadItems.ConvertAll(road => new Vector3Vo(road.transform.position));
        vo.rotations = roadItems.ConvertAll(road => new QuaternionVo(road.transform.rotation));
        return vo;
    }
}
