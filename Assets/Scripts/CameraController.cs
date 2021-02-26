using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float rotationSpeed = .2f;
    [SerializeField] private float maxZoom = 3f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private MyControls controls;

    private void Awake()
    {
        controls = new MyControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void LateUpdate()
    {   
        if(controls.Player.CanRotate.ReadValue<float>() == 1f)
        {
            offset = Quaternion.AngleAxis(controls.Player.Rotate.ReadValue<float>() * rotationSpeed, Vector3.up) * offset;
        }

        offset.y += controls.Player.Zoom.ReadValue<Vector2>().y * -.01f;
        offset.y = Mathf.Clamp(offset.y, maxZoom, minZoom);
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target.position);
    }
}