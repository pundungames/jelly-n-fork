using DG.Tweening;
using Obi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirePlace : MonoBehaviour
{
    public ObiSolver solver;
    public ObiSoftbody softbody;
    
    [SerializeField, Range(0, 1)] private float deformeAmount;
    [SerializeField] private float duration;
    
    private bool deformed;

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }
    
    private void Solver_OnCollision(ObiSolver obiSolver, ObiSolver.ObiCollisionEventArgs contacts)
    {
        if(deformed)
            return;
        
        for (int i = 0; i < contacts.contacts.Count; ++i)
        {
            var contact = contacts.contacts[i];

            if (contact.distance < 0)
            {
                var col = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
                
                if (col != null && col.gameObject == this.gameObject)
                {
                    deformed = true;

                    float targetValue = softbody.deformationResistance * deformeAmount;
                    
                    softbody.gameObject.GetComponent<Softbody>().isMovement = false;
                    DOVirtual.DelayedCall(0.6f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
                    
                    DOTween.To(
                        () => softbody.deformationResistance,
                        x => softbody.deformationResistance = x,
                        targetValue,
                        duration
                    ).SetEase(Ease.Linear)
                    .OnComplete(()=>EventManager.OnLevelFail());
                }
            }
        }
    }
}
