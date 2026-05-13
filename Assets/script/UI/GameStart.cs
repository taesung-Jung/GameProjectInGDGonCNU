using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    GameObject scene;
    void Awake()
    {
        scene = GameObject.Find("SceneCanvas");
    }
     
    public void clickStart()
    {
        scene.GetComponent<Loading>().LoadStart();
        Invoke("LoadScene",2f);
    }
    void LoadScene()
    {
        scene.GetComponent<Scenemanager>().ScenePass();
    }
}
