using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - rb.position;
        direction.y = 0; // Sadece yatay düzlemde takip
        direction.Normalize();

        // Rigidbody ile hareket
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        // Rotasyonu da Rigidbody ile uygula
        if (direction != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(direction));
    }
}