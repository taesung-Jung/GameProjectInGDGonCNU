using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerController player;
    private float elapsedTime = 0f;
    private const float GROUND_CHECK_COOLDOWN = 0.1f;

    public JumpState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        player.anim.SetBool("Grounded", false);
        elapsedTime = 0f;

        // ���� ���� �� DoJump ȣ��
        DoJump();
    }

    public void HandleInput()
    {
        // PlayerController�� jumpCount�� �����Ͽ� 2�� ���� üũ
        if (Input.GetMouseButtonDown(0) && player.jumpCount < 2)
        {
            if (player.ignoreInput)
                return;
            DoJump();
        }

        if (Input.GetMouseButtonUp(0) && player.rb.linearVelocity.y > 0)
        {
            if (player.ignoreInput)
                return;
            player.rb.linearVelocity *= 0.5f;
        }
    }

    private void DoJump()
    {
        player.jumpCount++; // PlayerController�� jumpCount ����

        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0);
        player.rb.AddForce(new Vector2(0, player.jumpForce));
        player.audioSource.Play();
    }

    public void UpdateState()
    {
        elapsedTime += Time.deltaTime;
        float currentVelocityY = player.rb.linearVelocity.y;

        if (elapsedTime >= GROUND_CHECK_COOLDOWN && currentVelocityY <= 0.01f)
        {
            if (IsGrounded())
            {
                player.ChangeState(new RunState(player));
            }
        }
    }

    private bool IsGrounded()
    {
        Vector2 colliderCenter = player.circleCollider.bounds.center;
        float radius = player.circleCollider.radius;

        RaycastHit2D hit = Physics2D.CircleCast(colliderCenter, radius * 0.9f, Vector2.down, radius * 0.8f, player.groundLayer);

        return hit.collider != null;
    }

    public void Exit()
    {
        player.anim.SetBool("Grounded", true);
    }
}