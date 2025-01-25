using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxEffect;
        lastCameraPosition = cameraTransform.position;
    }
}