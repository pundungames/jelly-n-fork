using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] private bool isLoop;
    
    [SerializeField] private float decreaseAmount = 1f;

    [SerializeField] private GameObject triggerObject;
    [SerializeField] private Vector3 triggerOffset;
    [SerializeField] private float triggerDuration;
    
    bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Softbody>(out _))
            return;

        if (isTriggered)
            return;

        isTriggered = true;

        transform.DOLocalMoveY(
            transform.localPosition.y - decreaseAmount,
            0.3f
        ).OnComplete(() =>
        {
            triggerObject.transform.DORotate(
                triggerOffset,
                triggerDuration,
                RotateMode.LocalAxisAdd);

            if (isLoop)
            {
                transform.DOLocalMoveY(
                    transform.localPosition.y + decreaseAmount,
                    0.3f
                ).OnComplete(() =>
                {
                    isTriggered = false;
                });
            }
        });
    }
}