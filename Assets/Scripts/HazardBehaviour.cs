using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Enemy"))
            {
                collision.GetComponent<HealthSystem>().TakeDamage(1);
                collision.gameObject.transform.position = Vector3.zero;
                return;
            }
            if (!collision.gameObject.GetComponent<PlayerController>().isInvulnerable)
            {
                collision.GetComponent<HealthSystem>().TakeDamage(1);
                collision.gameObject.transform.position = Vector3.zero;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Enemy"))
            {
                collision.GetComponent<HealthSystem>().TakeDamage(1);
                collision.gameObject.transform.position = Vector3.zero;
                return;
            }
            if (!collision.gameObject.GetComponent<PlayerController>().isInvulnerable)
            {
                collision.GetComponent<HealthSystem>().TakeDamage(1);
                collision.gameObject.transform.position = Vector3.zero;
            }
        }
    }
}
