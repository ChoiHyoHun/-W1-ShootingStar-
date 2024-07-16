using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class MapSpwanManager : MonoBehaviour
{
    public GameObject[] initialMapPrefabs; // 초기 설정한 배열값
    private List<GameObject> useMapPrefabs; // 사용할 배열
    private Vector3 offset = new Vector3(0,-500,0);
    Vector3 startTargetPosition = new Vector3(0,0,0);
    Vector3 MapPosition;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        ResetGameObjects();
        GameObject drawnElement1 = DrawElement(); // List에서 맵 조각 하나 뽑음
        GameObject drawnElement2 = DrawElement();

        drawnElement1.transform.position = startTargetPosition;
        drawnElement2.transform.position = startTargetPosition + offset;
        MapPosition = offset;

        Debug.Log(MapPosition.y);        
        
    }

    // Update is called once per frame
    void Update()
    {   // 맵 위치와 플레이어 위치 비교 후 반 넘어가면 맵 조각 생성
        Vector3 playerPosition = player.transform.position;  
        Debug.Log($"Player Y Position: {playerPosition.y}");
        Debug.Log($"Map Y Position: {MapPosition.y}");     
        if(playerPosition.y < MapPosition.y){
            Debug.Log("IF IN");
            MapPosition += offset;
            GameObject drawnElement = DrawElement();
            drawnElement.transform.position = MapPosition;

        }

    }

    void ResetGameObjects(){
        //초기 배열 현재 리스트 복사, 랜덤 수 다 뽑혔을 때 초기화 하는 거
        useMapPrefabs = new List<GameObject>(initialMapPrefabs);
    }

    GameObject DrawElement(){
        if(useMapPrefabs.Count == 0){

            // 현재 리스트 비어있으면 초기 리스트로 초기화
            ResetGameObjects();
        }

        //랜덤하게 뽑고 리스트 제거
        int index = Random.Range(0, useMapPrefabs.Count);
        GameObject drawnElement = useMapPrefabs[index];
        useMapPrefabs.RemoveAt(index);
        Debug.Log(index);

        return drawnElement;
    }

    
}
