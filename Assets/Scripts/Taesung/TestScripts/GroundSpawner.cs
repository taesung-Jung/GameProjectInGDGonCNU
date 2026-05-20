using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public Transform spawnPoint;

    public float spawnInterval = 2f;
    public float moveSpeed = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnGround();
            timer = 0f;
        }
    }

    void SpawnGround()
    {
        int index = Random.Range(0, groundPrefabs.Length);

        GameObject ground = Instantiate(
            groundPrefabs[index],
            spawnPoint.position,
            Quaternion.identity
        );

        GroundMover mover = ground.GetComponent<GroundMover>();

        if (mover != null)
        {
            mover.moveSpeed = moveSpeed;
        }
    }
}