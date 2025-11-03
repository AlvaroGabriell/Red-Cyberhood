using System.Collections;
using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{
    public float safeTime = 0.06f;
    private Coroutine damageCoroutine;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        damageCoroutine ??= StartCoroutine(DamageCountdown(collision.gameObject));
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        damageCoroutine ??= StartCoroutine(DamageCountdown(collision.gameObject));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if(damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageCountdown(GameObject player)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            Damage(player);
            yield break;
        }
        yield return new WaitForSeconds(safeTime);
        Damage(player);
    }
    
    private void Damage(GameObject playerObject)
    {
        damageCoroutine = null;
        var player = playerObject.GetComponent<PlayerController>();
        var health = player.GetComponent<HealthSystem>();

        if (gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(1);
            playerObject.transform.position = Vector3.zero;
            return;
        }

        if (!player.isInvulnerable)
        {
            health.TakeDamage(1);
            playerObject.transform.position = Vector3.zero;
        }
    }
}
