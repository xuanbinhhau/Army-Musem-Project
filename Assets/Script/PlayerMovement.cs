using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody rb;
    Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Đọc input ở Update
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        direction = new Vector3(x, 0, z);
    }

    void FixedUpdate()
    {
        // Di chuyển bằng vật lý ở FixedUpdate
        Vector3 move = transform.TransformDirection(direction) * speed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }
}