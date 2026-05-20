using UnityEngine;

public class GroundMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        
        if (transform.position.x < -30f)
        {
            Destroy(gameObject);
        }
    }
}
