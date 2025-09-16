using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TMP_Text turnTimerText;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text playerSwitchText;
    [SerializeField] private TMP_Text controlsText;

    private bool gameActive = false;


    void Start()
    {
        Time.timeScale = 0f;
        playerSwitchText.text = "Press SPACE to start";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameActive && Input.GetKeyDown(KeyCode.Space))
        {
            gameActive = true;
            Time.timeScale = 1f;
            controlsText.gameObject.SetActive(false);
        }


        turnTimerText.text = "Turn Timer: " + turnManager.turnTimer.ToString();
        playerText.text = turnManager.player1InControl ? "Player 1" : "Player 2";


        if (gameActive)
        {
            playerSwitchText.text = turnManager.player1InControl ? "Player 1 Get ready" : "Player 2 Get Ready";
        }

        // Set position text
        if (turnManager.player1InControl)
        {
            positionText.text = turnManager.player1TrackDistance > turnManager.player2TrackDistance ? "1st" : "2nd";
        }
        else
        {
            positionText.text = turnManager.player2TrackDistance > turnManager.player1TrackDistance ? "1st" : "2nd";
        }

        playerSwitchText.gameObject.SetActive(Time.timeScale < 1f);
    }
}
