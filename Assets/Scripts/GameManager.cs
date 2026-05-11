using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //A Singleton allows any script to instantly say: GameManager.Instance
    public static GameManager Instance; 

    public enum GamePhase { Preparation, Combat }

    public AudioSource audioSource;
    public AudioClip win_sound, lose_sound, combat_sound;
    
    [Header("Game State")]
    public GamePhase currentPhase = GamePhase.Preparation;

    [SerializeField]
    public GameObject winPanel, losePanel;

    public bool game_on = true;

    public void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame && game_on)
            Win();
        if (Keyboard.current.lKey.wasPressedThisFrame && game_on)
            Lose();
    }

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null) Instance = this;
    }

    //connect UI "Start" Button to this function!
    public void StartBattle()
    {
        StartCoroutine(battle());
    }

    public void Win()
    {
        game_on = false;
        audioSource.Stop();
        audioSource.clip = win_sound;
        audioSource.Play();
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("You won!");
    }

    public void Lose()
    {
        game_on = false;
        audioSource.Stop();
        audioSource.clip = lose_sound;
        audioSource.Play();
        losePanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("You lose :(");
    }

    IEnumerator battle()
    {
        Debug.Log("Battle sound!!!");
        audioSource.Stop();
        audioSource.clip = combat_sound;
        audioSource.Play();
        yield return new WaitForSeconds(combat_sound.length-2);
        currentPhase = GamePhase.Combat;
        Debug.Log("The Battle Has Begun!");
    }

    public void CheckForGameOver()
    {
        HeroController[] allHeroes = FindObjectsByType<HeroController>(FindObjectsSortMode.None);

        int playerCount = 0;
        int enemyCount = 0;

        foreach (HeroController hero in allHeroes)
        {
            // Ignore dead heroes
            if (hero.currentState == HeroController.HeroState.Dead)
                continue;

            if (hero.myTeam == HeroController.Team.Player)
            {
                playerCount++;
            }
            else if (hero.myTeam == HeroController.Team.Enemy)
            {
                enemyCount++;
            }
        }

        // No enemies left -> player wins
        if (enemyCount <= 0 && game_on)
        {
            Win();
        }

        // No players left -> player loses
        if (playerCount <= 0 && game_on)
        {
            Lose();
        }
    }
}
