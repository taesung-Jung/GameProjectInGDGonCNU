using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 1f;
    public AudioClip deathClip;
    public AudioClip slideClip;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public CircleCollider2D circleCollider;

    private IPlayerState currentState;
    public bool isDead = false;
    public bool ignoreInput = false;
    public int jumpCount = 0;

    public DestructibleWall currentTouchingWall;

    [Header("Random State Change Settings")]
    public float minRunTime = 10f;
    public float maxRunTime = 30f;

    private float stateTimer = 0f;
    private float targetChangeTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        ChangeState(new RunState(this));
        ResetRunTimer();
    }

    void Update()
    {
        if (isDead) return;
        if (GameObject.Find("SceneCanvas").GetComponent<Scenemanager>().ready) return;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < 0f)
        {
            Debug.LogWarning("[���] ȭ�� ��Ż");
            Die();
            return;
        }

        if (currentState != null)
        {
            if (currentState.GetType() != typeof(FlightState) && currentState.GetType() != typeof(BreakState))
            {
                stateTimer += Time.deltaTime;

                if (stateTimer >= targetChangeTime)
                {
                    MapManager mapManager = FindObjectOfType<MapManager>();
                    if (mapManager != null)
                    {
                        mapManager.Invoke("ChangeMode", 0f);
                    }
                    ResetRunTimer();
                }
            }
        }
        currentState?.HandleInput();
        currentState?.UpdateState();
    }

    public void ResetRunTimer()
    {
        stateTimer = 0f;
        if (minRunTime > maxRunTime) minRunTime = maxRunTime;
        targetChangeTime = Random.Range(minRunTime, maxRunTime);
    }

    public void ChangeState(IPlayerState newState)
    {
        if (newState == null || isDead) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            currentTouchingWall = col.gameObject.GetComponent<DestructibleWall>();
            return;
        }
        CheckDeath(col.gameObject);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall")) currentTouchingWall = null;
    }

    private void OnTriggerEnter2D(Collider2D other) => CheckDeath(other.gameObject);

    private void CheckDeath(GameObject obj)
    {
        if (obj.name.Contains("GameObject") || obj.name.Contains("Dead"))
        {
            Debug.LogWarning("[���] �߶�");
            Die();
            return;
        }

        if (obj.name.Contains("Platform"))
        {
            return;
        }

        if (obj.CompareTag("Obstacle") || obj.GetComponent<ScrollingObject>() != null)
        {
            Debug.LogWarning("[���] ��ֹ� �浹");
            Die();
            return;
        }
    }

    public bool IsGrounded()
    {
        Vector2 position = (Vector2)transform.position + circleCollider.offset;
        float radius = circleCollider.radius;
        RaycastHit2D hit = Physics2D.Raycast(position + Vector2.down * radius, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        anim.SetTrigger("Die");

        if (audioSource != null && deathClip != null)
        {
            audioSource.clip = deathClip;
            audioSource.Play();
        }
        rb.linearVelocity = Vector2.zero;

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

        GameObject.Find("SceneCanvas").GetComponent<Scenemanager>().Gameover();
    }
}