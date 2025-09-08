using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [Header("References")]
    public Doors doorPrefab;
    public Transform spawnPoint;
    public float spawnDistanceZ = 10f;
    public float doorSpacing = 5f;

    private void Start()
    {
        SpawnDoor();
    }

    public void SpawnDoor()
    {
        if (doorPrefab == null || spawnPoint == null) return;

        Doors newDoor = Instantiate(doorPrefab, spawnPoint.position, Quaternion.identity);

        // Kapıyı randomize et
        newDoor.RandomizeDoor();

        // Kapının pozisyonunu biraz ileri kaydır
        Vector3 pos = spawnPoint.position;
        pos.z += spawnDistanceZ;
        spawnPoint.position = pos;
    }

    // Eğer sürekli spawn istiyorsan bunu Update veya Coroutine ile yapabilirsin
}
