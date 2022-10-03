using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Axes")]
    public string HorizontalAxis = "Horizontal";
    public string VerticalAxis = "Vertical";
    public string MouseXAxis = "Mouse X";
    public string MouseYAxis = "Mouse Y";

    [Header("Input Buttons")]
    public string JumpButton = "Jump";

    [Header("Handling")]
    [Range(1.0f, 10.0f)] public float Speed = 5.0f;
    [Range(0.0f, 5.0f)] public float JumpHeight = 3.0f;

	private PlayerMotor _motor;

    public void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    public void Update()
    {
        _motor.DoMouseLook(Time.timeScale * new Vector2(Input.GetAxisRaw(MouseXAxis) * OptionsManager.MouseXSensitivity, Input.GetAxisRaw(MouseYAxis) * OptionsManager.MouseYSensitivity));
        if (!string.IsNullOrWhiteSpace(JumpButton) && Input.GetButtonDown(JumpButton)) _motor.Jump(JumpHeight);
    }
    public void FixedUpdate()
    {
        Vector3 move = new Vector3(Input.GetAxis(HorizontalAxis), 0, Input.GetAxis(VerticalAxis)).normalized;
        if (move.sqrMagnitude > 0)
        {
            move *= Speed;
            _motor.SetTargetVelocity(move);
        }
        else _motor.SetTargetVelocity(Vector3.zero);
    }
}