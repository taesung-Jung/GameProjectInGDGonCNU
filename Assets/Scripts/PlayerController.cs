using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 700f;

    // 🌟 (신규) 복귀를 위한 변수들
    public float recoverySpeed = 3f; // 제자리로 돌아가는 속도
    private float defaultX;          // 게임 시작 시 캐릭터의 원래 X 위치

    private int jumpCount = 0;
    private bool isDead = false;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;

    private DestructibleWall targetWall;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // 🌟 게임을 시작할 때, 캐릭터의 원래 X 좌표를 기억해 둡니다!
        defaultX = transform.position.x;
    }

    void Update()
    {
        if (isDead) return;

        if (targetWall != null && targetWall.gameObject == null)
        {
            targetWall = null;
        }

        // 🌟 [신규 복귀 로직] 
        // 1. 눈앞에 때릴 벽이 없고 (파괴했거나 평소 모드)
        // 2. 내 현재 위치가 원래 자리(defaultX)보다 왼쪽으로 밀려나 있다면?
        if (targetWall == null && transform.position.x < defaultX)
        {
            // 원래 자리를 향해 서서히 앞으로(오른쪽으로) 이동합니다!
            float newX = Mathf.MoveTowards(transform.position.x, defaultX, recoverySpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        // (아래 조작 로직은 그대로 둡니다)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (targetWall != null)
            {
                targetWall.TakeDamage();
            }
            else
            {
                if (jumpCount < 2)
                {
                    Jump();
                }
            }
        }
    }


    void Jump()
    {
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.AddForce(new Vector2(0, jumpForce));
        jumpCount++; 
    }

    private void Die() {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerRigidbody.linearVelocity = Vector2.zero; 
        isDead = true;

        PlatformSpawner spawner = FindObjectOfType<PlatformSpawner>();
        if (spawner != null)
        {
            spawner.currentSpeed = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥을 밟았을 때만 점프 횟수를 '0'으로 리셋해 줍니다.
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.7f)
        {
            jumpCount = 0;
        }

        DestructibleWall wall = collision.gameObject.GetComponent<DestructibleWall>();
        if (wall != null)
        {
            targetWall = wall;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // ❌ [삭제된 코드] 무조건 isGrounded = false; 로 만들던 주범 코드를 삭제했습니다! ❌

        // 벽과 멀어졌을 때만 타겟을 해제합니다.
        if (collision.gameObject.GetComponent<DestructibleWall>() != null)
        {
            targetWall = null;
        }
    }
}