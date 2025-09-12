using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private ArcadeCarController car;
    [SerializeField] private float maxTurnTime = 15f;

    private bool player1InControl = true;
    private float turnTimer = 0f;
    private PlayerData player1Data = new();
    private PlayerData player2Data = new();


    void Start()
    {
        turnTimer = maxTurnTime;
        UpdatePlayerData(player1Data);
        UpdatePlayerData(player2Data);
    }

    // Update is called once per frame
    void Update()
    {
        turnTimer -= Time.deltaTime;
        if (turnTimer <= 0f)
        {
            SwitchPlayer();
        }
    }


    private void SwitchPlayer()
    {
        if (player1InControl == true)
        {
            player1InControl = false;
            UpdatePlayerData(player1Data);
            SetPlayerData(player2Data);
        }
        else
        {
            player1InControl = true;
            UpdatePlayerData(player2Data);
            SetPlayerData(player1Data);
        }
        turnTimer = maxTurnTime;
    }


    private void SetPlayerData(PlayerData playerData)
    {
        car.transform.position = playerData.position;
        car.transform.rotation = playerData.rotation;
        car.rb.linearVelocity = playerData.linearVelocity;
        car.rb.angularVelocity = playerData.angularVelocity;
    }


    private void UpdatePlayerData(PlayerData playerData)
    {
        Rigidbody rb = car.GetComponent<Rigidbody>();
        playerData.linearVelocity = rb.linearVelocity;
        playerData.angularVelocity = rb.angularVelocity;
        playerData.position = car.transform.position;
        playerData.rotation = car.transform.rotation;
    }
}

class PlayerData
{
    public Vector3 linearVelocity;
    public Vector3 angularVelocity;
    public Vector3 position;
    public Quaternion rotation;
}
