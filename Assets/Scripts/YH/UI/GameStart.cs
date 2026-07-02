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
        
        if (GameObject.Find("Player") != null) 
            scene.GetComponent<Scenemanager>().stop();
        Time.timeScale = 1f;
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
