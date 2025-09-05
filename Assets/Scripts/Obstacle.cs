using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RunnerFollow runner = other.GetComponent<RunnerFollow>();
        if (runner != null)
        {
            Destroy(runner.gameObject); // Tek tek runner kaybolur
        }
    }
}
