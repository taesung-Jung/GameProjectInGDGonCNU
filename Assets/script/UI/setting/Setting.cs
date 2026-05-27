using UnityEngine;

public class Setting : MonoBehaviour
{
    GameObject scene;
    public GameObject settingpanel;
    void Update()
    {
        scene = GameObject.Find("SceneCanvas");
    }
    public void settingpanelEmerge()
    {
        if (scene.GetComponent<Loading>().loading) 
            return;
        settingpanel.SetActive(true);
    }
}
