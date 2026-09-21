using UnityEngine;
using UnityEngine.UI;

// Gắn script này lên Image crosshair (nằm giữa Canvas).
// Khi nhìn vào vật có PaintingInfo, crosshair đổi màu và phóng to nhẹ.
[RequireComponent(typeof(Image))]
public class Crosshair : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Camera cam;

    [Header("Raycast")]
    public float maxDistance = 5f;
    public LayerMask detectLayer = ~0;

    [Header("Giao diện")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.8f);
    public Color hoverColor = new Color(1f, 0.85f, 0.2f, 1f);
    public float normalSize = 12f;
    public float hoverSize = 20f;
    public float smooth = 12f;

    [Header("Chuột")]
    public bool lockCursor = true;

    Image image;
    RectTransform rect;

    void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        image.raycastTarget = false;
    }

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (lockCursor) SetCursorLocked(true);

        image.color = normalColor;
        rect.sizeDelta = Vector2.one * normalSize;
    }

    void Update()
    {
        HandleCursor();

        bool hovering = IsLookingAtInteractable();

        Color targetColor = hovering ? hoverColor : normalColor;
        float targetSize = hovering ? hoverSize : normalSize;

        float t = smooth * Time.deltaTime;
        image.color = Color.Lerp(image.color, targetColor, t);
        rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, Vector2.one * targetSize, t);
    }

    bool IsLookingAtInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, detectLayer))
            return hit.collider.GetComponentInParent<PaintingInfo>() != null;
        return false;
    }

    // Esc: mở khóa chuột. Click chuột: khóa lại.
    void HandleCursor()
    {
        if (!lockCursor) return;

        if (Input.GetKeyDown(KeyCode.Escape)) SetCursorLocked(false);
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            SetCursorLocked(true);
    }

    void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}