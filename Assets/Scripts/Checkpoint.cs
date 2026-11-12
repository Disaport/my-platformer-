using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Не перевіряємо тег — перевіряємо наявність компонента.
        // Так точка працює з будь-ким, у кого є респавн
        if (other.TryGetComponent<PlayerRespawn>(out var respawn))
            respawn.SetCheckpoint(transform.position);
    }
}