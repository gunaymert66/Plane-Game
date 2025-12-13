using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public AirplaneController airplane;
    public Transform[] muzzles;
    public GameObject bulletPrefab;

    [Header("Weapon Settings")]
    public float fireRate = 0.1f;

    private float nextFireTime;
    private int muzzleIndex;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            TryFire();
        }
    }

    void TryFire()
{
    if (Time.time < nextFireTime) return;
    nextFireTime = Time.time + fireRate;

    foreach (Transform muzzle in muzzles)
    {
        GameObject bulletGO =
            Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Init(airplane.activeSpeed);
    }
}

}
