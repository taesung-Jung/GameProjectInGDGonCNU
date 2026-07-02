using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class Scenemanager : MonoBehaviour
{
    public static Scenemanager Instance;
    public GameObject gameOver_prefab;
    GameObject _gameOver_prefab;
    public string UserName;
    public int Time;
    public bool ready = false;
    public bool End = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Gameover();
        }
    }

    public void ScenePass_ToGamePage()
    {
        ready = true;
        SceneManager.LoadScene("MapTest_Yeomin");
        transform.GetComponent<Loading>().LoadEnd();
    }

    public void ScenePass_ToStartPage()
    {
        End = false;
        GameObject.Find("Record").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -835f);
        SceneManager.LoadScene("StartPage");
        transform.GetComponent<Loading>().LoadEnd();
        
    }

    public void Gameover()
    {
        End = true;
        _gameOver_prefab = Instantiate(gameOver_prefab, GameObject.Find("MainUI").transform);
    }
}
