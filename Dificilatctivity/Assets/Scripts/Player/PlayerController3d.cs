using UnityEngine;
using UnityEngine.InputSystem; // OBRIGATÓRIO: Permite ler o InputValue

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Mantém o movimento funcionando mesmo se o Player Input estiver com uma
        // action map inválida ou sem uma ação chamada "Move".
        ReadKeyboardMovement();
    }

    private void ReadKeyboardMovement()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;

        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // Chamado automaticamente pelo Player Input quando as teclas de mover (WASD) são pressionadas
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Chamado automaticamente pelo Player Input quando a tecla de pular (Espaço) é pressionada
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Checagem se a bolinha está tocando no chão
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.5f, 0.4f, groundLayer);

        // Aplica o movimento nos eixos X e Z (3D)
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }
}
