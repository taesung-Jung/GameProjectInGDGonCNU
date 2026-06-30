using UnityEngine;

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
    public int jumpCount = 0;

    // --- 벽 파괴를 위한 변수 추가 ---
    private DestructibleWall currentTouchingWall;
    // ----------------------------

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
    }

    void Update()
    {
        if (isDead) return;

        // 벽 파괴 로직: 좌클릭 시 닿아있는 벽에 데미지 전달
        if (Input.GetMouseButtonDown(0))
        {
            if (currentTouchingWall != null)
            {
                currentTouchingWall.TakeDamage();
            }
        }

        currentState?.HandleInput();
        currentState?.UpdateState();
    }

    // --- 충돌 감지 로직 추가 ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            currentTouchingWall = collision.gameObject.GetComponent<DestructibleWall>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            currentTouchingWall = null;
        }
    }
    // -------------------------

    public bool IsGrounded()
    {
        Vector2 position = (Vector2)transform.position + circleCollider.offset;
        float radius = circleCollider.radius;

        RaycastHit2D hit = Physics2D.Raycast(position + Vector2.down * radius, Vector2.down, groundCheckDistance, groundLayer);

        Debug.DrawRay(position + Vector2.down * radius, Vector2.down * groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        audioSource.clip = deathClip;
        audioSource.Play();
        rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Dead") || other.CompareTag("Obstacle"))
        {
            Die();
        }
    }
}