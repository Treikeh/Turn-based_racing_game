using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Car;
    [SerializeField] private GameObject player2Car;
    [SerializeField] private GameObject otherPlayerMarker;
    [SerializeField] private float maxTurnTime = 15f;
    [SerializeField] private SplineContainer trackSpilneContainer;

    private NativeSpline nativeTrackSpline;

    [HideInInspector] public float player1TrackDistance = 0f;
    [HideInInspector] public float player2TrackDistance = 0f;
    [HideInInspector] public bool player1InControl = true;
    [HideInInspector] public float turnTimer = 0f;


    private void OnEnable()
    {
        nativeTrackSpline = new NativeSpline(trackSpilneContainer.Spline, Unity.Collections.Allocator.Persistent);
    }

    private void OnDisable()
    {
        nativeTrackSpline.Dispose();
    }


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

        float3 point;
        if (player1InControl)
        {
            SplineUtility.GetNearestPoint(nativeTrackSpline, player1Car.transform.position, out point, out float distance);
            player1TrackDistance = distance;
        }
        else
        {
            SplineUtility.GetNearestPoint(nativeTrackSpline, player2Car.transform.position, out point, out float distance);
            player2TrackDistance = distance;
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