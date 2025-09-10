using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject playerPrefab; // inspector'da player prefabını ata
    private GameObject playerInstance; 
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform target; // player veya sahnede var olan objeyi referans alacak

    [Header("Crowd Settings")]
    public int crowdCount = 3;      // Başlangıçta 1 player + 2 runner
    public float spacing = 1.5f;

    void Start()
    {
        // Eğer target atanmadıysa, playerPrefab'ı instantiate et
        if (target == null && playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab);
            target = playerInstance.transform;
        }

        // Başlangıçta runner’ları oluştur
        UpdateRunners();
    }

    public void AddCrowd(int amount)
    {
        crowdCount = Mathf.Max(1, crowdCount + amount);
        UpdateRunners();
    }

    public void RemoveCrowd(int amount)
    {
        crowdCount = Mathf.Max(1, crowdCount - amount);
        UpdateRunners();
    }

    private void UpdateRunners()
    {
        if (runnerParent == null || runnerPrefab == null) return;

        // Önce mevcut runnerları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
        }

        // Player zaten sahnede -> Runner'ları oluştur
        for (int i = 0; i < crowdCount - 1; i++)
        {
            GameObject runnerObj = Instantiate(runnerPrefab, runnerParent);
            runnerObj.transform.localPosition = new Vector3((i % 5) * spacing, 0, -(i / 5) * spacing);
            runnerObj.SetActive(true);

            RunnerFollow followScript = runnerObj.GetComponent<RunnerFollow>();
            if (followScript != null)
                followScript.target = target;
        }
    }

    public Vector3 GetTargetPosition()
    {
        return target != null ? target.position : runnerParent.position;
    }

    public int GetCrowdCount()
    {
        return crowdCount;
    }
}
