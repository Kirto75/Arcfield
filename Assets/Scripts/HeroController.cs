using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    
    public enum HeroState
    {
        Idle,
        Chase,
        Attack,
        Dead,
    }
    public enum Team { Player, Enemy }
    public Team myTeam = Team.Player;

    [Header("State Machine")]
    //Variables to show the current state 
    public HeroState currentState = HeroState.Idle;

    [Header("Stats")]
    public float moveSpeed = 1f;
    public float attackRange = 1f;

    [Header("Targeting")]
    public Transform currentTarget;

    //Pathfindign variables
    [Header("Grid Movement")]
    public Grid grid;
    public GridData gridData;

    private PathFinder pathfinder;
    private List<Vector3Int> currentPath;

    private Animator anim;
    private Vector3Int currentGridPosition;

    void Start()
    {
        anim = GetComponent<Animator>();

        pathfinder = new PathFinder();
        if(grid != null)
        {
            currentGridPosition = grid.WorldToCell(transform.position);
        }
    }

    void Update()
    {
        switch(currentState)
        {
            //Use Method for every state to handle the animator and any thing special to that state

            case HeroState.Idle:
            UpdateIdleState();
            break;

            case HeroState.Chase:
            UpdateChaseState();
            break;

            case HeroState.Attack:
            UpdateAttackState();
            break;

            case HeroState.Dead:
            //Do nothing because its dead ^_^
            break;
            
        }
    }
    public void ChangeState(HeroState newState)
    {   
        //Update the state to new one
        currentState = newState ;


        //Handle animations
        if (currentState == HeroState.Chase)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if(currentState == HeroState.Dead)
        {
            anim.SetBool("isDead",true);
        }
    }

    public void UpdateIdleState()
    {   
        if (GameManager.Instance.currentPhase == GameManager.GamePhase.Combat)
        {
            if (currentTarget == null)
            {
                // No target? Scan the board!
                FindClosestEnemy();
            }
            else
            {
                // Found one! Start running.
                ChangeState(HeroState.Chase);
            }
        }
        
    }
    public void UpdateChaseState()
    {
        //  Safety Check: Did the target die or disappear?
        if (currentTarget == null)
        {
            ChangeState(HeroState.Idle);
            return;
        }

        //  Are we close enough to attack?
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget <= attackRange)
        {
            ChangeState(HeroState.Attack);
            currentPath = null; // Clear the path so we calculate a fresh one next time we move
            return;
        }

        //  If we don't have a path, calculate one!
        if (currentPath == null || currentPath.Count == 0)
        {
            // Convert our real 3D positions into Grid Tile coordinates (e.g., [2, 4])
            Vector3Int myCell = grid.WorldToCell(transform.position);
            Vector3Int targetCell = grid.WorldToCell(currentTarget.position);
            
            // Ask the Pathfinder brain for a route using YOUR grid dictionary!
            currentPath = pathfinder.FindPath(myCell, targetCell, gridData);
        }

        //  Move along the path
        if (currentPath != null && currentPath.Count > 0)
        {
            // Find the exact 3D center of the first tile in our list
            Vector3 nextTileWorldPos = grid.GetCellCenterWorld(currentPath[0]);
            nextTileWorldPos.y = transform.position.y; // Keep Y the same so we don't float

            // Move towards that specific tile
            transform.position = Vector3.MoveTowards(transform.position, nextTileWorldPos, moveSpeed * Time.deltaTime);

            // Rotate to look at the tile we are walking towards
            Vector3 lookDirection = (nextTileWorldPos - transform.position).normalized;
            lookDirection.y = 0; 
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            //  Did we reach the center of this tile? 
            // If yes, remove it from the list so the next frame we start walking to the next tile!
            if (Vector3.Distance(transform.position, nextTileWorldPos) < 0.1f)
            {
                // with the GridData dictionary
                Vector3Int targetTile = currentPath[0];
                
                // Tell the dictionary to empty our old tile and block the new one
                gridData.MoveObject(currentGridPosition, targetTile, Vector2Int.one);
                
                // Update our tracker to the new tile
                currentGridPosition = targetTile;

                currentPath.RemoveAt(0);
            }
        }
    }
    public void UpdateAttackState()
    {
        //the target died?
        if (currentTarget == null)
        {
            ChangeState(HeroState.Idle);
            return;
        }
        anim.SetTrigger("isAttacking");
    }

    public void FindClosestEnemy()
    {
        // Find every single hero currently on the board
        HeroController[] allHeroes = FindObjectsByType<HeroController>(FindObjectsSortMode.None);
        
        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (HeroController hero in allHeroes)
        {
            // Skip heroes that are already dead, or skip ourselves
            if (hero == this || hero.currentState == HeroState.Dead) continue;

            // Skip heroes on the same team!
            if (hero.myTeam == this.myTeam) continue;

            // Measure distance to this valid enemy
            float distance = Vector3.Distance(transform.position, hero.transform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = hero.transform;
            }
        }

        // Lock onto the closest valid enemy
        currentTarget = bestTarget;
    }



}

