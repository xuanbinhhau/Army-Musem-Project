using UnityEngine;

// Gắn script này lên Main Camera.
// Main Camera phải là con (child) của object nhân vật (Player).
// Chuột trái/phải: xoay cả nhân vật. Chuột lên/xuống: ngẩng/cúi camera.
public class MouseLook : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Transform playerBody;          // để trống thì tự lấy object cha

    [Header("Cài đặt")]
    public float sensitivity = 2f;
    public float minPitch = -80f;         // giới hạn nhìn xuống
    public float maxPitch = 80f;          // giới hạn nhìn lên
    public bool invertY = false;
    public bool onlyWhenCursorLocked = true;

    float pitch;

    void Start()
    {
        if (playerBody == null) playerBody = transform.parent;

        if (playerBody == null)
            Debug.LogError("MouseLook: Camera cần là con của object nhân vật, hoặc hãy kéo Player vào ô Player Body.");

        // Khóa chuột nếu chưa có script nào khác làm việc này
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (playerBody == null) return;
        if (onlyWhenCursorLocked && Cursor.lockState != CursorLockMode.Locked) return;

        // Lưu ý: KHÔNG nhân với Time.deltaTime vì Mouse X/Y đã là độ dịch chuyển theo frame
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        pitch += invertY ? mouseY : -mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}