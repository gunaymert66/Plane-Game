using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputFlightController : MonoBehaviour
{
    [Header("Hassasiyet Ayarları")]
    public float pitchSpeed = 50f; // Yunuslama hızı
    public float rollSpeed = 50f;  // Yuvarlanma hızı
    public float yawSpeed = 30f;   // (YENİ) Sapma hızı (Genelde diğerlerinden biraz daha yavaş olur)

    [Header("Input Ayarı")]
    public InputActionReference lookAction; // Mouse Delta (Pitch/Roll)
    public InputActionReference yawAction;  // (YENİ) A ve D tuşları için

    private void OnEnable()
    {
        if (lookAction != null) lookAction.action.Enable();
        if (yawAction != null) yawAction.action.Enable(); // Yaw inputunu aktifleştir
    }

    private void OnDisable()
    {
        if (lookAction != null) lookAction.action.Disable();
        if (yawAction != null) yawAction.action.Disable(); // Yaw inputunu pasifleştir
    }

    void Update()
    {
        // --- MOUSE KONTROLLERİ (PITCH & ROLL) ---
        Vector2 mouseDelta = Vector2.zero;
        if (lookAction != null)
            mouseDelta = lookAction.action.ReadValue<Vector2>();

        // PITCH (Yunuslama) - Mouse Y
        float pitchAmount = mouseDelta.y * pitchSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * -pitchAmount);

        // ROLL (Yuvarlanma) - Mouse X
        float rollAmount = mouseDelta.x * rollSpeed * Time.deltaTime;
        transform.Rotate(Vector3.back * rollAmount);

        // --- KLAVYE KONTROLLERİ (YAW) ---
        // A ve D tuşlarından -1 ile 1 arasında değer okuruz
        float yawInput = 0f;
        if (yawAction != null)
            yawInput = yawAction.action.ReadValue<float>();

        // YAW (Sapma) - Klavye A/D
        // A'ya basınca -1 (Sola), D'ye basınca +1 (Sağa) döner
        float yawAmount = yawInput * yawSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * yawAmount);
    }
}