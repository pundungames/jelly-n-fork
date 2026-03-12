using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.Serialization;

[RequireComponent(typeof(ObiActor))]
public class AddRandomVelocity : MonoBehaviour {
	private Vector2 startPos;
	private Vector2 endPos;

	[SerializeField] private float intensity;
	[SerializeField, Range(0,1)] private float maxForce;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startPos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			endPos = Input.mousePosition;

			Vector2 direction = startPos - endPos;
			float force = direction.magnitude * maxForce;

			GetComponent<ObiActor>().AddForce(
				new Vector3(direction.normalized.x, direction.normalized.y, 0) * (-1 * force * intensity),
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
