using Unity.VectorGraphics;
using UnityEngine;

public class rank : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject rankpanel;
    GameObject scene;
    void Update()
    {
        scene = GameObject.Find("SceneCanvas"); 
    }
    public void rankpanelEmerge()
    {
        if (scene.GetComponent<Loading>().loading) 
            return;
        rankpanel.SetActive(true);
    }
}
