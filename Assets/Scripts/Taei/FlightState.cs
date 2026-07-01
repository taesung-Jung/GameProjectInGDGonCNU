using UnityEngine;

public class FlightState : IPlayerState
{
    private PlayerController player;
    private float flapForce = 5f;
    private float forwardSpeed = 3f;

    private float elapsedTime = 0f;
    private const float FLIGHT_DURATION = 10.0f;

    private AudioClip boost;

    public FlightState(PlayerController playerController)
    {
        this.player = playerController;
        boost = Resources.Load<AudioClip>("Audio/boost");
    }

    public void Enter()
    {
        elapsedTime = 0f;
        player.jumpCount = 0;
        player.anim.SetBool("IsFlying", true);
        player.rb.linearVelocity = Vector2.zero;
        player.rb.gravityScale = 0.8f;

        if (player.audioSource != null && boost != null)
        {
            player.audioSource.clip = boost;
        }
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0);
            player.rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

            if (player.audioSource != null && boost != null)
            {
                player.audioSource.PlayOneShot(boost);
            }
        }
    }

    public void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= FLIGHT_DURATION)
        {
            player.ChangeState(new RunState(player));

            MapManager mapManager = Object.FindFirstObjectByType<MapManager>();
            if (mapManager != null && mapManager.platformSpawner != null)
            {
                mapManager.platformSpawner.TriggerNormalMode(mapManager.normalSpeed);
            }
        }
    }

    public void Exit()
    {
        player.anim.SetBool("IsFlying", false);
        player.rb.gravityScale = 2.0f;

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in obstacles)
        {
            if (obj.transform.position.x > player.transform.position.x)
            {
                Object.Destroy(obj);
            }
        }
    }
}