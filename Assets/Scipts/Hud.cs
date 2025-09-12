using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TMP_Text turnTimerText;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turnTimerText.text = "Turn Timer: " + turnManager.turnTimer.ToString();
    }
}
