using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f;
    public float lifeTime = 3f;

    private float inheritedSpeed;

    public void Init(float planeSpeed)
    {
        inheritedSpeed = planeSpeed;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * (speed + inheritedSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        Destroy(gameObject);
    }
}
