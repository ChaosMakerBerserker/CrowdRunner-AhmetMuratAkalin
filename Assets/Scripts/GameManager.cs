using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject RestartUI;
    public bool gameEnded = false;
    public PlayerController playerController;
    public Doors[] doorsList = new Doors[0];

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
            foreach (var door in doorsList)
            {
                door.SetRandoms();
            }

    }

    void Start()
    {
        gameEnded = false;
        if (RestartUI != null)
            RestartUI.SetActive(false);
    }   
}