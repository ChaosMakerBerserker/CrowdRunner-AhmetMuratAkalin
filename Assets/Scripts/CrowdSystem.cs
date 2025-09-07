using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform target;

    [Header("Crowd Settings")]
    public int crowdCount = 2;
    public float spacing = 1.5f;

    void Start()
    {
        UpdateRunners();
    }

    public void AddCrowd(int amount)
    {
        crowdCount = Mathf.Max(1, crowdCount + amount);
        UpdateRunners();
    }

    public void RemoveCrowd(GameObject runner)
    {
        if (runnerParent == null || runner == null) return;

        Destroy(runner);
        crowdCount = Mathf.Max(1, crowdCount - 1);
    }

    private void UpdateRunners()
    {
        if (runnerParent == null || runnerPrefab == null) return;

        // Parent altını temizle
        for (int i = runnerParent.childCount - 1; i >= 0; i--)
        {
            Destroy(runnerParent.GetChild(i).gameObject);
        }

        // Runnerları oluştur
        for (int i = 0; i < crowdCount - 1; i++)
        {
            GameObject runner = Instantiate(runnerPrefab, runnerParent);
            runner.transform.localPosition = new Vector3((i % 5) * spacing, 0, -(i / 5) * spacing);

            RunnerFollow follow = runner.GetComponent<RunnerFollow>();
            if (follow != null)
                follow.target = target;
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