using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public PlayerInputActions PlayerInput;

    private void Awake()
    {
        Instance = this;
        PlayerInput = new PlayerInputActions();
    }

    private void Update()
    {
        PlayerInput.Movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            PlayerInput.Movement.y++;
        }

        if (Input.GetKey(KeyCode.S))
        {
            PlayerInput.Movement.y--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerInput.Movement.x++;
        }

        if (Input.GetKey(KeyCode.A))
        {
            PlayerInput.Movement.x--;
        }
    }
}

public struct PlayerInputActions
{
    public Vector2 Movement;
}
