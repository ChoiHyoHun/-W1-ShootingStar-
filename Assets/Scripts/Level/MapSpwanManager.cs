using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class MapSpwanManager : MonoBehaviour
{
    public GameObject[] initialMapPrefabs; // 초기 설정한 배열값
    private List<GameObject> useMapPrefabs; // 사용할 배열
    private List<GameObject> useMapPrefabs1;
    private Vector3 offset = new Vector3(0, -50f, 0); // 얼마만큼 맵 조각이 떨어져 있나. 맵 크기와 상관있음
    Vector3 startTargetPosition = new Vector3(0, 0, 0); // 첫 맵 조각이 나올 위치
    Vector3 MapPosition; // 랜덤하게 생성될 맵 조각 생성 위치
    // 플레이어

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePrefab();
        ResetGameObjects();
        GameObject drawnElement1 = DrawElement(); // List에서 맵 조각 하나 뽑음
        GameObject drawnElement2 = DrawElement(); // List 맵 조각 하나 뽑음2

        drawnElement1.transform.position = startTargetPosition; // 맵 조각1 생성 위치
        drawnElement2.transform.position = startTargetPosition + offset; // 맵 조각2 생성 위치
        MapPosition = offset; // 랜덤하게 생성될 맵 조각 생성 위치

        // Debug.Log(MapPosition.y);      
        // Debug.Log(useMapPrefabs1.Count);  

    }

    // Update is called once per frame
    void Update()
    {   // 맵 위치와 플레이어 위치 비교 후 반 넘어가면 맵 조각 생성
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        // Debug.Log($"Player Y Position: {playerPosition.y}");
        // Debug.Log($"Map Y Position: {MapPosition.y}");     
        if (playerPosition.y < MapPosition.y)
        { // 맵 조각 생성 조건
            // Debug.Log("IF IN");
            MapPosition += offset; // 맵 조각 생성 위치 더 밑으로 내리기
            GameObject drawnElement = DrawElement(); // 맵 조각 List 중에서 하나 가져오기
            drawnElement.transform.position = MapPosition; // 맵 조각 생성 위치로 보내기
        }

    }

    void InstantiatePrefab()
    { // 프리팹을 오브젝트로 생성
        useMapPrefabs = new List<GameObject>();

        foreach (GameObject prefab in initialMapPrefabs)
        {
            GameObject instantiatedObject = Instantiate(prefab);
            instantiatedObject.SetActive(false);
            useMapPrefabs.Add(instantiatedObject);
        }
    }

    void ResetGameObjects()
    { // 생성된 오브젝트를 넣은 List 초기화
        //초기 배열 현재 리스트 복사, 랜덤 수 다 뽑혔을 때 초기화 하는 거
        useMapPrefabs1 = new List<GameObject>(useMapPrefabs);

    }

    GameObject DrawElement()
    {
        if (useMapPrefabs1.Count == 0)
        { // List에 몇 개 남았는지 보기

            // 현재 리스트 비어있으면 초기 리스트로 초기화
            ResetGameObjects();
        }

        //랜덤하게 뽑고 리스트 제거
        int index = Random.Range(0, useMapPrefabs1.Count); // 숫자 뽑기
        GameObject drawnElement = useMapPrefabs1[index]; // 해당 숫자에 Element 가져오기
        drawnElement.SetActive(true);
        useMapPrefabs1.RemoveAt(index); // 뽑힌 Element 제거하기
        // Debug.Log(index);
        return drawnElement; // Element 반환
    }


}
