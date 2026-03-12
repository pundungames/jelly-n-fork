using Obi;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ObiActor))]
public class Softbody : MonoBehaviour
{
    [Header("Obi References")]
    public ObiActor obiActor;
    [SerializeField] private ObiSolver solver;
    [SerializeField] private ObiSoftbody softbody;

    [Header("Soft Body Movement Settings")]
    [Range(0,1)] public float maxDistance;
    [SerializeField] private float maxForceClamp = 20f;
    [SerializeField] private float _force;
    private Vector2 startPos;
    private Vector2 endPos;

    public bool isMovement = true;
    
    private void Awake()
    {
        obiActor = GetComponent<ObiActor>();
    }

    private void Update()
    {
        if (!isMovement)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;

            Vector2 direction = startPos - endPos;
            float force = direction.magnitude * maxDistance;
            
            force = Mathf.Clamp(force, 0f, maxForceClamp);
            
            obiActor.AddForce(
                new Vector3(direction.normalized.x, direction.normalized.y, 0) * (-force * _force),
                ForceMode.Impulse
            );
        }
    }
    

    // private void Update()
    // {
    // 	if (Input.touchCount > 0)
    // 	{
    // 		Touch touch = Input.GetTouch(0);
    //
    // 		if (touch.phase == TouchPhase.Began)
    // 		{
    // 			startPos = touch.position;
    // 		}
    //
    // 		if (touch.phase == TouchPhase.Ended)
    // 		{
    // 			endPos = touch.position;
    //
    // 			Vector2 direction = startPos - endPos;
    // 			float force = direction.magnitude * maxForce;
    //
    // 			GetComponent<ObiActor>().AddForce(
    // 				new Vector3(direction.normalized.x, direction.normalized.y, 0) * (-1 * force * intensity),
    // 				ForceMode.Impulse
    // 			);
    // 		}
    // 	}
    // }
}
