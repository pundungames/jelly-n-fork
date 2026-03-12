using System;
using DG.Tweening;
using UnityEngine;
using Obi;


public class FanForce : MonoBehaviour
{
    [Header("Fan Ray Settings")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float force;
    [SerializeField] private Vector3[] rayPositions;

    [Header("Visual Animation Settings")]
    [SerializeField] private Transform fanVisual;
    public Vector3 rotateAmount = new Vector3(0, 90, 0);
    public float duration = 1f;

    void Start()
    {
        fanVisual
            .DOLocalRotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
    
    void FixedUpdate()
    {
        for (int i = 0; i < rayPositions.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(rayPositions[i]);

            Vector3 worldDir = transform.TransformDirection(Vector3.up);

            if (Physics.Raycast(worldPos, worldDir, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.TryGetComponent(out Softbody softbody))
                {
                    softbody.obiActor.AddForce(force * worldDir, ForceMode.Force);
                }
            }
        }
    }
    
    void OnDrawGizmos()
    {
        for (int i = 0; i < rayPositions.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(rayPositions[i]);
            Vector3 worldDir = transform.TransformDirection(Vector3.up);

            Debug.DrawRay(worldPos, worldDir * rayDistance, Color.cyan);
        }
    }
}