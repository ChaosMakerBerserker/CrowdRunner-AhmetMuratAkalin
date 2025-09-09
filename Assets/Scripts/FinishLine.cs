using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private Doors[] doorsList = new Doors[0];
    void Start ()
    {
        doorsList = FindObjectsOfType<Doors>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Level bitti!");

            // Level tamamlandığında FinishUI

            player.ResetPosition();
            foreach (var door in doorsList)
            {
                door.SetRandoms();
            }

        }
    }
}