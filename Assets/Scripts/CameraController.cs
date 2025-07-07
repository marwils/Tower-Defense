using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveVelocity = 10f;

    [Header("Zoom")]
    [SerializeField]
    private float minPositionY;

    [SerializeField]
    private float maxPositionY;

    [SerializeField]
    private float minRotationX;

    [SerializeField]
    private float maxRotationX;

    [SerializeField]
    private float zoomVelocity = 10f;

    private float zoom = 0;

    void Update()
    {
        Vector2 direction = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) direction.y -= 1;
        if (Keyboard.current.sKey.isPressed) direction.y += 1;
        if (Keyboard.current.aKey.isPressed) direction.x += 1;
        if (Keyboard.current.dKey.isPressed) direction.x -= 1;

        direction *= moveVelocity;

        Vector2 scrollValue = Mouse.current.scroll.ReadValue();

        zoom += scrollValue.y * zoomVelocity * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, 0f, 1f);
        transform.position = new Vector3(transform.position.x + direction.x * Time.deltaTime, Mathf.Lerp(minPositionY, maxPositionY, zoom), transform.position.z + direction.y * Time.deltaTime);
        transform.localEulerAngles = new Vector3(Mathf.Lerp(minRotationX, maxRotationX, zoom), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
