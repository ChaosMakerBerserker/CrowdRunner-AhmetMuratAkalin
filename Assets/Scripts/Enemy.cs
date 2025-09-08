using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyStrength = 5; // Kaç runner yok edecek

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.crowdSystem != null)
        {
            CrowdSystem cs = player.crowdSystem;

            // Son enemyStrength kadar runner'ı kaldır
            int removeCount = Mathf.Min(enemyStrength, cs.GetCrowdCount());
            for (int i = 0; i < removeCount; i++)
            {
                GameObject lastRunner = cs.runners[cs.GetCrowdCount() - 1];
                cs.RemoveCrowd(lastRunner);
            }

            Destroy(gameObject); // Düşman yok oldu
        }
    }
}