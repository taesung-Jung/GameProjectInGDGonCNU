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

        // 원래 크기 저장
        originalRadius = player.circleCollider.radius;
        originalOffset = player.circleCollider.offset;

        // 히트박스 크기를 절반 정도로 줄임 (캐릭터에 맞춰 조절하세요)
        player.circleCollider.radius = originalRadius * 0.5f;
        // 캐릭터가 바닥에 붙도록 오프셋을 아래로 내림
        player.circleCollider.offset = new Vector2(originalOffset.x, originalOffset.y - (originalRadius * 0.25f));
    }

    public void HandleInput()
    {
        // 1. 마우스 오른쪽 버튼(슬라이드 키)에서 손을 떼면 즉시 달리기 상태로 복귀
        if (Input.GetMouseButtonUp(1))
        {
            player.ChangeState(new RunState(player));
        }

        // 2. 슬라이딩 도중 점프(좌클릭 0)를 누르면 슬라이드를 끊고 점프
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

        // 상태가 끝나면 히트박스를 원래대로 복구
        player.circleCollider.radius = originalRadius;
        player.circleCollider.offset = originalOffset;
    }
}