using DG.Tweening;
using Obi;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObiSolver solver;
    
    [Header("Settings")]
    [SerializeField] private float duration;
    [SerializeField, Range(0,1)] private float scaleAmpunt;
    private bool isScale;
    
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
        if (isScale)
            return;
        
        Transform t = obiSolver.gameObject.transform.GetChild(0);
        Vector3 newScale = t.localScale * scaleAmpunt;
        
        for (int i = 0; i < contacts.contacts.Count; ++i)
        {
            var contact = contacts.contacts[i];

            if (contact.distance < 0)
            {
                var col = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;

                if (col != null && col.gameObject == this.gameObject)
                {
                    isScale = true;
                    
                    Transform obiSolverT = obiSolver.gameObject.transform;
                    obiSolver.gameObject.SetActive(false);
                    
                    t.gameObject.SetActive(false);
                    t.SetParent(null);
                    
                    t.gameObject.GetComponent<Softbody>().maxDistance = 0.2f;
                    
                    obiSolverT.DOScale(newScale, duration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            t.SetParent(obiSolverT);
                            t.gameObject.SetActive(true);
                        });
                    
                    
                }
            }
        }
    }
}
