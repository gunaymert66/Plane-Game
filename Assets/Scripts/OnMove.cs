using UnityEngine;
using UnityEngine.InputSystem;

public class AirplaneController : MonoBehaviour
{
    [Header("Input Referansları")]
    public InputActionReference flightInputRef;    // WASD (Vector2)
    public InputActionReference yawInputRef;       // Q-E (Float)
    public InputActionReference throttleInputRef;  // Space (+1) / Shift (-1)

    [Header("Hız Ayarları")]
    public float normalSpeed = 20f;
    public float boostSpeed = 45f;
    public float brakeSpeed = 8f;
    public float acceleration = 2f;

    [Header("Dönüş Ayarları")]
    public float pitchSpeed = 60f;
    public float rollSpeed = 80f;
    public float yawSpeed = 35f;

    public float activeSpeed;

    private void OnEnable()
    {
        flightInputRef?.action.Enable();
        yawInputRef?.action.Enable();
        throttleInputRef?.action.Enable();
    }

    private void OnDisable()
    {
        flightInputRef?.action.Disable();
        yawInputRef?.action.Disable();
        throttleInputRef?.action.Disable();
    }

    void Start()
    {
        activeSpeed = normalSpeed;
    }

    void Update()
    {
        HandleInput();
        MoveForward();
    }

    void HandleInput()
    {
        Vector2 flightInput = flightInputRef != null
            ? flightInputRef.action.ReadValue<Vector2>()
            : Vector2.zero;

        float yawInput = yawInputRef != null
            ? yawInputRef.action.ReadValue<float>()
            : 0f;

        float throttleInput = throttleInputRef != null
            ? throttleInputRef.action.ReadValue<float>()
            : 0f;

        HandleRotation(flightInput, yawInput);
        HandleSpeed(throttleInput);
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * activeSpeed * Time.deltaTime, Space.Self);
    }

    void HandleSpeed(float throttle)
    {
        float targetSpeed = normalSpeed;

        if (throttle > 0f)
            targetSpeed = boostSpeed;
        else if (throttle < 0f)
            targetSpeed = brakeSpeed;

        activeSpeed = Mathf.Lerp(activeSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    void HandleRotation(Vector2 wasd, float qe)
    {
        float pitch = wasd.y * pitchSpeed * Time.deltaTime;
        float roll  = -wasd.x * rollSpeed * Time.deltaTime;
        float yaw   = qe * yawSpeed * Time.deltaTime;

        // Pitch → X
        transform.Rotate(Vector3.right, pitch, Space.Self);

        // Yaw → Y
        transform.Rotate(Vector3.up, yaw, Space.Self);

        // Roll → Z (kendi etrafında net dönüş)
        transform.Rotate(Vector3.forward, roll, Space.Self);
    }
}
