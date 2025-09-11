using System;
using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform target;
    public GameManager gameManager;

    [Header("Crowd Settings")]
    public int crowdCount = 2; // Başlangıçta 2 runner
    public float spacing = 1.5f;

    private void Awake()
    {
        crowdCount = PlayerPrefs.GetInt("LevelID", 1);
        
        UpdateRunners();
    }

    void Start()
    {
    }

    public void AddCrowd(int amount)
    {
        crowdCount += amount;
        
        if (crowdCount <= 0)
        {
            gameManager.LoseGame();
            return;
        }
        
        UpdateRunners();
    }

    private void UpdateRunners()
    {
        print("Crowd Güncelleniyor. Yeni Crowd Sayısı: " + crowdCount);
        if (runnerParent == null || runnerPrefab == null) return;

        // Önce var olan runner’ları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
        }

        // Yeni runner’ları oluştur
        for (int i = 0; i < crowdCount; i++)
        {
            print("Runner oluşturuluyor: " + i);
            GameObject runnerObj = Instantiate(runnerPrefab, runnerParent);
            // Runner’ları düzenli bir şekilde yerleştir (sollu sağlı grid şeklinde dizilim)
            int row = i / 8; // Her 5 runner’da bir alt satıra geç
            int col = i % 8;
            Vector3 positionOffset = new Vector3((col - 2) * spacing, 0, -row * spacing); // Ortalamak için col-2
            runnerObj.transform.localPosition = positionOffset;
            
            runnerObj.SetActive(true);
            
            // RunnerFollow scriptini al ve hedefi ata
            RunnerFollow followScript = runnerObj.GetComponent<RunnerFollow>();
            if (followScript != null)
            {
                followScript.target = target;
                followScript.gameManager = gameManager;                
            }
        }
    }

    public void RemoveCrowdList()
    {
        if (runnerParent == null) return;

        // Var olan runner’ları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
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
