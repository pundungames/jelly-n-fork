using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Softbody softbody))
        {
            softbody.isMovement = false;
            EventManager.OnLevelFail();
        }
    }
}
