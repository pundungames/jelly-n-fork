using UnityEngine;

public class JellyMove : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    
    [SerializeField] private float minDragDistance;
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody[] _rigs;
    [SerializeField] private SpringJoint[] springJoints;
    
    [SerializeField, Range(0,1f)] private float damping;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            SetSpringState(true);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            
            ResetAll();
            SetSpringState(false);
            
            Vector2 direction = startPos - endPos;
            if (direction.magnitude < minDragDistance)
                return;
           
            float force = Mathf.Clamp(direction.magnitude, 0, 1000f);
        
            rb.AddForce(direction.normalized * (-1 * force * 0.2f), ForceMode.Impulse);
        }
    }

    private void ResetAll()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    
        foreach (Rigidbody rig in _rigs)
        {
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
        }
    }

    public void SetSpringState(bool isSpring)
    {
        if (isSpring)
        {
            foreach (SpringJoint sj in springJoints)
            {
                sj.spring = 10000;
                sj.minDistance = 0f;
                sj.maxDistance = 0f;
            }
            return;
        }
        
        foreach (SpringJoint sj in springJoints)
        {
            sj.spring = 100;
            sj.minDistance = 0f;
            sj.maxDistance = 0f;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _rigs.Length; i++)
        {
            _rigs[i].velocity *= damping;
        }
    
        rb.velocity *= damping;
    }
}
