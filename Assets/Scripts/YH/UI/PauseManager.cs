using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        var pc = GameObject.Find("Player").GetComponent<PlayerController>();
        if (pc != null) pc.ignoreInput = true;   // pause 중엔 처음부터 입력 무시
        isPaused = true;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        GameObject.Find("Player").GetComponent<PlayerController>().ignoreInput = true;
        StartCoroutine(EnableInput());

        isPaused = false;
    }

    IEnumerator EnableInput()
    {
        yield return new WaitForEndOfFrame(); // 현재 프레임(버튼 클릭 이벤트 포함)이 완전히 끝난 후
        yield return null;                    // 한 프레임 더 넘겨서 확실히 다음 입력부터만 허용
        GameObject.Find("Player").GetComponent<PlayerController>().ignoreInput = false;
    }
}
