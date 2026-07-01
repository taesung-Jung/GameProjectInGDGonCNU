using UnityEngine;

public class BreakState : IPlayerState
{
    private PlayerController player;
    private bool isWallDestroyed = false;
    private DestructibleWall targetWall;


    public BreakState(PlayerController playerController, DestructibleWall wall)
    {
        this.player = playerController;
        this.targetWall = wall;
    }

    public void Enter()
    {
        player.jumpCount = 0;
        player.anim.SetBool("IsBreaking", true);
        isWallDestroyed = false;

        if (player.currentTouchingWall == null)
        {
            player.currentTouchingWall = targetWall;
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isWallDestroyed)
        {
            Attack();
        }
    }

    private void Attack()
    {
        player.anim.SetTrigger("Attack");

        if (player.audioSource != null)
        {
            player.audioSource.Play();
        }

        DestructibleWall activeWall = player.currentTouchingWall;

        if (activeWall == null)
        {
            activeWall = Object.FindFirstObjectByType<DestructibleWall>();
        }

        if (activeWall != null && activeWall.TakeDamage())
        {
            isWallDestroyed = true;

            MapManager mapManager = Object.FindFirstObjectByType<MapManager>();
            if (mapManager != null)
            {
                mapManager.EndWallMode();
            }
            else
            {
                player.ChangeState(new RunState(player));
            }
        }
    }

    public void UpdateState()
    {

    }

    public void Exit()
    {
        player.anim.SetBool("IsBreaking", false);
        player.currentTouchingWall = null;
    }
}