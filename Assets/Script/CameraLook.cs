using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Chỉ xoay khi GIỮ chuột phải
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") 
                         * mouseSensitivity 
                         * Time.deltaTime;

            float mouseY = Input.GetAxis("Mouse Y") 
                         * mouseSensitivity 
                         * Time.deltaTime;

            // Xoay lên / xuống
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(
                xRotation,
                0f,
                0f
            );

            // Xoay trái / phải
            transform.parent.Rotate(
                Vector3.up * mouseX
            );
        }
    }
}
