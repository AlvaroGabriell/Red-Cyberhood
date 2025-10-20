using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(AttributesSystem))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private HealthSystem health;
    private AttributesSystem attributes;
    private ScenarioManager scenarioManager;

    private Vector2 movement = Vector2.zero, lastDirection = Vector2.zero;
    public float dashSpeedMultiplier = 2f, playerDashInfluence = 0.3f, dashDuration = 0.3f, timeTravelCooldown = 3f, lastTimeTravel = -1;
    private bool isMoving = false, isDashing = false, moved = false; 
    public bool isInvulnerable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthSystem>();
        attributes = GetComponent<AttributesSystem>();
        scenarioManager = GameObject.FindGameObjectWithTag("ScenarioManager").GetComponent<ScenarioManager>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateValues();
    }

    void UpdateValues()
    {
        // ---------- Animator ----------
        isMoving = Mathf.Abs(movement.x) > 0f || Mathf.Abs(movement.y) > 0f && !isDashing;
        animator.SetBool("isMoving", isMoving);

        if (movement != Vector2.zero && !isDashing) lastDirection = movement;
        animator.SetFloat("movementX", lastDirection.x);
        animator.SetFloat("movementY", lastDirection.y);
    }

    //Calcula e executa o movimento do jogador.
    public void HandleMovement()
    {
        if (isDashing)
        {
            Vector2 playerInfluence = movement.normalized * playerDashInfluence;
            Vector2 finalDirection = (lastDirection.normalized + playerInfluence).normalized;
            Vector2 dashVelocity = attributes.dashSpeed.FinalValue * attributes.moveSpeed.FinalValue * finalDirection;
            rb.linearVelocity = dashVelocity;
            return;
        }

        rb.linearVelocity = movement * attributes.moveSpeed.FinalValue;

        if(movement != Vector2.zero) transform.localScale = new Vector3(Mathf.Sign(movement.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    //Captura o input de movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        if(moved == false)
        {
            UIController.Instance.hideTutorial = true;
            moved = true;
        }
        movement = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (isDashing) return;

        if (context.performed)
        {
            StartCoroutine(PerformDash());
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // No implemented yet
    }

    public void OnTimeTravel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Time.time - lastTimeTravel >= timeTravelCooldown)
            {
                lastTimeTravel = Time.time;
                PerformTimeTravel();
            }
        }
    }

    public void OnThrowBomb(InputAction.CallbackContext context)
    {
        // No implemented yet
    }

    public void OnEscapeButton(InputAction.CallbackContext context)
    {
        UIController.Instance.HandleEscape();
    }

    private IEnumerator PerformDash()
    {
        if (lastDirection == Vector2.zero) yield break;

        isDashing = true;
        isInvulnerable = true;
        animator.SetTrigger("dash");
        SFXManager.Instance.Play("Dash");

        //movement = Vector2.zero;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        isInvulnerable = false;
    }
    
    private void PerformTimeTravel()
    {
        scenarioManager.SwitchScenarios();
        MusicManager.Instance.SwitchGameplayMusic();
    }
}