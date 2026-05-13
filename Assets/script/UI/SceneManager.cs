using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanager : MonoBehaviour
{
    public static Scenemanager Instance;
    public string UserName;
    public int Time;
    public bool ready = false;

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

    public void ScenePass()
    {
        ready = true;
        SceneManager.LoadScene("GamePage");
        transform.GetComponent<Loading>().LoadEnd();
    }
}
