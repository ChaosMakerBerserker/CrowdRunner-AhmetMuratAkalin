using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CrowdSystem crowdSystem;   
    public GameManager gameManager;   

    [Header("Movement Settings")]
    public float speed = 10f;
    public float slideSpeed = 5f;
    public float roadWidth = 20f;

    private Vector3 clickedScreenPosition;
    private Vector3 clickedPlayerPosition;
    private Vector3 targetPositionX;

    private Vector3 initialPosition;

    void Start()
    {
        targetPositionX = transform.position;
        initialPosition = transform.position;

        if(crowdSystem != null)
            crowdSystem.AddCrowd(crowdSystem.crowdCount - 1);
    }

    void Update()
    {
        if (gameManager != null && gameManager.gameEnded) return;
        HandleSlide();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (crowdSystem == null) return;

        float moveZ = speed * Time.deltaTime;
        float newZ = transform.position.z + moveZ;

        // Rotation satırını tamamen kaldırdık
        transform.position = new Vector3(targetPositionX.x, transform.position.y, newZ);
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        targetPositionX = initialPosition;
    }

    private void HandleSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedScreenPosition = Input.mousePosition;
            clickedPlayerPosition = transform.position;
            targetPositionX = clickedPlayerPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            float xDiff = (Input.mousePosition.x - clickedScreenPosition.x) / Screen.width * slideSpeed;
            targetPositionX.x = Mathf.Clamp(clickedPlayerPosition.x + xDiff, -roadWidth / 2, roadWidth / 2);
        }

        targetPositionX.x = Mathf.Lerp(transform.position.x, targetPositionX.x, 0.2f);
    }
}
