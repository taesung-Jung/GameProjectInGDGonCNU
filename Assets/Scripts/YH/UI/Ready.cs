using UnityEngine;
using System.Collections;
using TMPro;

public class Ready : MonoBehaviour
{
    [Header("프리팹 연결 (비어있어도 자동 작동)")]
    public GameObject CountPrefab;
    private TextMeshProUGUI targetText;

    void Start()
    {
        Time.timeScale = 0f;

        if (CountPrefab != null)
        {
            GameObject spawned = Instantiate(CountPrefab, GameObject.Find("MainUI").transform);
            targetText = spawned.GetComponent<TextMeshProUGUI>();
            if (targetText == null) targetText = spawned.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            targetText = GetComponent<TextMeshProUGUI>();
            if (targetText == null) targetText = GetComponentInChildren<TextMeshProUGUI>();
        }

        StartCoroutine(count());
    }

    IEnumerator count()
    {
        for (int i = 3; i >= 0; i--)
        {
            if (targetText != null)
            {
                if (i == 0) targetText.text = "START!";
                else targetText.text = i.ToString();
            }

            yield return new WaitForSecondsRealtime(1.0f);
        }

        Time.timeScale = 1f;

        GameObject sceneCanvas = GameObject.Find("SceneCanvas");
        if (sceneCanvas != null)
        {
            Scenemanager sm = sceneCanvas.GetComponent<Scenemanager>();
            if (sm != null) sm.ready = false;
        }

        if (CountPrefab != null && targetText != null && targetText.gameObject != gameObject)
        {
            Destroy(targetText.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}