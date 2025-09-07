using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Çarpan: " + other.name);

        // GetComponent yerine GetComponentInParent kullanıyoruz
        RunnerFollow runner = other.GetComponentInParent<RunnerFollow>();
        if (runner != null)
        {
            Debug.Log("Runner bulundu, öldürülüyor: " + runner.name);

            // CrowdSystem'i sahneden bul
            CrowdSystem crowd = FindObjectOfType<CrowdSystem>();
            if (crowd != null)
            {
                // RemoveCrowd fonksiyonunu çağır
                crowd.RemoveCrowd(runner.gameObject);
            }
            else
            {
                Debug.LogWarning("CrowdSystem bulunamadı!");
            }
        }
        else
        {
            Debug.Log("RunnerFollow scripti bulunamadı! Bu yüzden ölmedi.");
        }
    }
}