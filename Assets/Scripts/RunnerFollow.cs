using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody ayarları
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - rb.position;
        direction.y = 0;
        direction.Normalize();

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(direction));
    }
}