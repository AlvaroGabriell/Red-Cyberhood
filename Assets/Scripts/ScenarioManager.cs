using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public GameObject pastScenario, futureScenario;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SwitchScenarios()
    {
        if (pastScenario.activeSelf)
        {
            pastScenario.SetActive(false);
            futureScenario.SetActive(true);
        } else
        {
            pastScenario.SetActive(true);
            futureScenario.SetActive(false);
        }
    }
}
