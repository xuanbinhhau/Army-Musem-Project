using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Gắn script này lên Main Camera (hoặc Player).
// Khi nhìn vào tranh (có PaintingInfo), panel UI sẽ hiện ảnh + thông tin.
public class PaintingViewer : MonoBehaviour
{
    [Header("Raycast")]
    public Camera cam;
    public float maxDistance = 5f;
    public LayerMask paintingLayer = ~0;   // mặc định: tất cả layer

    [Header("Tùy chọn")]
    public bool requireKeyPress = false;   // true: phải bấm phím mới hiện
    public KeyCode interactKey = KeyCode.E;
    public float fadeSpeed = 8f;

    [Header("UI")]
    public CanvasGroup panel;              // panel chứa toàn bộ UI (thêm CanvasGroup)
    public Image paintingImage;
    public TMP_Text titleText;
    public TMP_Text artistText;
    public TMP_Text yearText;
    public TMP_Text descriptionText;

    PaintingInfo current;
    float targetAlpha;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        SetPanel(0f);
    }

    void Update()
    {
        PaintingInfo looked = GetLookedPainting();

        bool show = looked != null &&
                    (!requireKeyPress || Input.GetKey(interactKey));

        if (looked != null && looked != current)
        {
            current = looked;
            FillUI(current);
        }

        targetAlpha = show ? 1f : 0f;
        panel.alpha = Mathf.MoveTowards(panel.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        panel.blocksRaycasts = panel.alpha > 0.9f;

        if (looked == null && panel.alpha <= 0f) current = null;
    }

    PaintingInfo GetLookedPainting()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, paintingLayer))
            return hit.collider.GetComponentInParent<PaintingInfo>();
        return null;
    }

    void FillUI(PaintingInfo info)
    {
        paintingImage.sprite = info.image;
        paintingImage.preserveAspect = true;
        titleText.text = info.title;
        artistText.text = info.artist;
        yearText.text = info.year;
        descriptionText.text = info.description;
    }

    void SetPanel(float a)
    {
        panel.alpha = a;
        panel.blocksRaycasts = false;
        panel.interactable = false;
    }
}