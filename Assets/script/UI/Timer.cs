using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    Scenemanager Scene;
    double time;

    void Start()
    {
        Scene = GameObject.Find("SceneCanvas").GetComponent<Scenemanager>();
    }
    void Update()
    {
        if (Scene.ready)
            return;
        time += Time.deltaTime;
        transform.GetComponent<TextMeshProUGUI>().text = string.Format("{0:F2}", time);
    }
}
