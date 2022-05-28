using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mainCamera;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    [HideInInspector] public Camera mainCam;
    private Transform _thisTransform;
    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private Vector3 _newZoom;
    private InputManager _inputManager;

    private readonly Vector3 _up = Vector3.up;

    void Awake()
    {
        mainCam = mainCamera.GetComponent<Camera>();
        _inputManager = Manager.instance.inputManager;
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
    }

    private void HandleInput()
    {
        if (Input.GetKey(_inputManager.moveForward))
        {
            _newPosition += (_thisTransform.forward * movementSpeed);
        }

        if (Input.GetKey(_inputManager.moveBackward))
        {
            _newPosition += (_thisTransform.forward * -movementSpeed);
        }

        if (Input.GetKey(_inputManager.moveRight))
        {
            _newPosition += (_thisTransform.right * movementSpeed);
        }

        if (Input.GetKey(_inputManager.moveLeft))
        {
            _newPosition += (_thisTransform.right * -movementSpeed);
        }

        if (Input.GetKey(_inputManager.rotateLeft))
        {
            _newRotation *= Quaternion.Euler(_up * rotationAmount);
        }

        if (Input.GetKey(_inputManager.rotateRight))
        {
            _newRotation *= Quaternion.Euler(_up * -rotationAmount);
        }

        if (Input.GetKey(_inputManager.zoomIn))
        {
            _newZoom += zoomAmount;
        }

        if (Input.GetKey(_inputManager.zoomOut))
        {
            _newZoom -= zoomAmount;
        }

        _thisTransform.position = Vector3.Lerp(_thisTransform.position, _newPosition, Time.deltaTime * movementTime);
        _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _newRotation, Time.deltaTime * movementTime);
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, _newZoom, Time.deltaTime * movementTime);
    }
}
