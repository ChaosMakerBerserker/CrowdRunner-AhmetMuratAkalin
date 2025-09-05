using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshPro rightDoorText;
    public TextMeshPro leftDoorText;

    [Header("Settings")]
    public BonusType rightDoorBonusType = BonusType.Addition;
    public BonusType leftDoorBonusType = BonusType.Addition;
    public int rightDoorBonusAmount = 10;
    public int leftDoorBonusAmount = 2;

    void Start()
    {
        UpdateDoorTexts();
    }

    private void UpdateDoorTexts()
    {
        if (rightDoorText != null)
            rightDoorText.text = GetFormattedText(rightDoorBonusType, rightDoorBonusAmount);

        if (leftDoorText != null)
            leftDoorText.text = GetFormattedText(leftDoorBonusType, leftDoorBonusAmount);
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

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || player.crowdSystem == null) return;

        float playerX = other.transform.position.x;
        float middleX = transform.position.x;

        if (playerX >= middleX) // Sağ kapı
        {
            ApplyBonus(player, rightDoorBonusType, rightDoorBonusAmount);
            Debug.Log("Sağ kapı seçildi!");
        }
        else // Sol kapı
        {
            ApplyBonus(player, leftDoorBonusType, leftDoorBonusAmount);
            Debug.Log("Sol kapı seçildi!");
        }
    }

    private void ApplyBonus(PlayerController player, BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition:
                player.crowdSystem.AddCrowd(amount);
                break;

            case BonusType.Difference:
                player.crowdSystem.AddCrowd(-amount);
                break;

            case BonusType.Multiplication:
                int current = player.crowdSystem.GetCrowdCount();
                player.crowdSystem.AddCrowd(current * (amount - 1));
                break;

            case BonusType.Division:
                int cur = player.crowdSystem.GetCrowdCount();
                if (amount != 0)
                    player.crowdSystem.AddCrowd(-(cur - cur / amount));
                break;
        }
    }
}
