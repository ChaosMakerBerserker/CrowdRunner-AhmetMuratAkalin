using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int obstacleStrength = 3;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.crowdSystem != null)
        {
            player.crowdSystem.RemoveRunners(obstacleStrength);
        }
    }
}
