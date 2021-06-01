using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigurations;

    [SerializeField]
    private int MaxPlayer = 4;

    public static PlayerControllerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigurations = new List<PlayerConfiguration>();
            Debug.Log("Instantiate player configurations as empty list.");
        }
    }

    public void SetPlayerMaterial(int index, Material material)
    {
        playerConfigurations[index].PlayerMaterial = material;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigurations[index].IsReady = true;
        // Everyone is ready!
        if (playerConfigurations.Count == MaxPlayer && playerConfigurations.All(player => player.IsReady))
        {
            SceneManager.LoadScene("Swab Test");
        }
    }

    public void OnPlayerJoin(PlayerInput playerInput) 
    {
        Debug.Log("Player Joined: " + playerInput.playerIndex);
        if (!playerConfigurations.Any(player => player.PlayerIndex == playerInput.playerIndex))
        {
            //playerInput.transform.SetParent(transform);
            playerConfigurations.Add(new PlayerConfiguration(playerInput));
        }
    }

    public PlayerConfiguration GetConfiguration(int index)
    {
        return playerConfigurations[index];
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}