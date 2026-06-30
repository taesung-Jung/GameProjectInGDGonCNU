using UnityEngine;

public class BreakState : IPlayerState
{
    private PlayerController player;
    private DestructibleWall targetWall; 
    private float runSpeed;
    private bool isWallDestroyed = false;

    public BreakState(PlayerController playerController, DestructibleWall wall) //컴파일 오류 지점(장애물때문에)
    {
        this.player = playerController;
        this.targetWall = wall;
    }

    public void Enter()
    {
        player.jumpCount = 0; // 모드 변경 시 점프 횟수 초기화
        runSpeed = player.rb.linearVelocity.x;
        if (runSpeed < 5f) runSpeed = 8f;

        player.anim.SetBool("IsBreaking", true);
        isWallDestroyed = false;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !isWallDestroyed)
        {
            Attack();
        }

        // B 버튼 클릭 시 달리기 상태로 변경
        if (Input.GetKeyDown(KeyCode.B))
        {
            player.ChangeState(new RunState(player));
        }
    }

    private void Attack()
    {
        player.anim.SetTrigger("Attack");
        player.audioSource.Play();

        if (targetWall != null && targetWall.TakeDamage())
        {
            isWallDestroyed = true;
            // 벽이 부서지면 즉시 달리기 상태로 복귀
            player.ChangeState(new RunState(player));
        }
    }

    public void UpdateState()
    {
 
    }

    public void Exit()
    {
        player.anim.SetBool("IsBreaking", false);
    }
}