using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Player’a çarpıldı mı?
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("OYUN BİTTİ! KAZANDIN 🎉");

            // GameManager üzerinden oyunu bitir
            GameManager.EndGame();
        }
    }
}