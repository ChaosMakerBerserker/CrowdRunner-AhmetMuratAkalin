using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshPro rightDoorText; // Sağ kapı texti
    public TextMeshPro leftDoorText;  // Sol kapı texti

    public enum BonusType { Addition, Subtraction, Multiplication, Division }
    private BonusType rightDoorType;
    private int rightDoorAmount;
    private BonusType leftDoorType;
    private int leftDoorAmount;

    [Header("References")]
    public CrowdSystem crowdSystem;

    [Header("Visuals")]
    public Renderer rightDoorRenderer;
    public Renderer leftDoorRenderer;
    private readonly Color additionColor = Color.green;      // Toplama: Yeşil
    private readonly Color subtractionColor = Color.red;     // Çıkarma: Kırmızı
    private readonly Color multiplicationColor = Color.cyan; // Çarpma: Cyan
    private readonly Color divisionColor = Color.magenta;    // Bölme: Mor (magenta)

    [Header("Kapı Pozisyonları")]
    public Transform rightDoorTransform; // Sağ kapının transform'u
    public Transform leftDoorTransform;  // Sol kapının transform'u

    public bool isFirstDoor = false;

    void Start()
    {
        Debug.Log(gameObject.name + ": Doors Start çağrıldı");
        RandomizeDoor();
        UpdateDoorTexts();
        UpdateDoorColors();
    }

    // Bonusları randomize et
    public void RandomizeDoor()
    {
        // Sağ kapı için mevcut mantık
        if (isFirstDoor)
        {
            rightDoorType = Random.value > 0.5f ? BonusType.Addition : BonusType.Multiplication;
        }
        else
        {
            rightDoorType = (BonusType)Random.Range(0, 4);
        }
        rightDoorAmount = GetRandomAmount(rightDoorType);

        // Sol kapı için ayrı mantık: İlk kapıysa sağın tersini al (pozitif), yoksa rastgele ama sağdan farklı
        if (isFirstDoor)
        {
            leftDoorType = (rightDoorType == BonusType.Addition) ? BonusType.Multiplication : BonusType.Addition;
        }
        else
        {
            leftDoorType = (BonusType)Random.Range(0, 4);
            while (leftDoorType == rightDoorType)
            {
                leftDoorType = (BonusType)Random.Range(0, 4);
            }
        }
        leftDoorAmount = GetRandomAmount(leftDoorType);

        Debug.Log(gameObject.name + ": RandomizeDoor - rightDoorType: " + rightDoorType + ", rightDoorAmount: " + rightDoorAmount +
                  " | leftDoorType: " + leftDoorType + ", leftDoorAmount: " + leftDoorAmount);
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
        string rightText = GetFormattedText(rightDoorType, rightDoorAmount);
        string leftText = GetFormattedText(leftDoorType, leftDoorAmount);

        if (rightDoorText != null)
            rightDoorText.text = rightText;
        else
            Debug.LogWarning(gameObject.name + ": rightDoorText atanmamış!");

        if (leftDoorText != null)
            leftDoorText.text = leftText;
        else
            Debug.LogWarning(gameObject.name + ": leftDoorText atanmamış!");
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
        Debug.Log(gameObject.name + ": UpdateDoorColors çağrıldı");

        Color rightColor;
        switch (rightDoorType)
        {
            case BonusType.Addition:
                rightColor = additionColor;
                break;
            case BonusType.Subtraction:
                rightColor = subtractionColor;
                break;
            case BonusType.Multiplication:
                rightColor = multiplicationColor;
                break;
            case BonusType.Division:
                rightColor = divisionColor;
                break;
            default:
                rightColor = Color.white;
                Debug.LogWarning(gameObject.name + ": Sağ için geçersiz BonusType, varsayılan renk beyaz");
                break;
        }

        Color leftColor;
        switch (leftDoorType)
        {
            case BonusType.Addition:
                leftColor = additionColor;
                break;
            case BonusType.Subtraction:
                leftColor = subtractionColor;
                break;
            case BonusType.Multiplication:
                leftColor = multiplicationColor;
                break;
            case BonusType.Division:
                leftColor = divisionColor;
                break;
            default:
                leftColor = Color.white;
                Debug.LogWarning(gameObject.name + ": Sol için geçersiz BonusType, varsayılan renk beyaz");
                break;
        }

        ApplyColorToRenderer(rightDoorRenderer, rightColor, "sağ kapı");
        ApplyColorToRenderer(leftDoorRenderer, leftColor, "sol kapı");
    }

    private void ApplyColorToRenderer(Renderer renderer, Color color, string doorName)
    {
        if (renderer == null)
        {
            Debug.LogWarning(gameObject.name + ": " + doorName + " renderer atanmamış!");
            return;
        }

        foreach (var r in renderer.GetComponentsInChildren<Renderer>())
        {
            if (r == null || r.material == null)
            {
                Debug.LogWarning(gameObject.name + ": " + doorName + " renderer veya material eksik!");
                continue;
            }

            Material mat = r.material;
            if (mat.HasProperty("_Color"))
            {
                mat.SetColor("_Color", color);
                Debug.Log(gameObject.name + ": " + doorName + " _Color olarak renklendi: " + color);
            }
            else
            {
                Debug.LogWarning(gameObject.name + ": " + doorName + " material'inde _Color property'si yok! Shader: " + mat.shader.name);
                mat.shader = Shader.Find("Unlit/Color");
                mat.SetColor("_Color", color);
                Debug.Log(gameObject.name + ": " + doorName + " Unlit/Color Shader ile renklendi: " + color);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || crowdSystem == null)
        {
            Debug.LogWarning(gameObject.name + ": PlayerController veya CrowdSystem eksik!");
            return;
        }

        // Hangi kapıdan geçtiğini belirle
        Vector3 playerPos = other.transform.position;
        Vector3 rightPos = rightDoorTransform != null ? rightDoorTransform.position : transform.position;
        Vector3 leftPos = leftDoorTransform != null ? leftDoorTransform.position : transform.position;
        float distToRight = Vector3.Distance(playerPos, rightPos);
        float distToLeft = Vector3.Distance(playerPos, leftPos);

        BonusType type;
        int amount;
        if (distToRight < distToLeft)
        {
            type = rightDoorType;
            amount = rightDoorAmount;
            Debug.Log(gameObject.name + ": Sağ kapı seçildi - Type: " + type + ", Amount: " + amount +
                      ", PlayerPos: " + playerPos + ", RightPos: " + rightPos + ", DistToRight: " + distToRight);
        }
        else
        {
            type = leftDoorType;
            amount = leftDoorAmount;
            Debug.Log(gameObject.name + ": Sol kapı seçildi - Type: " + type + ", Amount: " + amount +
                      ", PlayerPos: " + playerPos + ", LeftPos: " + leftPos + ", DistToLeft: " + distToLeft);
        }

        ApplyDoorBonus(type, amount);
        gameObject.SetActive(false);
    }

    private void ApplyDoorBonus(BonusType type, int amount)
    {
        int currentCrowd = crowdSystem.GetCrowdCount();
        int newCrowd;

        switch (type)
        {
            case BonusType.Addition:
                newCrowd = currentCrowd + amount;
                crowdSystem.AddCrowd(amount);
                break;
            case BonusType.Subtraction:
                newCrowd = Mathf.Max(0, currentCrowd - amount);
                crowdSystem.AddCrowd(-(currentCrowd - newCrowd));
                break;
            case BonusType.Multiplication:
                newCrowd = currentCrowd * amount; // Örneğin, 5 * 3 = 15
                crowdSystem.AddCrowd(newCrowd - currentCrowd); // 15 - 5 = 10 ekler
                break;
            case BonusType.Division:
                if (amount != 0)
                {
                    newCrowd = currentCrowd / amount; // Tam sayı bölme, örneğin 5 / 2 = 2
                    crowdSystem.AddCrowd(newCrowd - currentCrowd);
                }
                else
                {
                    newCrowd = currentCrowd;
                    Debug.LogWarning(gameObject.name + ": Bölme işleminde amount sıfır!");
                }
                break;
            default:
                newCrowd = currentCrowd;
                Debug.LogWarning(gameObject.name + ": Geçersiz BonusType!");
                break;
        }

        Debug.Log(gameObject.name + ": ApplyDoorBonus - Type: " + type + ", Amount: " + amount +
                  ", Eski Kalabalık: " + currentCrowd + ", Yeni Kalabalık: " + newCrowd +
                  ", Eklenen Miktar: " + (newCrowd - currentCrowd));
    }
}