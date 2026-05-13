using UnityEngine;
using System.Collections;
public class Loading : MonoBehaviour
{

    
    public float speed = 10f;
    RectTransform LoadScreen;
    void Start()
    {
        LoadScreen = GameObject.Find("LoadScreen").GetComponent<RectTransform>();
    }
    public void LoadStart()
    {
        StartCoroutine(Loadfirst());
    }

    public void LoadEnd()
    {
        StartCoroutine(Loadsecond());
    }

    IEnumerator Loadfirst()
    {
        LoadScreen.offsetMax = new Vector2(LoadScreen.offsetMax.x, 1080);
        LoadScreen.offsetMin = new Vector2(LoadScreen.offsetMin.x, 1080);
        for (int i = 1080; i >= 0; i-=10)
        {
            LoadScreen.offsetMax = new Vector2(LoadScreen.offsetMax.x, i);
            LoadScreen.offsetMin = new Vector2(LoadScreen.offsetMin.x, i);
            speed = Mathf.Lerp(0.01f, 0.1f, 1/(float)(i + 1) );

            yield return new WaitForSeconds(speed/2);
        }
    }
    IEnumerator Loadsecond()
    {
        LoadScreen.offsetMax = new Vector2(LoadScreen.offsetMax.x, 0);
        LoadScreen.offsetMin = new Vector2(LoadScreen.offsetMin.x, 0);
        for (int i = 0; i <= 1080; i+=10)
        {
            LoadScreen.offsetMax = new Vector2(LoadScreen.offsetMax.x, -i);
            LoadScreen.offsetMin = new Vector2(LoadScreen.offsetMin.x, -i);
            speed = Mathf.Lerp(0.01f, 0.1f, (float)(i + 1)/10800);

            yield return new WaitForSeconds(speed/2);
        }
    }
}
