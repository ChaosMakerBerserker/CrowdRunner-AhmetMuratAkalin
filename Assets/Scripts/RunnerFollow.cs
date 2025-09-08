using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    void Update()
    {
        if (GameManager.gameEnded) return;
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
        }
    }
}
