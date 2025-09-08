using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshPro rightDoorText;
    public TextMeshPro leftDoorText;

    [Header("References")]
    public CrowdSystem crowdSystem;
    public GameObject runnerPrefab;
    public Transform spawnPoint;

    [Header("Visuals")]
    public Renderer rightDoorRenderer;
    public Renderer leftDoorRenderer;
    public Color greenColor = Color.green;
    public Color redColor = Color.red;

    private BonusType rightDoorBonusType;
    private BonusType leftDoorBonusType;
    private int rightDoorBonusAmount;
    private int leftDoorBonusAmount;

    void Start()
    {
        RandomizeDoorBonuses();
        UpdateDoorTexts();
        UpdateDoorColors();
    }

    private void RandomizeDoorBonuses()
    {
        rightDoorBonusType = (BonusType)Random.Range(0, 4);
        leftDoorBonusType = (BonusType)Random.Range(0, 4);

        rightDoorBonusAmount = Random.Range(1, 11);
        leftDoorBonusAmount = Random.Range(1, 11);
    }

    private void UpdateDoorTexts()
    {
        if (rightDoorText != null)
        {
            rightDoorText.text = GetFormattedText(rightDoorBonusType, rightDoorBonusAmount);
            rightDoorText.color = (rightDoorBonusType == BonusType.Addition || rightDoorBonusType == BonusType.Multiplication)
                                  ? greenColor : redColor;
        }

        if (leftDoorText != null)
        {
            leftDoorText.text = GetFormattedText(leftDoorBonusType, leftDoorBonusAmount);
            leftDoorText.color = (leftDoorBonusType == BonusType.Addition || leftDoorBonusType == BonusType.Multiplication)
                                 ? greenColor : redColor;
        }
    }

    private string GetFormattedText(BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition: return $"+{amount}";
            case BonusType.Difference: return $"-{amount}";
            case BonusType.Multiplication: return $"x{amount}";
            case BonusType.Division: return $"/{amount}";
            default: return amount.ToString();
        }
    }

    private void UpdateDoorColors()
    {
        if (rightDoorRenderer != null)
        {
            Renderer[] rightRenderers = rightDoorRenderer.GetComponentsInChildren<Renderer>();
            foreach (var r in rightRenderers)
            {
                if (r != null && r.material != null)
                    r.material.color = (rightDoorBonusType == BonusType.Addition || rightDoorBonusType == BonusType.Multiplication) 
                                       ? greenColor : redColor;
            }
        }

        if (leftDoorRenderer != null)
        {
            Renderer[] leftRenderers = leftDoorRenderer.GetComponentsInChildren<Renderer>();
            foreach (var r in leftRenderers)
            {
                if (r != null && r.material != null)
                    r.material.color = (leftDoorBonusType == BonusType.Addition || leftDoorBonusType == BonusType.Multiplication) 
                                      ? greenColor : redColor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || crowdSystem == null || runnerPrefab == null || spawnPoint == null) return;

        float playerX = other.transform.position.x;
        float middleX = transform.position.x;

        if (playerX >= middleX) // Sağ kapı
        {
            ApplyBonus(rightDoorBonusType, rightDoorBonusAmount);
        }
        else // Sol kapı
        {
            ApplyBonus(leftDoorBonusType, leftDoorBonusAmount);
        }

        gameObject.SetActive(false); // Destroy yerine deactivate
    }

    private void ApplyBonus(BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition:
                for (int i = 0; i < amount; i++)
                {
                    GameObject newRunner = Instantiate(runnerPrefab, spawnPoint.position, Quaternion.identity);
                    if (crowdSystem != null && newRunner != null)
                    {
                        crowdSystem.AddCrowd(newRunner);
                        RunnerFollow follow = newRunner.GetComponent<RunnerFollow>();
                        if (follow != null)
                        {
                            int count = crowdSystem.GetCrowdCount();
                            follow.target = count <= 1 ? crowdSystem.leader : crowdSystem.runners[count - 2].transform;
                        }
                    }
                }
                break;

            case BonusType.Difference:
                int removeCount = Mathf.Min(amount, crowdSystem.GetCrowdCount());
                for (int i = 0; i < removeCount; i++)
                {
                    if (crowdSystem.GetCrowdCount() == 0) break;
                    GameObject lastRunner = crowdSystem.runners[crowdSystem.GetCrowdCount() - 1];
                    if (lastRunner != null)
                        crowdSystem.RemoveCrowd(lastRunner);
                }
                break;

            case BonusType.Multiplication:
                int current = crowdSystem.GetCrowdCount();
                for (int i = 0; i < current * (amount - 1); i++)
                {
                    GameObject newRunner = Instantiate(runnerPrefab, spawnPoint.position, Quaternion.identity);
                    if (crowdSystem != null && newRunner != null)
                    {
                        crowdSystem.AddCrowd(newRunner);
                        RunnerFollow follow = newRunner.GetComponent<RunnerFollow>();
                        if (follow != null && crowdSystem.GetCrowdCount() >= 2)
                            follow.target = crowdSystem.runners[crowdSystem.GetCrowdCount() - 2].transform;
                    }
                }
                break;

            case BonusType.Division:
                int cur = crowdSystem.GetCrowdCount();
                if (amount != 0)
                {
                    int toRemove = cur - cur / amount;
                    toRemove = Mathf.Min(toRemove, cur);
                    for (int i = 0; i < toRemove; i++)
                    {
                        if (crowdSystem.GetCrowdCount() == 0) break;
                        GameObject lastRunner = crowdSystem.runners[crowdSystem.GetCrowdCount() - 1];
                        if (lastRunner != null)
                            crowdSystem.RemoveCrowd(lastRunner);
                    }
                }
                break;
        }
    }
}
