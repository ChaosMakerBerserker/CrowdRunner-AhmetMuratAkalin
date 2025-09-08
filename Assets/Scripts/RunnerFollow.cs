using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -1f); // Hafif arka pozisyon
    public float followSpeed = 5f;
    public float minDistance = 1f;

    [Header("Map Bounds")]
    public Vector2 xBounds = new Vector2(-50, 50);
    public Vector2 zBounds = new Vector2(-50, 50);

    void Update()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        float distance = Vector3.Distance(transform.position, target.position);

        // Minimum mesafe kontrolü
        if (distance > minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followSpeed);
        }

        // Harita sınırları
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xBounds.x, xBounds.y);
        pos.z = Mathf.Clamp(pos.z, zBounds.x, zBounds.y);
        transform.position = pos;
    }
}
