using UnityEngine;

/// <summary>
/// Script di chuyển cho Cube — dùng khi Main Camera đã được kéo làm
/// GameObject con (child) của Cube trong Hierarchy.
/// Vì Camera là con của Cube nên khi Cube di chuyển/xoay, Camera sẽ
/// tự động đi theo mà KHÔNG cần script camera riêng.
///
/// CÁCH SETUP:
/// 1. Gắn script này vào Cube.
/// 2. Trong Hierarchy, kéo thả "Main Camera" thả vào bên trong "Cube"
///    (để nó trở thành con của Cube) — đúng như bạn vừa làm.
/// 3. Chọn Main Camera, chỉnh Position ở Inspector thành khoảng
///    (0, 3, -6) và Rotation X khoảng 20 độ để camera nhìn chéo
///    từ phía sau xuống Cube.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CubeMovement : MonoBehaviour
{
    [Header("Thông số di chuyển")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Xoay Cube theo hướng di chuyển")]
    public bool rotateTowardMoveDirection = true; // Camera là con nên sẽ xoay theo Cube

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input WASD / phím mũi tên (theo hướng thế giới)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(horizontal, 0f, vertical);

        if (moveDir.magnitude >= 0.1f)
        {
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            if (rotateTowardMoveDirection)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}