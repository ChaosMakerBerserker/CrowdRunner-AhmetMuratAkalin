using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour 
{
    [Header("Elements")]

    public DoorType doorType;

    public Color additionColor = Color.green;
    public Color differenceColor = Color.red;
    public Color multiplicationColor = Color.blue;
    public Color divisionColor = Color.yellow;

    public SpriteRenderer RightdoorSprite;
    public SpriteRenderer LeftdoorSprite;
    public TextMeshPro rightDoorText;
    public TextMeshPro leftDoorText;

    [Header("Settings")]
    public BonusType rightDoorBonusType = BonusType.Addition;
    public BonusType leftDoorBonusType = BonusType.Addition;
    public int rightDoorBonusAmount = 10;
    public int leftDoorBonusAmount = 2;

    void Awake()
    {
        
    }

    void Start()
    {
        SetRandoms();
    }


    public void SetRandoms()
    {
        SetRandomBonuses();
        UpdateDoorTexts();
        UpdateDoorColors();
    }
    void SetRandomBonuses()

    {
        if (doorType == DoorType.Right)
        {
            rightDoorBonusType = (BonusType)Random.Range(0, 4);
            rightDoorBonusAmount = GetRandomAmount(rightDoorBonusType);
        }
        else if (doorType == DoorType.Left)
        {
            leftDoorBonusType = (BonusType)Random.Range(0, 4);
            leftDoorBonusAmount = GetRandomAmount(leftDoorBonusType);
        }
    }

    void UpdateDoorColors()
    {
        if(doorType == DoorType.Right && RightdoorSprite != null)
        {
            switch (rightDoorBonusType)
            {
                case BonusType.Addition:
                    RightdoorSprite.color = additionColor;
                    break;
                case BonusType.Difference:
                    RightdoorSprite.color = differenceColor;
                    break;
                case BonusType.Multiplication:
                    RightdoorSprite.color = multiplicationColor;
                    break;
                case BonusType.Division:
                    RightdoorSprite.color = divisionColor;
                    break;
            }
        }
        if(doorType == DoorType.Left && LeftdoorSprite != null)
        {
            switch (leftDoorBonusType)
            {
                case BonusType.Addition:
                    LeftdoorSprite.color = additionColor;
                    break;
                case BonusType.Difference:
                    LeftdoorSprite.color = differenceColor;
                    break;
                case BonusType.Multiplication:
                    LeftdoorSprite.color = multiplicationColor;
                    break;
                case BonusType.Division:
                    LeftdoorSprite.color = divisionColor;
                    break;
            }
        }
    }

    int GetRandomAmount(BonusType type)
    {
        switch (type)
        {
            case BonusType.Addition:
            case BonusType.Difference:
                return Random.Range(1, 16); // 1 ile 15 arasında
            case BonusType.Multiplication:
            case BonusType.Division:
                return Random.Range(2, 5); // 2, 3, 4
            default:
                return 1;
        }
    }

    private void UpdateDoorTexts()
    {
        if (doorType == DoorType.Right && rightDoorText != null)
        {
            rightDoorText.text = GetFormattedText(rightDoorBonusType, rightDoorBonusAmount);
        }
        if (doorType == DoorType.Left && leftDoorText != null)
        {
            leftDoorText.text = GetFormattedText(leftDoorBonusType, leftDoorBonusAmount);
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

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("Player") == false) return;
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
