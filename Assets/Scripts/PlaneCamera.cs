using UnityEngine;

public class PlaneCamera : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    public Transform target; // Takip edilecek uçak (Player)

    [Header("Konum Ayarları")]
    public Vector3 offset = new Vector3(0, 4f, -12f); // Uçağın ne kadar arkasında ve yukarısında?
    public float followSpeed = 10f; // Kameranın uçağa yetişme hızı (Düşük = Daha ağır/sinematik)

    [Header("Dönüş Ayarları")]
    public float rotationSpeed = 5f; // Kameranın dönüş hızı

    void FixedUpdate()
    {
        if (target == null) return;

        // --- 1. Pozisyon Takibi (Yumuşak) ---
        // Uçağın arkasındaki olması gereken hedef noktayı buluyoruz (Yerel koordinattan dünyaya çevirir)
        Vector3 desiredPosition = target.TransformPoint(offset);

        // O noktaya yumuşakça kayıyoruz (Lerp)
        // Time.deltaTime yerine fixedDeltaTime kullanıyoruz çünkü uçak FixedUpdate ile uçuyor
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        // --- 2. Dönüş Takibi (Yumuşak) ---
        // Kameranın bakması gereken yön: Uçağın baktığı yön + Uçağın yukarısı
        Quaternion desiredRotation = Quaternion.LookRotation(target.forward, target.up);

        // O açıya yumuşakça dönüyoruz (Slerp)
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothedRotation;
    }
}