using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    void Update()
    {
        if (target == null) return;

        // Yönü hedefe çevir
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // sadece yatay dön
        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        // İleri hareket
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
