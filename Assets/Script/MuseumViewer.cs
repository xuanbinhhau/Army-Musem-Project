using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MuseumViewer : MonoBehaviour
{
    [Header("Xoay camera")]
    public float sensitivity = 3f;

    [Header("UI")]
    public GameObject panel;
    public Image paintingImage;
    public TMP_Text titleText;
    public TMP_Text artistText;
    public TMP_Text yearText;
    public TMP_Text descriptionText;

    Camera cam;
    float yaw, pitch;

    void Start()
    {
        cam = GetComponent<Camera>();

        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x > 180f ? e.x - 360f : e.x;

        panel.SetActive(false);
    }

    void Update()
    {
        // 1. Giữ chuột phải để xoay camera
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // 2. Click chuột trái để chọn tranh
        if (Input.GetMouseButtonDown(0))
        {
            // Đang click lên UI thì bỏ qua
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                PaintingInfo info = hit.collider.GetComponentInParent<PaintingInfo>();
                if (info != null)
                {
                    Show(info);
                    return;
                }
            }
            panel.SetActive(false); // click ra ngoài thì ẩn
        }

        if (Input.GetKeyDown(KeyCode.Escape)) panel.SetActive(false);
    }

    void Show(PaintingInfo info)
    {
        paintingImage.sprite = info.image;
        paintingImage.preserveAspect = true;
        titleText.text = info.title;
        artistText.text = info.artist;
        yearText.text = info.year;
        descriptionText.text = info.description;
        panel.SetActive(true);
    }
}