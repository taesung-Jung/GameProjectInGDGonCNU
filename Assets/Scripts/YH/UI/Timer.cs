using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    Scenemanager Scene;
    public double time;

    void Start()
    {
        
    }
    void Update()
    {
        Scene = GameObject.Find("SceneCanvas").GetComponent<Scenemanager>();
        if (Scene.ready)
            return;
        if (Scene.End)
            return;
        time += Time.deltaTime;
        transform.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", (int)time);
    }
}
