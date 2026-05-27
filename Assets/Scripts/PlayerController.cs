using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 1f;
    public AudioClip deathClip;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public CircleCollider2D circleCollider;

    private IPlayerState currentState;
    public bool isDead = false;

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

        currentState?.HandleInput();
        currentState?.UpdateState();
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

    // 추가: 벽 파괴 구역(Trigger)에 닿으면 BreakState 진입
        if (other.CompareTag("BreakZone"))
        {
            // 장애물이 아직 없으므로 임시로 null 넘김
            ChangeState(new BreakState(this, null));
        }

        if (other.CompareTag("FlightZone"))
        {
            ChangeState(new FlightState(this));
        }
    }
}