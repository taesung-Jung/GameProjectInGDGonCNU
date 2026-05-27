using UnityEngine;

public class FlightState : IPlayerState
{
    private PlayerController player;
    private float flapForce = 5f; // 한 번 클릭할 때 튀어오르는 힘
    private float forwardSpeed = 3f; // 비행 구역 내에서 자동으로 앞으로 가는 속도

    public FlightState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        player.anim.SetBool("IsFlying", true);
        player.rb.linearVelocity = Vector2.zero;
        player.rb.gravityScale = 0.8f;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 수직 속도를 0으로 초기화
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0);
            player.rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

            player.audioSource.Play();
        }

        // f 버튼 클릭 시 비행 상태로 변경
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.ChangeState(new RunState(player));
        }
    }

    public void UpdateState()
    {
        // 앞으로 계속 전진
        float h = Input.GetAxisRaw("Horizontal");
        player.rb.linearVelocity = new Vector2(h * forwardSpeed, player.rb.linearVelocity.y);
    }

    public void Exit()
    {
        player.anim.SetBool("IsFlying", false);
        player.rb.gravityScale = 1.0f;
    }
}