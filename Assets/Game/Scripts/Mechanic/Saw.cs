using DG.Tweening;
using UnityEngine;
using Zenject;

public class Saw : MonoBehaviour
{
    [Inject] UIManager uiManager;
    
    [SerializeField] bool isMovement = false;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    public float moveDuration = 2f;
    public float rotateSpeed = 360f;

    void Start()
    {
        if (isMovement)
        {
            transform.position = startPos.position;
            transform.DOMove(endPos.position, moveDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        transform.DORotate(
                new Vector3(0, 0, 360),
                rotateSpeed,
                RotateMode.LocalAxisAdd
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Softbody softbody))
        {
            softbody.isMovement = false;
            transform.DOKill();
            EventManager.OnLevelFail();
        }
    }
}
