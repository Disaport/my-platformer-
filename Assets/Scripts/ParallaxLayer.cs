using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = шар стоїть нерухомо, 1 = рухається разом із камерою")]
    [SerializeField, Range(0f, 1.5f)] private float parallaxFactor = 0.5f;

    private Transform cam;
    private Vector3 startPosition;
    private Vector3 camStartPosition;

    private void Start()
    {
        cam = Camera.main.transform;
        startPosition = transform.position;
        camStartPosition = cam.position;
    }

    // LateUpdate: камера вже перемістилася цього кадру — тепер підлаштовуємо фон
    private void LateUpdate()
    {
        Vector3 camDelta = cam.position - camStartPosition;
        transform.position = startPosition + camDelta * parallaxFactor;
    }
}
