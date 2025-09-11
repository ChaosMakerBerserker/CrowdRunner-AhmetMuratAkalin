using UnityEngine;
using System.Collections.Generic; // List için
using TMPro; // TextMeshPro için

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject RestartUI;

    [Header("Player")]
    public PlayerController playerController;

    [Header("Doors")]
    public List<Doors> doorsList = new List<Doors>(); // Dinamik liste
    public GameObject rightDoorPrefab; // Sağ kapı prefab'ı (Inspector'da ata)
    public GameObject leftDoorPrefab;  // Sol kapı prefab'ı (Inspector'da ata)
    public Transform doorsParent;      // Kapıların parent'ı (opsiyonel, Inspector'da ata)

    public bool gameEnded = false;

    void Start()
    {
        gameEnded = false;
        if (RestartUI != null)
            RestartUI.SetActive(false);
        SpawnDoors(); // Kapıları spawn et
    }

    // Kapıları spawn et (sağ ve sol)
    private void SpawnDoors()
    {
        if (rightDoorPrefab == null || leftDoorPrefab == null)
        {
            Debug.LogError("GameManager: rightDoorPrefab veya leftDoorPrefab atanmamış! Inspector'da ata.");
            return;
        }

        // Örnek pozisyonlar – sahnenize göre değiştir (örneğin, ilk kapı z=0)
        Vector3 basePos = new Vector3(0, 0, 0); // Kapı baz pozisyonu
        Vector3 rightPos = basePos + new Vector3(5, 0, 0); // Sağ kapı x=5
        Vector3 leftPos = basePos + new Vector3(-5, 0, 0);  // Sol kapı x=-5

        // Sağ kapı spawn
        GameObject rightDoorObj = Instantiate(rightDoorPrefab, rightPos, Quaternion.identity, doorsParent);
        Doors rightDoors = rightDoorObj.GetComponent<Doors>();
        if (rightDoors != null)
        {
            doorsList.Add(rightDoors); // Listeye ekle
            rightDoors.RandomizeDoor(); // Yeniden randomize et
        }

        // Sol kapı spawn
        GameObject leftDoorObj = Instantiate(leftDoorPrefab, leftPos, Quaternion.identity, doorsParent);
        Doors leftDoors = leftDoorObj.GetComponent<Doors>();
        if (leftDoors != null)
        {
            Renderer leftRenderer = leftDoorObj.GetComponentInChildren<Renderer>();
            TextMeshPro leftText = leftDoorObj.GetComponentInChildren<TextMeshPro>();
            leftDoors.SetLeftDoorReferences(leftRenderer, leftText, leftDoorObj.transform);
            doorsList.Add(leftDoors); // Listeye ekle
            leftDoors.RandomizeDoor(); // Yeniden randomize et
        }

        Debug.Log("GameManager: Kapılar spawn edildi. Toplam kapı sayısı: " + doorsList.Count);
    }

    public void EndGame()
    {
        gameEnded = true;
        Debug.Log("Oyun bitti!");
        RestartUI.SetActive(true);
    }

    public void RestartGame()
    {
        gameEnded = false;
        Debug.Log("Oyun yeniden başladı!");
        RestartUI.SetActive(false);
        playerController.ResetPosition();
        
        // Mevcut kapıları temizle ve yeniden spawn et
        foreach (var door in doorsList)
        {
            if (door != null)
                Destroy(door.gameObject);
        }
        doorsList.Clear(); // Listeyi temizle
        SpawnDoors(); // Yeniden spawn et
    }
}