using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform target;

    [Header("Crowd Settings")]
    public int crowdCount = 2;      // Başlangıçta 1 player + 1 runner
    public float spacing = 1.5f;

    void Start()
    {
        // Başlangıçta runner’ları oluştur
        UpdateRunners();
    }

    public void AddCrowd(int amount)
    {
        crowdCount = Mathf.Max(1, crowdCount + amount);
        UpdateRunners();
    }

    private void UpdateRunners()
    {
        if (runnerParent == null || runnerPrefab == null) return;

        // Önce var olan runner’ları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
        }

        // Yeni runner’ları oluştur
        for (int i = 0; i < crowdCount - 1; i++) // -1 çünkü player zaten var
        {
            GameObject runnerObj = Instantiate(runnerPrefab, runnerParent);
            runnerObj.transform.localPosition = new Vector3((i % 5) * spacing, 0, -(i / 5) * spacing);
            runnerObj.SetActive(true);

            // RunnerFollow scriptini al ve hedefi ata
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
