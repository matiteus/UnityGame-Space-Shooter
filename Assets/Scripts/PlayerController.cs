using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //variables being modified in the engine
    [SerializeField]
    private Rigidbody2D myRigidbody2D; //player prefab
    [SerializeField]
    private float speed = 3f; //player movement speed
    [SerializeField]
    private GameObject playerBullet; // player bullet prefab

    //definition of the input actions class to be called
    private MyInputActions myInputActions;

    //other variables
    private float currentSpeed = 0f;
    private float attackTimer = 0f;
    private float cooldown = 0.5f; //cooldown player attack


    private void Awake()
    {
        myInputActions = new MyInputActions();

        myInputActions.Player.Enable();

        myInputActions.Player.Actions.performed += InputMovePerformedHandler;
        myInputActions.Player.Actions.canceled += InputMoveCanceledHandler;

        myInputActions.Player.Shoot.performed += OnShootPerformed;
    }

    private void Update()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }


    private void OnDestroy()
    {
        myInputActions.Player.Actions.performed -= InputMovePerformedHandler;
        myInputActions.Player.Actions.canceled -= InputMoveCanceledHandler;

        myInputActions.Player.Shoot.performed -= OnShootPerformed;
    }

    //movement methods
    private void InputMovePerformedHandler(InputAction.CallbackContext context)
    {
        float verticalInput = context.ReadValue<float>();
        currentSpeed = speed * verticalInput;
    }

    private void InputMoveCanceledHandler(InputAction.CallbackContext context)
    {
        currentSpeed = 0f;
    }
    //end moviment methods

    private void FixedUpdate()
    {
        myRigidbody2D.velocity =
            new Vector2(myRigidbody2D.velocity.x, currentSpeed);
    }

    //atack method
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (attackTimer<= 0f) 
        {
            GameObject bullet = Instantiate(playerBullet, transform.position, Quaternion.Euler(0, 0, -90));
            bullet.tag = "Player Bullet";
            attackTimer = cooldown;
        }

    }
    //end attack method

}
