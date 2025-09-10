using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshPro rightDoorText; // Sağ kapı texti
    public TextMeshPro leftDoorText;  // Sol kapı texti (ama mantık sağ kapı gibi olacak)

    public enum BonusType { Addition, Subtraction, Multiplication, Division }
    private BonusType rightDoorType;
    private int rightDoorAmount;

    [Header("References")]
    public CrowdSystem crowdSystem;

    [Header("Visuals")]
    public Renderer rightDoorRenderer;
    public Renderer leftDoorRenderer;
    public Color greenColor = Color.green;
    public Color redColor = Color.red;

    public bool isFirstDoor = false;

    void Start()
    {
        RandomizeDoor();
        UpdateDoorTexts();
        UpdateDoorColors();
    }

    // Bonusları randomize et
    public void RandomizeDoor()
    {
        if (isFirstDoor)
        {
            rightDoorType = Random.value > 0.5f ? BonusType.Addition : BonusType.Multiplication;
        }
        else
        {
            rightDoorType = (BonusType)Random.Range(0, 4);
        }

        rightDoorAmount = GetRandomAmount(rightDoorType);
    }

    private int GetRandomAmount(BonusType type)
    {
        switch (type)
        {
            case BonusType.Addition:
            case BonusType.Subtraction: return Random.Range(1, 11);
            case BonusType.Multiplication:
            case BonusType.Division: return Random.Range(2, 5);
            default: return 1;
        }
    }

    public void UpdateDoorTexts()
    {
        string text = GetFormattedText(rightDoorType, rightDoorAmount);

        if (rightDoorText != null)
            rightDoorText.text = text;

        if (leftDoorText != null)
            leftDoorText.text = text; // Sol kapıyı sağ kapı gibi göster
    }

    private string GetFormattedText(BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition: return "+" + amount;
            case BonusType.Subtraction: return "-" + amount;
            case BonusType.Multiplication: return "x" + amount;
            case BonusType.Division: return "/" + amount;
            default: return amount.ToString();
        }
    }

    public void UpdateDoorColors()
    {
        Color color = (rightDoorType == BonusType.Addition || rightDoorType == BonusType.Multiplication) ? greenColor : redColor;

        if (rightDoorRenderer != null)
            foreach (var r in rightDoorRenderer.GetComponentsInChildren<Renderer>())
                if (r != null && r.material != null)
                    r.material.color = color;

        if (leftDoorRenderer != null)
            foreach (var r in leftDoorRenderer.GetComponentsInChildren<Renderer>())
                if (r != null && r.material != null)
                    r.material.color = color; // Sol kapıyı sağ kapı gibi renklendir
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || crowdSystem == null) return;

        ApplyDoorBonus(rightDoorType, rightDoorAmount); // Hem sağ hem sol aynı mantık
        gameObject.SetActive(false);
    }

    private void ApplyDoorBonus(BonusType type, int amount)
    {
        int currentCrowd = crowdSystem.GetCrowdCount();

        switch (type)
        {
            case BonusType.Addition:
                crowdSystem.AddCrowd(amount);
                break;
            case BonusType.Subtraction:
                crowdSystem.AddCrowd(-amount);
                break;
            case BonusType.Multiplication:
                crowdSystem.AddCrowd(currentCrowd * (amount - 1));
                break;
            case BonusType.Division:
                if (amount != 0)
                    crowdSystem.AddCrowd(-(currentCrowd - currentCrowd / amount));
                break;
        }
    }
}
