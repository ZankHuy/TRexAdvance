using UnityEngine;

public class BossObject : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    private void Update()
    {
        // Move the object to the left each frame
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Optional: Destroy the object when it's off-screen
        if (transform.position.x < -10f) // Adjust based on your game's boundary
        {
            Destroy(gameObject);
        }
    }
}