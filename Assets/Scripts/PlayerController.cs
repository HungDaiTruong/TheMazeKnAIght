using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Transform playerModel; // Reference to the player model
    private MazeGenerator mazeGenerator;
    private UIManager uiManager; // Reference to the UIManager

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        uiManager = FindObjectOfType<UIManager>(); // Find the UIManager in the scene
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized * speed;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            playerModel.rotation = Quaternion.RotateTowards(playerModel.rotation, toRotation * Quaternion.Euler(-90, 0, 0), 360 * Time.deltaTime);
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            mazeGenerator.GenerateNextLevel();
            uiManager.ResetLevelTimer();
        }
        else if (other.gameObject.GetComponent<CoinManager>())
        {
            uiManager.IncreaseCoinCount();
            Destroy(other.gameObject);
        }
    }
}
