using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private Vector3 velocity;

    void Update()
    {
        // Lấy input
        float x = Input.GetAxis("Horizontal"); // A D
        float z = Input.GetAxis("Vertical");   // W S

        // Hướng di chuyển theo hướng của Player
        Vector3 move = transform.right * x + transform.forward * z;

        // Di chuyển
        controller.Move(move * speed * Time.deltaTime);

        // Kiểm tra đang đứng trên mặt đất
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Nhảy
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;

        // Áp dụng trọng lực
        controller.Move(velocity * Time.deltaTime);
    }
}