using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CyberwolfIdleRandomizer : MonoBehaviour
{
    private Animator animator;
    private static readonly string[] idleStates = { "Cyberwolf_Idle1", "Cyberwolf_Idle2", "Cyberwolf_Idle3" };

    void Start()
    {
        animator = GetComponent<Animator>();

        string chosenIdle = idleStates[Random.Range(0, idleStates.Length)];

        animator.Play(chosenIdle);
    }
}
