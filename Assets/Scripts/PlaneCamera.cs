using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    [Header("Hedefler")]
    public Transform target;                 // AirplaneRoot
    public AirplaneController airplaneScript;

    [Header("Mesafe ve Yükseklik")]
    public float height = 4f;
    public float distance = 12f;
    public float boostDistanceMultiplier = 1.5f;

    [Header("Takip Ayarları")]
    public float positionDamping = 0.2f;
    public float rotationDamping = 5f;
    public float lookAheadAmount = 10f;

    [Header("Roll Takibi")]
    public float rollFollowStrength = 3f;   // Kameranın roll’u ne kadar takip etsin

    [Header("Hız Efekti (FOV)")]
    public float defaultFOV = 60f;
    public float boostFOV = 80f;

    private Vector3 _currentVelocity;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!target || !airplaneScript) return;

        HandleCameraPosition();
        HandleCameraRotation();
        HandleFOV();
    }

    void HandleCameraPosition()
    {
        float speedRatio =
            (airplaneScript.activeSpeed - airplaneScript.normalSpeed) /
            (airplaneScript.boostSpeed - airplaneScript.normalSpeed);

        speedRatio = Mathf.Clamp01(speedRatio);

        float currentDistance =
            Mathf.Lerp(distance, distance * boostDistanceMultiplier, speedRatio);

        Vector3 targetPosition =
            target.position
            - target.forward * currentDistance
            + target.up * height;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            positionDamping
        );
    }

    void HandleCameraRotation()
    {
        // 1. Kameranın bakacağı nokta
        Vector3 lookPoint = target.position + target.forward * lookAheadAmount;

        // 2. Uçağın rotasyonunu baz al
        Quaternion baseRotation =
            Quaternion.LookRotation(lookPoint - transform.position, target.up);

        // 3. Roll’u da takip et (uçakla beraber yat)
        Quaternion rollRotation =
            Quaternion.AngleAxis(target.eulerAngles.z, Vector3.forward);

        Quaternion finalRotation =
            Quaternion.Slerp(
                transform.rotation,
                baseRotation,
                rotationDamping * Time.deltaTime
            );

        transform.rotation = finalRotation;
    }

    void HandleFOV()
    {
        float speedRatio =
            (airplaneScript.activeSpeed - airplaneScript.normalSpeed) /
            (airplaneScript.boostSpeed - airplaneScript.normalSpeed);

        speedRatio = Mathf.Clamp01(speedRatio);

        _cam.fieldOfView =
            Mathf.Lerp(defaultFOV, boostFOV, speedRatio);
    }
}
