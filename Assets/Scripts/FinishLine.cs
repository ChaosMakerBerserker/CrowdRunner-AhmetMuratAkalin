using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Level bitti!");

            // Direkt 2. seviyeye geç
            SceneManager.LoadScene("Level2");
            Time.timeScale = 1f; // garanti olsun diye ekle
        }
    }
}