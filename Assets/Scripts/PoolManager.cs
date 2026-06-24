using System.Collections.Generic; // 큐(Queue)를 사용하기 위해 필요합니다.
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 파이썬의 클래스 원본처럼 쓰일 프리팹을 넣을 공간입니다.
    public GameObject platformPrefab;
    public int poolSize = 20;

    // 파이썬의 리스트(대기열)와 같은 역할인 Queue를 만듭니다.
    private Queue<GameObject> platformPool = new Queue<GameObject>();

    void Start()
    {
        // 파이썬의 for i in range(20): 와 동일한 반복문입니다.
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab); // 프리팹을 복제(Instantiate)
            obj.SetActive(false); // 화면에서 일단 숨김 처리
            platformPool.Enqueue(obj); // 대기열(Queue)에 집어넣기 (파이썬의 append)
        }
    }

    // 다른 스크립트에서 "발판 하나 줘!" 할 때 실행되는 함수
    public GameObject GetPoolItem()
    {
        if (platformPool.Count > 0)
        {
            GameObject obj = platformPool.Dequeue(); // 대기열에서 맨 앞의 하나를 뺌 (파이썬의 pop(0))
            obj.SetActive(true); // 화면에 보이게 켬
            return obj;
        }
        else
        {
            // 대기열 20개를 다 써서 부족할 경우, 비상용으로 새로 하나 만듦
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(true);
            return obj;
        }
    }

    // 다 쓴 발판을 파괴하지 않고 다시 대기열로 돌려보내는 함수
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false); // 다시 숨김
        platformPool.Enqueue(obj); // 대기열 맨 뒤에 줄 세우기
    }
}