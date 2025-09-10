using UnityEngine;

public class RunnerFollow : MonoBehaviour
{
    public GameManager gameManager; // GameManager referansı
    public Transform target;
    public float speed = 5f;

    void Update()
    {
        if (gameManager != null && gameManager.gameEnded) return;

        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

}