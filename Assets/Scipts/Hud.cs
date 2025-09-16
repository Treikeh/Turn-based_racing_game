using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TMP_Text turnTimerText;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text positionText;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turnTimerText.text = "Turn Timer: " + turnManager.turnTimer.ToString();
        playerText.text = turnManager.player1InControl ? "Player 1" : "Player 2";

        // Set position text
        if (turnManager.player1InControl)
        {
            positionText.text = turnManager.player1TrackDistance > turnManager.player2TrackDistance ? "1st" : "2nd";
        }
        else
        {
            positionText.text = turnManager.player2TrackDistance > turnManager.player1TrackDistance ? "1st" : "2nd";
        }
    }
}
