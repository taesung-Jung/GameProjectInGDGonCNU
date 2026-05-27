using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerController player;
    private int jumpCount = 0;

    public JumpState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        DoJump();
    }

    public void HandleInput()
    {
        // 공중에서 한 번 더 클릭하면 2단 점프
        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            DoJump();
        }

        // 마우스 떼면 상승 속도 절반 (가변 점프)
        if (Input.GetMouseButtonUp(0) && player.rb.linearVelocity.y > 0)
        {
            player.rb.linearVelocity *= 0.5f;
        }
    }

    private void DoJump()
    {
        jumpCount++;
        player.rb.linearVelocity = Vector2.zero;
        player.rb.AddForce(new Vector2(0, player.jumpForce));
        player.audioSource.Play();
    }

    public void UpdateState()
    {
        // 다시 바닥에 떨어졌는지 체크 (간단하게 속도로 체크)
        if (player.rb.linearVelocity.y == 0)
        {
            player.ChangeState(new RunState(player));
        }
    }

    public void Exit()
    {
        player.anim.SetBool("Grounded", false);
    }
}