using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
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

    public void stop()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().isDead = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator animator = player.GetComponent<Animator>();
        rb.gravityScale = 0f;      // 중력 제거
        rb.linearVelocity = Vector2.zero; // 현재 속도 제거
        rb.angularVelocity = 0f;
        rb.simulated = false;
        animator.enabled = false;
        
        PlatformSpawner spawner = Object.FindFirstObjectByType<PlatformSpawner>();
        if (spawner != null)
        {
            spawner.currentSpeed = 0f;
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.isGameover = true;
            if (GameManager.instance.gameoverUI != null)
            {
                GameManager.instance.gameoverUI.SetActive(true);
            }
        }
        


    }
}
