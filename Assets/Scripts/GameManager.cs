using UnityEngine;

public class GameManager : MonoBehaviour
{
    //A Singleton allows any script to instantly say: GameManager.Instance
    public static GameManager Instance; 

    public enum GamePhase { Preparation, Combat }
    
    [Header("Game State")]
    public GamePhase currentPhase = GamePhase.Preparation;

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null) Instance = this;
    }

    //connect UI "Start" Button to this function!
    public void StartBattle()
    {
        currentPhase = GamePhase.Combat;
        Debug.Log("The Battle Has Begun!");
    }
}
