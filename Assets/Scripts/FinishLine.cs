using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("🏆 Kazandın! Kalabalık sayısı: " + player.crowdSystem.GetCrowdCount());
            // Burada UI veya sonraki level açabilirsin
        }
    }
}
