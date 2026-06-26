using UnityEngine;

public class SlideState : IPlayerState
{
    private PlayerController player;
    private float originalRadius;
    private Vector2 originalOffset;

    public SlideState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        player.anim.SetBool("IsSliding", true);

        originalRadius = player.circleCollider.radius;
        originalOffset = player.circleCollider.offset;

        player.circleCollider.radius = originalRadius * 0.5f;
        player.circleCollider.offset = new Vector2(originalOffset.x, originalOffset.y - (originalRadius * 0.25f));

        if (player.slideClip != null)
        {
            player.audioSource.clip = player.slideClip;
            player.audioSource.Play();
        }
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonUp(1))
        {
            player.ChangeState(new RunState(player));
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(new JumpState(player));
        }
    }

    public void UpdateState()
    {
        float currentY = player.rb.linearVelocity.y;
    }

    public void Exit()
    {
        player.anim.SetBool("IsSliding", false);

        player.circleCollider.radius = originalRadius;
        player.circleCollider.offset = originalOffset;
    }
}