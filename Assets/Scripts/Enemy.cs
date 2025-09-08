using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyStrength = 5;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.crowdSystem != null)
        {
            player.crowdSystem.RemoveRunners(enemyStrength);
            Destroy(gameObject);
        }
    }
}
