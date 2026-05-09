using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refrences")]
    public PlacementSystem placementSystem;
    public HeroPlacer heroPlacer;
    public Grid grid;
    public HeroesDatabaseSO heroesDatabaseSO;

    [Header("Spawn Settings")]
    public List<int> allowedEnemyIDs = new List<int> { 0, 1, 2 };
    public int numberOfEnemies = 4;

    [Header("Board Limits (Enemy Side)")]
    public int minX = 0;
    public int maxX = 7;
    public int minZ = 0;
    public int maxZ = 7;

    void Start()
    {
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
            SpawnRandomEnemies();
        // }
    }

    public void SpawnRandomEnemies()
    {
        GridData gridData = placementSystem.gridData;
        if (gridData == null)
        {
            Debug.LogWarning("GridData is not ready yet!");
            return;
        }

        int spawnedCount = 0;
        int safetyNet = 0; 

        while (spawnedCount < numberOfEnemies && safetyNet < 100)
        {
            safetyNet++;

            // Pick a random grid coordinate
            int randomX = Random.Range(minX, maxX + 1);
            int randomZ = Random.Range(minZ, maxZ + 1);
            Vector3Int randomPos = new Vector3Int(randomX, 0, randomZ);

            //  Roll a random ID from our allowed list! ---
            int randomListIndex = Random.Range(0, allowedEnemyIDs.Count);
            int chosenEnemyID = allowedEnemyIDs[randomListIndex];

            // Find the exact prefab for the chosen ID
            int enemyIndex = heroesDatabaseSO.objectsData.FindIndex(data => data.ID == chosenEnemyID);
            if (enemyIndex < 0) continue; // If ID is invalid, try again
            
            GameObject prefab = heroesDatabaseSO.objectsData[enemyIndex].Prefab;
            Vector2Int size = heroesDatabaseSO.objectsData[enemyIndex].Size;
            // ----------------------------------------------------

            // Ask the Dictionary: Is this tile empty?
            if (gridData.CanPlaceObjectAt(randomPos, size))
            {
                Vector3 cellCenter = grid.GetCellCenterWorld(randomPos);
                Vector3 cellBase = grid.CellToWorld(randomPos);
                Vector3 spawnPosition = new Vector3(cellCenter.x, cellBase.y, cellCenter.z);

                // Spawn the physical object
                int placedId = heroPlacer.PlaceObject(prefab, spawnPosition, grid, gridData);

                // Grab the spawned enemy and rotate it 180
                GameObject spawnedEnemy = heroPlacer.GetPlacedObject(placedId);
                if (spawnedEnemy != null)
                {
                    spawnedEnemy.transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
                //Assign the unit to enemy team
                spawnedEnemy.GetComponent<HeroController>().myTeam = HeroController.Team.Enemy;

                // Register it in the Dictionary!
                gridData.AddObjectAt(randomPos, size, chosenEnemyID, placedId);

                spawnedCount++;
            }
        }
        
        Debug.Log("Successfully spawned " + spawnedCount + " random enemies!");
    }
}
