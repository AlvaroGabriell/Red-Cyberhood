using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyRenderer : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private static readonly string[] idleStates = { "Cyberwolf_Idle1", "Cyberwolf_Idle2", "Cyberwolf_Idle3" };
    private string chosenIdle;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        chosenIdle = idleStates[Random.Range(0, idleStates.Length)];

        animator.Play(chosenIdle);
    }

    void OnEnable()
    {
        animator.Play(chosenIdle);
    }
}
