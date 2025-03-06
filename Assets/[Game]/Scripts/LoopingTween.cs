using System;
using UnityEngine;
using DG.Tweening;

public class LoopingTween : MonoBehaviour
{
    public float duration = 1f; // Hareket süresi
    public bool isPositionActive;
    public Vector3 positionOffset = Vector3.zero; // Pozisyon değişikliği
    public bool isRotationActive;
    public Vector3 rotationAngle = new Vector3(0, 0, 0); // Rotasyon açısı

    private Tween positionTween;
    private Tween rotationTween;

    private void Start()
    {
        Animate();
    }

    private void Animate()
    {
        if (!isPositionActive && !isRotationActive)
            return;


        if (isPositionActive)
        {
            Vector3 startPos = transform.localPosition;
            positionTween = transform.DOLocalMove(startPos + positionOffset, duration).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        if (isRotationActive)
        {
            Quaternion targetRot = Quaternion.Euler(rotationAngle); // Hedef rotasyon
            rotationTween = transform.DOLocalRotateQuaternion(targetRot, duration).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnDestroy()
    {
        positionTween?.Kill();
        rotationTween?.Kill();
    }

    private void OnDisable()
    {
        positionTween?.Kill();
        rotationTween?.Kill();
    }
}