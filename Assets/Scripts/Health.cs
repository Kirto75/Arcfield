using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Unit Stats")]
    public float maxHealth = 5f;
    private float currentHelth ;
    
    [Header("User Interface")]
    public Slider healthSlider ;
    public Image fillImage ;

    private HeroController heroController;

    public GameManager GameManager;

    void Start()
    {
        // Set health to full at start
        currentHelth = maxHealth ;
        GameManager = GetComponent<GameManager>();
        heroController = GetComponent<HeroController>();
        
        if (healthSlider != null)
        {
            //Set the maxValue of the slider 
            healthSlider.maxValue = maxHealth ;

            //Fill the slider based on current health
            healthSlider.value = currentHelth ;
        }
    }
    public void Update()
    {
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     TakeDamage(1f);
        // }
    }

    public void TakeDamage(float damageAmount)
    {
        //Decrese the health based on the damage
        currentHelth -= damageAmount ;

        //Prevent the health from going negative
        if (currentHelth <= 0 )
        {
            currentHelth = 0;

            //Change the fill color to black so the slider looks empty
            if (fillImage != null)
            {
                fillImage.color = Color.black;
            }

            Die();
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHelth ;
        }

    }
    public void Die()
    {
        if (IsDead) return;

        IsDead = true;

        Debug.Log(gameObject + " has been defeated");

        if (heroController != null)
        {
            heroController.ChangeState(HeroController.HeroState.Dead);
        }

        Grid grid = heroController.grid;

        if (grid != null && heroController.gridData != null)
        {
            Vector3Int cell = grid.WorldToCell(transform.position);

            heroController.gridData.ClearArea(cell, Vector2Int.one);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckForGameOver();
        }
    }

    public bool IsDead { get; private set; }


}
