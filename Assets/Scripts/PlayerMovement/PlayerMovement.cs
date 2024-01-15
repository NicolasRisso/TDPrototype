using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float fastSpeed = 22f;
    [SerializeField] private float upSpeed = 5f;
    [SerializeField] private float fastUpSpeed = 10f;
    [SerializeField] private float downSpeed = 5f;
    [SerializeField] private float fastDownSpeed = 10f;

    [Header("Key Configuration")]
    [SerializeField] private KeyCode sprint;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;

    private float currentSpeed = 0f;
    private float verticalSpeed = 0f;

    private CharacterController cController;

    void Start()
    {
        cController = GetComponent<CharacterController>();
        currentSpeed = speed;
        Screen.fullScreen = true;
    }

    void Update()
    {
        SpeedDetector();
        Movement();
        DetectFullScreen();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = verticalSpeed;
        cController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void SpeedDetector()
    {
        verticalSpeed = 0f;

        if (Input.GetKey(sprint))
        {
            currentSpeed = fastSpeed;
            if (Input.GetKey(up)) verticalSpeed = fastUpSpeed;
            else if (Input.GetKey(down)) verticalSpeed = -fastDownSpeed;
        }
        else
        {
            currentSpeed = speed;
            if (Input.GetKey(up)) verticalSpeed = upSpeed;
            else if (Input.GetKey(down)) verticalSpeed = -downSpeed;
        }
    }

    private void DetectFullScreen()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
