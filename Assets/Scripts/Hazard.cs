using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerRespawn>(out var respawn))
            respawn.Respawn();
    }
}