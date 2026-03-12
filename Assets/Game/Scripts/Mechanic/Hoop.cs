using DG.Tweening;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [Header("Ray Positions")]
    [SerializeField] private Transform topRayPos;
    [SerializeField] private Transform bottomRayPos;

    [Header("Settings")]
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private float force;

    [Header("Visual Settings")] 
    [SerializeField] float angleAmount = 1.3f;
    [SerializeField] float angleDuration = 0.2f;
    
    private bool basket = false;
    Tween rotTween;

    void FixedUpdate()
    {
        if (rotTween != null && rotTween.IsActive())
            return;
        
        RaycastHit hit;

        if (Physics.Raycast(topRayPos.position, Vector3.back, out hit, rayDistance) && !basket)
        {
            basket = true;
        }

        if (Physics.Raycast(bottomRayPos.position, Vector3.back, out hit, rayDistance) && basket)
        {
            if (hit.transform.TryGetComponent(out Softbody softbody))
            {
                basket = false;

                softbody.obiActor.AddForce(Vector3.down * force, ForceMode.Impulse);
                Debug.Log(softbody);

                transform.DOKill();

                rotTween = transform
                    .DOLocalRotate(new Vector3(-3f, 0, 0), angleDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutQuad)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => basket = true);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (topRayPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(topRayPos.position, Vector3.back * rayDistance);
        }

        if (bottomRayPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(bottomRayPos.position, Vector3.back * rayDistance);
        }
    }
}
