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
            return;
        }
    

       // Eğer Runner çarparsa
        if (other.CompareTag("Runner"))
        {
           CrowdSystem crowd = Object.FindFirstObjectByType<CrowdSystem>();
            if (crowd != null)
            {
                crowd.AddCrowd(-1); // 1 runner eksilt
            }

            Destroy(other.gameObject); // Runner yok olur
            Destroy(gameObject);       // Enemy de yok olur
        }
    }


}