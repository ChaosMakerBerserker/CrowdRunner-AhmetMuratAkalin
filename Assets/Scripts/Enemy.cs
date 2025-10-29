using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform player; // Player objesi (Inspector'da ata)
    public float chaseSpeed = 3f; // Takip hızı
    public float detectionRange = 10f; // Algılama mesafesi

    [Header("Death")]
    public GameObject deathEffect; // Ölüm efekti (opsiyonel, particle prefab)

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Enemy: NavMesh Agent eksik! Add Component > Navigation > NavMesh Agent ekle.");
            enabled = false; // Script'i devre dışı bırak
            return;
        }
        agent.speed = chaseSpeed;
        agent.areaMask = 1; // Walkable alan

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Otomatik bul
        }

        // Agent'i NavMesh'e yerleştir (ana hata çözümü)
        PlaceOnNavMesh();
    }

    void PlaceOnNavMesh()
    {
        if (agent == null) return; // Null kontrolü (satır 72 hatası için)

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); // NavMesh'e warp et
            transform.position = hit.position; // Pozisyonu güncelle
            Debug.Log("Enemy: NavMesh'e başarıyla yerleştirildi. Pozisyon: " + hit.position);
        }
        else
        {
            Debug.LogError("Enemy: NavMesh bake hatası! NavMesh Surface'te Bake et veya pozisyonu değiştir.");
            // Fallback: Pozisyonu varsayılan NavMesh'e ayarla
            transform.position = new Vector3(0, 0, 0); // Sahne başlangıcı
            // Tekrar dene
            NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
            if (hit.hit) agent.Warp(hit.position);
        }
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position); // Takip et
        }
        else
        {
            agent.ResetPath(); // Dur
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Runner"))
        {
            Debug.Log("Enemy: Runner ile çarpıştı, her ikisi de yok ediliyor!");

            // Runner'ı yok et
            Destroy(collision.gameObject);

            // Ölüm efekti (opsiyonel)
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);

            // Düşmanı yok et
            Destroy(gameObject);
        }
    }
}