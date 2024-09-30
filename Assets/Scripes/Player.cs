using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;
    public float moveSpeed = 5f;

    private int jumpCount; // Track the number of jumps
    private const int maxJumpCount = 2; // Maximum number of jumps (double jump)

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        jumpCount = 0; // Reset jump count when enabled
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;
            jumpCount = 0; // Reset jump count when grounded

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.up * jumpForce;
                jumpCount++; // Increment jump count on first jump
            }
        }
        else if (jumpCount < maxJumpCount && Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector3.up * jumpForce; // Apply jump force for double jump
            jumpCount++; // Increment jump count on double jump
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            character.Move(Vector3.left * moveSpeed * Time.deltaTime * 1.5f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            character.Move(Vector3.right * moveSpeed * Time.deltaTime);
        }

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
