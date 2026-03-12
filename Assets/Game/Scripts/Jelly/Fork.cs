using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Fork : MonoBehaviour
{
    [Inject] private LevelManager _levelManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Softbody>(out _))
        {
            EventManager.OnLevelComplete();
        }
    }
}
