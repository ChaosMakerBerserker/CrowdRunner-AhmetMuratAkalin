using UnityEngine;
using System.Collections.Generic;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform leader;

    [Header("Crowd Settings")]
    public int crowdCount = 10;
    public float spacing = 0.8f;
    public float goldenAngle = 137.508f;

    [Header("Map Bounds")]
    public Vector2 xBounds = new Vector2(-50, 50);
    public Vector2 zBounds = new Vector2(-50, 50);

    public List<GameObject> runners = new List<GameObject>();

    void Start()
    {
        SpawnRunners();
    }

    void SpawnRunners()
    {
        for (int i = 0; i < crowdCount; i++)
        {
            float angle = i * goldenAngle * Mathf.Deg2Rad;
            float radius = Mathf.Sqrt(i) * spacing;

            Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            pos += runnerParent.position;

            pos.x = Mathf.Clamp(pos.x, xBounds.x, xBounds.y);
            pos.z = Mathf.Clamp(pos.z, zBounds.x, zBounds.y);

            GameObject runner = Instantiate(runnerPrefab, pos, Quaternion.identity, runnerParent);
            runners.Add(runner);

            RunnerFollow follow = runner.GetComponent<RunnerFollow>();
            if (follow != null)
            {
                follow.target = i == 0 ? leader : runners[i - 1].transform;
            }
        }
    }

    // -----------------------
    // Önceki scriptlerin kullandığı metotlar
    // -----------------------

    public void AddCrowd(GameObject newRunner)
    {
        runners.Add(newRunner);
        newRunner.transform.parent = runnerParent;
    }

    public void RemoveCrowd(GameObject runner)
    {
        if (runners.Contains(runner))
        {
            runners.Remove(runner);
            Destroy(runner);
        }
    }

    public int GetCrowdCount()
    {
        return runners.Count;
    }

    public Vector3 GetTargetPosition(int index)
    {
        if (index < 0 || index >= runners.Count) return leader.position;
        return runners[index].transform.position;
    }
}