using UnityEngine;
using UnityEngine.InputSystem;

public class MatriarchBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
            UIController.Instance.OpenMenu(UIController.Instance.VictoryMenu);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
            UIController.Instance.OpenMenu(UIController.Instance.VictoryMenu);
        }
    }
}
