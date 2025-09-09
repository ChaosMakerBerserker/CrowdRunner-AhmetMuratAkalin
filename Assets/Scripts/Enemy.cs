using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyStrength = 5; // Kaç kişiyi yok eder

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.crowdSystem != null)
        {
            player.crowdSystem.AddCrowd(-enemyStrength);
            Destroy(gameObject); // Düşman yok oldu
        }
    }
}