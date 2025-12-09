using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    [Header("Hedefler")]
    public Transform target;           // Uçak Transformu
    public AirplaneController airplaneScript; // Uçağın hızına erişmek için scripti

    [Header("Mesafe ve Yükseklik")]
    public float height = 4f;          // Uçağın ne kadar üstünde?
    public float distance = 12f;       // Uçağın ne kadar arkasında? (Normal hızda)
    public float boostDistanceMultiplier = 1.5f; // Hızlanınca mesafe kaç katına çıksın?

    [Header("Takip Ayarları")]
    public float positionDamping = 0.2f; // Konum gecikmesi (Yüksek = Daha geç gelir)
    public float rotationDamping = 5f;   // Dönüş yumuşaklığı
    public float lookAheadAmount = 10f;  // Kamera uçağın ne kadar önüne baksın? (Nişan yardımı)

    [Header("Hız Efekti (FOV)")]
    public float defaultFOV = 60f;     // Normal görüş açısı
    public float boostFOV = 80f;       // Hızlanınca görüş açısı

    private Vector3 _currentVelocity;  // SmoothDamp için gerekli geçici değişken

    void LateUpdate()
    {
        if (!target || !airplaneScript) return;

        HandleCameraPosition();
        HandleCameraRotation();
        HandleFOV();
    }

    void HandleCameraPosition()
    {
        // 1. Hız Oranını Hesapla (0 ile 1 arasında bir değer)
        // Eğer uçak boost hızındaysa 1, normal hızdaysa 0'a yakın olur.
        float speedRatio = (airplaneScript.activeSpeed - airplaneScript.normalSpeed) /
                           (airplaneScript.boostSpeed - airplaneScript.normalSpeed);

        // Değeri 0-1 arasına sıkıştır (Hata olmasın)
        speedRatio = Mathf.Clamp01(speedRatio);

        // 2. Hedef Mesafeyi Belirle
        // Hızlıysak daha geride dur, yavaşsak normal mesafede.
        float currentDistance = Mathf.Lerp(distance, distance * boostDistanceMultiplier, speedRatio);

        // 3. Hedef Pozisyonu Hesapla
        // Uçağın arkasında ve yukarısında bir nokta buluyoruz.
        // target.back * currentDistance -> Uçağın arkası
        // target.up * height -> Uçağın yukarısı
        Vector3 targetPosition = target.position - (target.forward * currentDistance) + (target.up * height);

        // 4. SmoothDamp ile Oraya Git (Lerp'ten daha akıllı ve fizikseldir)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, positionDamping);
    }

    void HandleCameraRotation()
    {
        // Kameranın bakacağı yer: Uçağın kendisi değil, uçağın burnunun biraz ilerisi.
        // Bu sayede uçak dönerken kamera "gitmek istediğin yeri" gösterir.
        Vector3 lookPoint = target.position + (target.forward * lookAheadAmount);

        // Hedef rotasyonu bul
        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - transform.position);

        // Yumuşakça dön
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamping * Time.deltaTime);
    }

    void HandleFOV()
    {
        // Hız oranını tekrar hesapla (Performans için yukarıdaki değişkene de taşınabilir)
        float speedRatio = (airplaneScript.activeSpeed - airplaneScript.normalSpeed) /
                           (airplaneScript.boostSpeed - airplaneScript.normalSpeed);
        speedRatio = Mathf.Clamp01(speedRatio);

        // FOV'u değiştir (Hızlanınca tünel etkisi)
        Camera.main.fieldOfView = Mathf.Lerp(defaultFOV, boostFOV, speedRatio);
    }
}