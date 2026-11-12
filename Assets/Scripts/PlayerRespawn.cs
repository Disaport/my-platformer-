using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 checkpoint;

    private void Awake() => checkpoint = transform.position;

    public void SetCheckpoint(Vector3 position) => checkpoint = position;

    public void Respawn()
    {
        transform.position = checkpoint;
        // Обов'язково: інакше персонаж з'явиться на точці з накопиченою
        // швидкістю падіння й одразу полетить униз
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}