using UnityEngine;

public class ToLobby : MonoBehaviour
{
    GameObject scene;
    void Awake()
    {
        scene = GameObject.Find("SceneCanvas");
    }
     
    public void clickToLobby()
    {
        if (scene.GetComponent<Loading>().loading) 
            return;
        
        scene.GetComponent<Scenemanager>().stop();
        Time.timeScale = 1f;
        scene.GetComponent<Loading>().LoadStart();
        Invoke("LoadScene",2f);
    }
    void LoadScene()
    {
        scene.GetComponent<Scenemanager>().ScenePass_ToStartPage();
    }
}
