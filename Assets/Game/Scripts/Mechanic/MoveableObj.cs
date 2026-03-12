using DG.Tweening;
using Obi;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Rigidbody))]
public class MoveableObj : MonoBehaviour
{
    [SerializeField] private ObiSolver solver;
    [SerializeField] private Transform targetPos;

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        
        if (targetPos.position != Vector3.zero && Vector3.Distance(transform.position, targetPos.position) >= 1.1f)
        {
            transform.DOKill();
        }
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    bool isActive = true;
    private void Solver_OnCollision(ObiSolver obiSolver, ObiSolver.ObiCollisionEventArgs contacts)
    {
        for (int i = 0; i < contacts.contacts.Count; ++i)
        {
            var contact = contacts.contacts[i];

            if (contact.distance < 0)
            {
                var col = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
                if (col != null && col.gameObject == this.gameObject)
                {
                    if (Vector3.Distance(transform.position, targetPos.position) < 1.1f && isActive)
                    {
                        transform.DOMove(targetPos.position, 0.3f).OnComplete(() => isActive = !isActive);
                    }
                }
            }
        }
    }
}
