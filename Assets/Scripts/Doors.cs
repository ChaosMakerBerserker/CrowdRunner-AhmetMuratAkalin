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

    // GameManager'dan sol kapı eklendiğinde referansları güncelle (public metod)
    public void SetLeftDoorReferences(Renderer leftRenderer, TextMeshPro leftText, Transform leftTrans)
    {
        leftDoorRenderer = leftRenderer;
        leftDoorText = leftText;
        leftDoorTransform = leftTrans;
        UpdateDoorTexts(); // Metinleri güncelle
        UpdateDoorColors(); // Renkleri güncelle
        Debug.Log(gameObject.name + ": Sol kapı referansları güncellendi. LeftTransform X: " + leftTrans.position.x);
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

        Debug.Log(gameObject.name + ": Door Texts Güncellendi - Sağ: " + rightText + ", Sol: " + leftText);
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

        Color rightColor = GetColorForType(rightDoorType);
        Color leftColor = GetColorForType(leftDoorType);

        ApplyColorToRenderer(rightDoorRenderer, rightColor, "sağ kapı");
        ApplyColorToRenderer(leftDoorRenderer, leftColor, "sol kapı");
    }

    private Color GetColorForType(BonusType type)
    {
        switch (type)
        {
            case BonusType.Addition: return additionColor;
            case BonusType.Subtraction: return subtractionColor;
            case BonusType.Multiplication: return multiplicationColor;
            case BonusType.Division: return divisionColor;
            default: return Color.white;
        }
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
            if (r == null || r.material == null) continue;

            Material mat = r.material;
            if (mat.HasProperty("_Color"))
            {
                mat.SetColor("_Color", color);
                Debug.Log(gameObject.name + ": " + doorName + " _Color olarak renklendi: " + color);
            }
            else
            {
                Debug.LogWarning(gameObject.name + ": " + doorName + " material'inde _Color yok! Shader: " + mat.shader.name);
                mat.shader = Shader.Find("Unlit/Color");
                mat.SetColor("_Color", color);
                Debug.Log(gameObject.name + ": " + doorName + " Unlit/Color ile renklendi: " + color);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + ": Tetikleyici sadece Player tag'i için: " + other.name);
            return;
        }

        if (crowdSystem == null)
        {
            Debug.LogError(gameObject.name + ": CrowdSystem atanmamış! Inspector'da ata.");
            return;
        }

        // Sol/Sağ kapı seçimi: Player X < 0 ise sol (sahne sol taraf negatif x varsayımı – değiştirilebilir)
        Vector3 playerPos = other.transform.position;
        bool isLeftDoor = playerPos.x < 0; // Sol kapı için player x < 0 (sahne ayarına göre değiştir, örneğin < transform.position.x - 1)

        BonusType type;
        int amount;
        string selectedDoorText;
        if (isLeftDoor)
        {
            type = leftDoorType;
            amount = leftDoorAmount;
            selectedDoorText = GetFormattedText(leftDoorType, leftDoorAmount);
            Debug.Log(gameObject.name + ": Sol kapı seçildi - Type: " + type + ", Amount: " + amount + ", Kapı Metni: " + selectedDoorText + ", Player X: " + playerPos.x);
        }
        else
        {
            type = rightDoorType;
            amount = rightDoorAmount;
            selectedDoorText = GetFormattedText(rightDoorType, rightDoorAmount);
            Debug.Log(gameObject.name + ": Sağ kapı seçildi - Type: " + type + ", Amount: " + amount + ", Kapı Metni: " + selectedDoorText + ", Player X: " + playerPos.x);
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
                newCrowd = Mathf.Max(1, currentCrowd - amount);
                crowdSystem.AddCrowd(-amount);
                break;
            case BonusType.Multiplication:
                newCrowd = currentCrowd * amount;
                crowdSystem.AddCrowd(newCrowd - currentCrowd);
                break;
            case BonusType.Division:
                if (amount != 0)
                {
                    newCrowd = Mathf.Max(1, currentCrowd / amount);
                    crowdSystem.AddCrowd(newCrowd - currentCrowd);
                }
                else
                {
                    newCrowd = currentCrowd;
                    Debug.LogWarning(gameObject.name + ": Bölme amount=0!");
                }
                break;
            default:
                newCrowd = currentCrowd;
                break;
        }

        int finalCrowd = crowdSystem.GetCrowdCount();
        Debug.Log(gameObject.name + ": ApplyDoorBonus - Type: " + type + ", Amount: " + amount +
                  ", Eski: " + currentCrowd + ", Hesaplanan Yeni: " + newCrowd +
                  ", Eklenen: " + (newCrowd - currentCrowd) + ", Final CrowdSystem: " + finalCrowd);
    }
}