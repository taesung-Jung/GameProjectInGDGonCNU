using UnityEngine;
using System.Collections;
using TMPro;

public class Ready : MonoBehaviour
{
    public GameObject CountPrefab;
    GameObject _countPrefab;
    void Start()
    {
        _countPrefab = Instantiate(CountPrefab, GameObject.Find("MainUI").transform);
        StartCoroutine(count());
    }

    IEnumerator count()
    {
        
        for (int i = 3; i >= 0; i--)
        {
            if (i == 0)
            {
                _countPrefab.GetComponent<TextMeshProUGUI>().text = string.Format("START!");
            } else
            {
                _countPrefab.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", i);
            }
            yield return new WaitForSeconds(1.0f);
        }
        GameObject.Find("SceneCanvas").GetComponent<Scenemanager>().ready = false;
        Destroy(_countPrefab);
    }
}
