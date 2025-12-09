using UnityEngine;
using UnityEngine.InputSystem;

public class AirplaneController : MonoBehaviour
{
    [Header("Input Referansları")]
    public InputActionReference flightInputRef; // WASD
    public InputActionReference yawInputRef;    // Q-E
    public InputActionReference throttleInputRef; // Space-Shift (YENİ)

    [Header("Hız Ayarları")]
    public float normalSpeed = 20f;
    public float boostSpeed = 45f;  // Space'e basınca çıkılacak hız
    public float brakeSpeed = 8f;   // Shift'e basınca düşülecek hız
    public float acceleration = 2f; // Hız değişiminin yumuşaklığı (İvme)

    [Header("Dönüş Ayarları")]
    public float pitchSpeed = 60f;
    public float rollSpeed = 45f;
    public float yawSpeed = 30f;

    private float activeSpeed; // O anki güncel hızımız

    private void OnEnable()
    {
        if (flightInputRef != null) flightInputRef.action.Enable();
        if (yawInputRef != null) yawInputRef.action.Enable();
        if (throttleInputRef != null) throttleInputRef.action.Enable(); // YENİ
    }

    private void OnDisable()
    {
        if (flightInputRef != null) flightInputRef.action.Disable();
        if (yawInputRef != null) yawInputRef.action.Disable();
        if (throttleInputRef != null) throttleInputRef.action.Disable(); // YENİ
    }

    void Start()
    {
        // Oyuna normal hızda başla
        activeSpeed = normalSpeed;
    }

    void Update()
    {
        HandleInput();
        MoveForward();
    }

    void HandleInput()
    {
        // 1. WASD ve Q-E verilerini oku (Eğer ref yoksa hata vermesin diye 0 alıyoruz)
        Vector2 flightInput = flightInputRef != null ? flightInputRef.action.ReadValue<Vector2>() : Vector2.zero;
        float yawInput = yawInputRef != null ? yawInputRef.action.ReadValue<float>() : 0f;

        // 2. Gaz verisini oku (Space: 1, Shift: -1, Hiçbiri: 0)
        float throttleInput = throttleInputRef != null ? throttleInputRef.action.ReadValue<float>() : 0f;

        // 3. Dönüşleri uygula
        HandleRotation(flightInput, yawInput);

        // 4. Hedef hızı belirle ve uygula
        HandleSpeed(throttleInput);
    }

    void MoveForward()
    {
        // Hesaplanmış "activeSpeed" ile ileri git
        transform.Translate(Vector3.forward * activeSpeed * Time.deltaTime);
    }

    void HandleSpeed(float throttleValue)
    {
        float targetSpeed = normalSpeed;

        if (throttleValue > 0)
        {
            targetSpeed = boostSpeed; // Space
        }
        else if (throttleValue < 0)
        {
            targetSpeed = brakeSpeed; // Shift
        }

        // Mevcut hızı hedef hıza yumuşak bir şekilde (Lerp) geçir
        activeSpeed = Mathf.Lerp(activeSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    void HandleRotation(Vector2 wasdInput, float qeInput)
    {
        float pitch = wasdInput.y * pitchSpeed * Time.deltaTime;
        float roll = -wasdInput.x * rollSpeed * Time.deltaTime;
        float yaw = qeInput * yawSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, roll, Space.Self);
    }
}