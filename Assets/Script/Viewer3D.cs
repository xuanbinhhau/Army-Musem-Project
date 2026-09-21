using UnityEngine;
using UnityEngine.EventSystems;

public class Viewer3D : MonoBehaviour
{
    public static bool IsViewing { get; private set; }

    [Header("Camera phụ đặt ở xa bảo tàng")]
    public Camera previewCamera;
    public float distance = 3f;
    public float objectSize = 1.5f;
    public float rotateSpeed = 5f;
    public float zoomSpeed = 30f;

    Camera mainCam;
    Transform pivot;

    void Start()
    {
        mainCam = GetComponent<Camera>();
        previewCamera.enabled = false;

        pivot = new GameObject("PreviewPivot").transform;
        pivot.position = previewCamera.transform.position
                       + previewCamera.transform.forward * distance;
    }

    void Update()
    {
        if (IsViewing) { UpdateViewing(); return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Viewable3D v = hit.collider.GetComponentInParent<Viewable3D>();
                if (v != null) StartViewing(v.gameObject);
            }
        }
    }

    void StartViewing(GameObject source)
    {
        pivot.rotation = Quaternion.identity;

        GameObject clone = Instantiate(source, pivot.position, Quaternion.identity);
        clone.transform.localScale = source.transform.lossyScale;

        Renderer[] rs = clone.GetComponentsInChildren<Renderer>();
        if (rs.Length == 0) { Destroy(clone); return; }

        Bounds b = GetBounds(rs);
        float max = Mathf.Max(b.size.x, b.size.y, b.size.z);
        if (max > 0f) clone.transform.localScale *= objectSize / max;

        b = GetBounds(rs);
        clone.transform.position += pivot.position - b.center;
        clone.transform.SetParent(pivot, true);

        // Ẩn panel thông tin (nếu đang mở)
        MuseumViewer mv = GetComponent<MuseumViewer>();
        if (mv != null) mv.HidePanel();

        previewCamera.fieldOfView = 60f;
        mainCam.enabled = false;
        previewCamera.enabled = true;
        IsViewing = true;
    }

    void UpdateViewing()
    {
        if (Input.GetMouseButton(0))
        {
            float dx = Input.GetAxis("Mouse X") * rotateSpeed;
            float dy = Input.GetAxis("Mouse Y") * rotateSpeed;
            pivot.Rotate(Vector3.up, -dx, Space.World);
            pivot.Rotate(Vector3.right, dy, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        previewCamera.fieldOfView =
            Mathf.Clamp(previewCamera.fieldOfView - scroll * zoomSpeed, 20f, 80f);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            StopViewing();
    }

    void StopViewing()
    {
        foreach (Transform child in pivot) Destroy(child.gameObject);
        previewCamera.enabled = false;
        mainCam.enabled = true;
        IsViewing = false;
    }

    Bounds GetBounds(Renderer[] rs)
    {
        Bounds b = rs[0].bounds;
        foreach (Renderer r in rs) b.Encapsulate(r.bounds);
        return b;
    }
}