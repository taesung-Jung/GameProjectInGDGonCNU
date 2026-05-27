using UnityEngine;
using System.Collections;
using TMPro;
public class GameOverEvent : MonoBehaviour
{
    public RectTransform targetUI;
    float moveDistance = 835f;
    float duration = 0.5f;
    void OnEnable()
    {
        targetUI = GameObject.Find("Record").GetComponent<RectTransform>();
        //GameObject.Find("RecordTimer").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().text;
        Invoke("StartMove", 2f);
    }

    void StartMove()
    {
        StartCoroutine(MoveUpCoroutine(targetUI, moveDistance));
        StartCoroutine(MoveUpCoroutine(transform.GetComponent<RectTransform>(), 300f));
    }
    IEnumerator MoveUpCoroutine(RectTransform targetUI, float moveDistance)
    {
        Vector2 startPos = targetUI.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, moveDistance);

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            // 부드러운 움직임
            t = Mathf.SmoothStep(0f, 1f, t);

            targetUI.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        targetUI.anchoredPosition = endPos;
    }
}
