using System;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] float JumpPower;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out JellyMove player))
        {
            player.SetSpringState(true);
            player.GetComponent<Rigidbody>().AddForce(JumpPower * -transform.up, ForceMode.Impulse);
            player.SetSpringState(false);
        }
    }
}
