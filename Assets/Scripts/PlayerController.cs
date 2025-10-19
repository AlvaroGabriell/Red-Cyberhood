using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement = Vector2.zero;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateValues();
    }

    void UpdateValues()
    {
        // ---------- Animator ----------
        isMoving = Mathf.Abs(movement.x) > 0f || Mathf.Abs(movement.y) > 0f;
        animator.SetBool("isMoving", isMoving);
    }

    //Calcula e executa o movimento do jogador.
    public void HandleMovement()
    {
        rb.linearVelocity = movement * moveSpeed;

        if (movement.x > 0f) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movement.x < 0f) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    //Captura o input de movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnTimeTravel(InputAction.CallbackContext context)
    {

    }

    public void OnThrowBomb(InputAction.CallbackContext context)
    {

    }
    
    public void OnEscapeButton(InputAction.CallbackContext context)
    {
        UIController.Instance.HandleEscape();
    }
}