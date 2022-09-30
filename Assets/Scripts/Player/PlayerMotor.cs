using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    #region Serialized Fields
    [Header("Rotation Settings")]
    public Vector2 CameraRotationClamp = new(360, 180);
    public Vector2 MouseSmoothing = new(3, 3);
    public bool LockCursor;

    [Header("Movement Settings")]
    [Range(0.0f, 1.0f)] public float AirControl = 0.5f;
    public float BoostWindow = 0.25f;
    public LayerMask GroundMask;

    [Header("Components")]
    public Transform Body;
    public Transform Camera;
    #endregion
    #region Private Fields
    private Rigidbody _rigidbody;
    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;
    private Vector2 _targetRotation;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    #endregion
    #region Read-Only Properties
    public bool OnGround { get; private set; }
    public bool Moving { get; private set; }
    public UnityEvent OnJump = new();
    public UnityEvent OnLand = new();
    #endregion

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _targetRotation = transform.rotation.eulerAngles;
        OnGround = true;

#if UNITY_EDITOR
        LockCursor = false;
#endif
        Cursor.lockState = LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !LockCursor;
    }
    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) LockCursor = false;
        if (Input.GetMouseButtonDown(0)) LockCursor = true;
#endif

        if (OnGround) OnGround = Physics.OverlapSphere(transform.position - transform.localScale.y * Vector3.up, 0.1f, GroundMask).Length > 0;
        else
        {
            OnGround = _rigidbody.velocity.y <= 0 && Physics.OverlapSphere(transform.position - transform.localScale.y * Vector3.up, 0.1f, GroundMask).Length > 0;
            if (OnGround) OnLand.Invoke();
        }
    }

    public void FixedUpdate()
    {
        _movementVelocity = _targetMovementVelocity;

        Vector3 move = _rigidbody.position + (Vector3)(Body.localToWorldMatrix * (_movementVelocity * Time.fixedDeltaTime));
        _rigidbody.MovePosition(move);
    }

    public void DoMouseLook(Vector2 mouseInput)
    {
        Cursor.lockState = LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !LockCursor;

        if (LockCursor)
        {
            Quaternion targetOrientation = Quaternion.Euler(_targetRotation);

            var mouseDelta = mouseInput;
            mouseDelta = Vector2.Scale(mouseDelta, MouseSmoothing);

            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / MouseSmoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / MouseSmoothing.y);
            _mouseAbsolute += _smoothMouse;

            if (CameraRotationClamp.x < 360) _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -CameraRotationClamp.x * 0.5f, CameraRotationClamp.x * 0.5f);

            var pitch = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
            Camera.localRotation = pitch;
            Body.localRotation = Quaternion.identity;

            if (CameraRotationClamp.y < 360) _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -CameraRotationClamp.y * 0.5f, CameraRotationClamp.y * 0.5f);

            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            Body.localRotation *= yRotation;
            Body.rotation *= targetOrientation;
        }
    }
    public void Jump(float jumpHeight)
    {
        if (OnGround)
        {
            _rigidbody.MovePosition(_rigidbody.position + Vector3.up * 0.05f);
            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            OnJump.Invoke();
        }
    }
    public void SetTargetVelocity(Vector3 targetVelocity)
    {
        _targetMovementVelocity = targetVelocity;
        Moving = _targetMovementVelocity.sqrMagnitude > 0;
    }
}