using System;
using DG.Tweening;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{
    public Vector3 child;

    public float duration = .2f;
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 childWorldPos;

    
    bool isTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Softbody softbody))
        {
            if (isTrigger)
                return;
            
            isTrigger = true;
            GameObject softbodyObj = softbody.gameObject;
            
            softbodyObj.transform.parent.gameObject.SetActive(false);
            child = softbodyObj.transform.position;
            
            softbodyObj.transform.parent.DOScale(targetScale, duration)
                .OnComplete(() =>
                {
                    softbodyObj.transform.position = child;
                    softbodyObj.transform.parent.gameObject.SetActive(true);
                });
            
        }
    }
}
