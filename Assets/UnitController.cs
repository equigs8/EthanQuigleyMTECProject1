using UnityEngine;

public class UnitController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control movement speed

    private Rigidbody2D rb;

    public bool isControlling = false;

    public GameObject unitUI;

    private GameManager gameManager;

    public GameObject gameManagerObject;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on this GameObject!");
        }
    }

    void Update()
    {
        if (isControlling)
        {
            unitUI.SetActive(true);

            // Get input from WASD keys
            float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 for Left, 0 for None, 1 for Right
            float verticalInput = Input.GetAxisRaw("Vertical");   // -1 for Down, 0 for None, 1 for Up

            // Calculate movement direction
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized; // .normalized makes diagonal movement not faster

            // Apply movement using Rigidbody2D for physics-based movement
            rb.linearVelocity = moveDirection * moveSpeed;


            //  Alternative using transform.position (less realistic physics, but can be useful)
            //  Vector2 moveAmount = moveDirection * moveSpeed * Time.deltaTime; // Time.deltaTime makes movement frame-rate independent
            //  transform.position += new Vector3(moveAmount.x, moveAmount.y, 0f); // Assuming 2D, z is 0
        }
        else
        {
            unitUI.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!gameManager.CheckIfControlling())
        {
            isControlling = true;
            gameManager.ControllingUnit(true);
        }
        
    }

    public void Leave()
    {
        isControlling = false;
        gameManager.ControllingUnit(false);
    }
}
