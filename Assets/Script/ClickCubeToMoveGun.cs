using UnityEngine;
using System.Collections;

public class ClickCubeToMoveGun : MonoBehaviour
{
    [Header("Đối tượng súng cần di chuyển")]
    public Transform gunTransform;

    [Header("Vị trí & góc xoay đích")]
    public Vector3 targetPosition;
    public Vector3 targetRotationEuler;

    [Header("Tốc độ di chuyển")]
    public float duration = 1.5f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isAtTarget = false;
    private bool isMoving = false;

    void Start()
    {
        startPosition = gunTransform.position;
        startRotation = gunTransform.rotation;
    }

    // Unity tự gọi hàm này khi người dùng click chuột trái vào object có Collider này
    void OnMouseDown()
    {
        if (isMoving) return; // tránh click liên tục khi đang di chuyển

        StopAllCoroutines();
        if (!isAtTarget)
        {
            StartCoroutine(MoveAndRotate(gunTransform.position, gunTransform.rotation,
                targetPosition, Quaternion.Euler(targetRotationEuler)));
        }
        else
        {
            StartCoroutine(MoveAndRotate(gunTransform.position, gunTransform.rotation,
                startPosition, startRotation));
        }
        isAtTarget = !isAtTarget;
    }

    IEnumerator MoveAndRotate(Vector3 fromPos, Quaternion fromRot, Vector3 toPos, Quaternion toRot)
    {
        isMoving = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            gunTransform.position = Vector3.Lerp(fromPos, toPos, t);
            gunTransform.rotation = Quaternion.Slerp(fromRot, toRot, t);
            yield return null;
        }
        gunTransform.position = toPos;
        gunTransform.rotation = toRot;
        isMoving = false;
    }
}