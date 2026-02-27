using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float JumpPower;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out JellyMove player))
        {
            player.SetSpringState(true);
            player.GetComponent<Rigidbody>().AddForce(JumpPower * transform.up, ForceMode.Impulse);
            player.SetSpringState(false);
        }
    }
}
