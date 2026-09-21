using UnityEngine;

// Gắn script này lên mỗi bức tranh (object phải có Collider).
public class PaintingInfo : MonoBehaviour
{
    [Header("Hình ảnh hiển thị trên màn hình")]
    public Sprite image;

    [Header("Thông tin văn bản")]
    public string title;
    public string artist;
    public string year;

    [TextArea(4, 12)]
    public string description;
}