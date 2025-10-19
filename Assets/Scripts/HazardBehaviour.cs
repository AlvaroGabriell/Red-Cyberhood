using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<HealthSystem>().canTakeDamage == true) collision.gameObject.transform.position = Vector3.zero;
            //collision.GetComponent<HealthSystem>().TakeDamage(1);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<HealthSystem>().canTakeDamage == true) collision.gameObject.transform.position = Vector3.zero;
            //collision.GetComponent<HealthSystem>().TakeDamage(1);
        }
    }
}
