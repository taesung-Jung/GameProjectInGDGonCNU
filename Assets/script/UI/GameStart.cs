using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    GameObject scene;
    void Update()
    {
        scene = GameObject.Find("SceneCanvas");
    }
     
    public void clickStart()
    {
        if (scene.GetComponent<Loading>().loading) 
            return;
        Time.timeScale = 1.0f;
        scene.GetComponent<Loading>().LoadStart();
        scene.GetComponent<Scenemanager>().ready = true;
        scene.GetComponent<Scenemanager>().End = false;
        Invoke("LoadScene",2f);
    }
    void LoadScene()
    {
        scene.GetComponent<Scenemanager>().ScenePass_ToGamePage();
    }
}
