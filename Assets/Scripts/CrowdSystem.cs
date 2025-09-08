using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshPro rightDoorText;
    public TextMeshPro leftDoorText;

    [Header("References")]
    public CrowdSystem crowdSystem;

    [Header("Visuals")]
    public Renderer rightDoorRenderer;
    public Renderer leftDoorRenderer;
    public Color greenColor = Color.green;
    public Color redColor = Color.red;

    // Random bonus bilgileri
    private bool rightDoorIsAddition;
    private int rightDoorAmount;

    private bool leftDoorIsAddition;
    private int leftDoorAmount;

    void Start()
    {
        RandomizeDoor();
        UpdateDoorTexts();
        UpdateDoorColors();
    }

    // Random bonusları belirler
    public void RandomizeDoor()
    {
        rightDoorIsAddition = Random.value > 0.5f;
        leftDoorIsAddition = Random.value > 0.5f;

        rightDoorAmount = Random.Range(1, 11);
        leftDoorAmount = Random.Range(1, 11);
    }

    public void UpdateDoorTexts()
    {
        if (rightDoorText != null)
        {
            rightDoorText.text = (rightDoorIsAddition ? "+" : "-") + rightDoorAmount;
            rightDoorText.color = rightDoorIsAddition ? greenColor : redColor;
        }

        if (leftDoorText != null)
        {
            leftDoorText.text = (leftDoorIsAddition ? "+" : "-") + leftDoorAmount;
            leftDoorText.color = leftDoorIsAddition ? greenColor : redColor;
        }
    }

    public void UpdateDoorColors()
    {
        if (rightDoorRenderer != null)
        {
            foreach (var r in rightDoorRenderer.GetComponentsInChildren<Renderer>())
            {
                if (r != null && r.material != null)
                    r.material.color = rightDoorIsAddition ? greenColor : redColor;
            }
        }

        if (leftDoorRenderer != null)
        {
            foreach (var r in leftDoorRenderer.GetComponentsInChildren<Renderer>())
            {
                if (r != null && r.material != null)
                    r.material.color = leftDoorIsAddition ? greenColor : redColor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || crowdSystem == null) return;

        float playerX = other.transform.position.x;
        float middleX = transform.position.x;

        // Sağ veya sol kapıya göre bonus uygula
        if (playerX >= middleX)
            ApplyDoorBonus(rightDoorIsAddition, rightDoorAmount);
        else
            ApplyDoorBonus(leftDoorIsAddition, leftDoorAmount);

        // Kapıyı kapat
        gameObject.SetActive(false);
    }

    private void ApplyDoorBonus(bool isAddition, int amount)
    {
        if (isAddition)
            crowdSystem.AddRunners(amount);
        else
            crowdSystem.RemoveRunners(amount);
    }
}
