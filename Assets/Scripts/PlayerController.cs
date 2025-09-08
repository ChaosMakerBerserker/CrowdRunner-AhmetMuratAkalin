using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CrowdSystem crowdSystem;   // CrowdSystem referansı
    public GameObject runnerPrefab;   // Eklemek istediğin runner prefab
    public Transform spawnPoint;      // Runner spawn pozisyonu

    [Header("Movement Settings")]
    public float speed = 10f;
    public float turnSpeed = 10f;
    public float slideSpeed = 5f;
    public float roadWidth = 10f;

    private Vector3 clickedScreenPosition;
    private Vector3 clickedPlayerPosition;
    private Vector3 targetPositionX;

    void Start()
    {
        targetPositionX = transform.position;

        // Başlangıçta bir runner ekle
        if (crowdSystem != null && runnerPrefab != null && spawnPoint != null)
        {
            GameObject newRunner = Instantiate(runnerPrefab, spawnPoint.position, Quaternion.identity);
            crowdSystem.AddCrowd(newRunner);

            RunnerFollow follow = newRunner.GetComponent<RunnerFollow>();
            if (follow != null)
                follow.target = crowdSystem.leader;
        }
    }

    void Update()
    {
        if (GameManager.gameEnded) return; // Oyun bitti, player durur
        HandleSlide();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (crowdSystem == null) return;

        // Player hedef pozisyonu: ilk runner veya lider
        Vector3 targetPos = crowdSystem.GetCrowdCount() > 0 ? 
                            crowdSystem.runners[0].transform.position : 
                            crowdSystem.leader.position;

        Vector3 direction = targetPos - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        float moveZ = speed * Time.deltaTime;
        float newZ = transform.position.z + moveZ;

        transform.position = new Vector3(targetPositionX.x, transform.position.y, newZ);
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
