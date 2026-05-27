using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerController player;

    public RunState(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void Enter()
    {
        player.anim.SetBool("Grounded", true);
    }

    public void HandleInput()
    {
        // 마우스 왼쪽 버튼 클릭 시 점프 상태로 변경
        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(new JumpState(player));
        }

        // 마우스 오른쪽 버튼 클릭 시 슬라이드 상태로 변경
        if (Input.GetMouseButtonDown(1))
        {
            player.ChangeState(new SlideState(player));
        }

        // f 버튼 클릭 시 비행 상태로 변경
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.ChangeState(new FlightState(player));
        }

        // b 버튼 클릭 시 공격 상태로 변경
        if (Input.GetKeyDown(KeyCode.B))
        {
            player.ChangeState(new BreakState(player, null));
        }
    }

    public void UpdateState() { }
    public void Exit() { }
}
