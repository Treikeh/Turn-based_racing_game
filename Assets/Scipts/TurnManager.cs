using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Car;
    [SerializeField] private GameObject player2Car;
    [SerializeField] private GameObject otherPlayerMarker;
    [SerializeField] private float maxTurnTime = 15f;

    private bool player1InControl = true;
    [HideInInspector] public float turnTimer = 0f;


    void Start()
    {
        turnTimer = maxTurnTime;
        player2Car.SetActive(false);
    }


    void Update()
    {
        // Switch player when timer runs out
        turnTimer -= Time.deltaTime * Time.timeScale;
        if (turnTimer <= 0f)
        {
            SwitchPlayer();
        }
    }


    private void SwitchPlayer()
    {
        StartCoroutine(nameof(PauseTimer));
        if (player1InControl == true)
        {
            player1InControl = false;
            player1Car.SetActive(false);
            player2Car.SetActive(true);
            otherPlayerMarker.transform.position = player1Car.transform.position;
            otherPlayerMarker.transform.rotation = player1Car.transform.rotation;
        }
        else
        {
            player1InControl = true;
            player2Car.SetActive(false);
            player1Car.SetActive(true);
            otherPlayerMarker.transform.position = player2Car.transform.position;
            otherPlayerMarker.transform.rotation = player2Car.transform.rotation;
        }
        turnTimer = maxTurnTime;
    }


    private IEnumerator PauseTimer()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }
}