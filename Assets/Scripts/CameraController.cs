using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mainCamera;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Camera _mainCam;
    private Transform _thisTransform;
    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private Vector3 _newZoom;

    private readonly Vector3 _up = Vector3.up;

    void Awake()
    {
        _mainCam = mainCamera.GetComponent<Camera>();
        _thisTransform = transform;
    }

    public void ChangePositionRotationZoom(Vector3 position, Quaternion rotation, Vector3 zoom)
    {
        _newPosition = position;
        _newRotation = rotation;
        _newZoom = zoom;
    }

    void Update()
    {
        HandleInput();
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += (_thisTransform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += (_thisTransform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += (_thisTransform.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += (_thisTransform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _newRotation *= Quaternion.Euler(_up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            _newRotation *= Quaternion.Euler(_up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            _newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            _newZoom -= zoomAmount;
        }

        _thisTransform.position = Vector3.Lerp(_thisTransform.position, _newPosition, Time.deltaTime * movementTime);
        _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _newRotation, Time.deltaTime * movementTime);
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, _newZoom, Time.deltaTime * movementTime);
    }
}
