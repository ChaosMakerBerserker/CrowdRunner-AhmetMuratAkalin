using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public CrowdSystem crowdSystem; // Inspector'dan atayabilirsiniz

    private void OnTriggerEnter(Collider other)
    {
        // Runner tag kontrolü
        if (other.CompareTag("Runner"))
        {
            RunnerFollow runner = other.GetComponentInParent<RunnerFollow>();
            if (runner != null)
            {
                if (crowdSystem != null)
                    crowdSystem.RemoveCrowd(runner.gameObject);
                else
                    Debug.LogWarning("CrowdSystem referansı atanmadı!");
            }
        }
    }
}