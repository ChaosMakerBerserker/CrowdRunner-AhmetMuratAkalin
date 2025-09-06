using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded = false;

    public static void EndGame()
    {
        gameEnded = true;
        Debug.Log("Oyun bitti!");
    }
}