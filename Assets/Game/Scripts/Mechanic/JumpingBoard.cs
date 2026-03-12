using DG.Tweening;
using UnityEngine;

public class JumpingBoard : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform padVisual;
    [SerializeField] float scaleAmount;
    [SerializeField] float scaleDuration = 0.15f;
    [SerializeField] float jumpForce = 15f;

    bool isJumping;
    Vector3 startScale;

    void Awake()
    {
        startScale = padVisual.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isJumping) return;
        
        if (other.gameObject.TryGetComponent(out Softbody softbody))
        {
            isJumping = true;

            softbody.obiActor.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                padVisual.DOKill();
                padVisual.DOScaleY(startScale.y * scaleAmount, scaleDuration)
                    .SetEase(Ease.OutElastic)
                    .OnComplete(() =>
                    {
                        padVisual.DOScaleY(startScale.y, scaleDuration)
                            .SetEase(Ease.InOutSine)
                            .OnComplete(() => isJumping = false);
                    });
            });
        }
    }
}
