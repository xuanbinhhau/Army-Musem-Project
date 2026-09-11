using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Tốc độ di chuyển")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float slowSpeed = 2.5f;

    [Header("Nhảy & trọng lực")]
    public float jumpForce = 8f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (spawnPoint != null)
        {
            controller.enabled = false;

            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            controller.enabled = true;
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // =========================
        // TỐC ĐỘ
        // =========================

        float speed = walkSpeed;

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = runSpeed;
        }
        else if (Keyboard.current.rightShiftKey.isPressed)
        {
            speed = slowSpeed;
        }

        // =========================
        // DI CHUYỂN WASD
        // =========================

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        controller.Move(move.normalized * speed * Time.deltaTime);

        // =========================
        // TRỌNG LỰC
        // =========================

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}