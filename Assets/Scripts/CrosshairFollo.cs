using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    public Transform airplane;     // Uçağın transformu
    public Camera mainCamera;       // Main Camera
    public float forwardDistance = 200f; // Uçağın önünde ne kadar ileri bakalım

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (!mainCamera)
            mainCamera = Camera.main;
    }

    void Update()
    {
        // 1️⃣ Uçağın önünde bir nokta al
        Vector3 targetWorldPos = airplane.position + airplane.forward * forwardDistance;

        // 2️⃣ World → Screen
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetWorldPos);

        // 3️⃣ Kamera arkasındaysa gizle (önemli)
        if (screenPos.z < 0)
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }
        else
        {
            rectTransform.gameObject.SetActive(true);
        }

        // 4️⃣ UI pozisyonunu ayarla
        rectTransform.position = screenPos;
    }
}
