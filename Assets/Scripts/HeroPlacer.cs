using System.Collections.Generic;
using UnityEngine;

public class HeroPlacer : MonoBehaviour
{
    //we will use Dictionary instead of list
    //the int is unique ID and the GameObject is the hero 
    private Dictionary<int, GameObject> placedGameObjects = new();

    private int nextSpawnId = 0;

    public int PlaceObject(GameObject prefab, Vector3 position, Grid grid, GridData gridData)
    {
        Debug.Log("Spawning hero");
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        //Hand the grid date to the hero
        HeroController controller = newObject.GetComponent<HeroController>();
        if (controller != null)
        {
            controller.grid = grid;
            controller.gridData = gridData;
        }
        //assign the unique id
        int currentId = nextSpawnId;
        
        //add it to the dictionary
        placedGameObjects.Add(currentId, newObject);

        //increment the unique id to preserve uniqueness
        nextSpawnId++ ;

        //return unique id insetad of list index
        return currentId ;
    }

    public void RemoveObjectAt(int uniqueId)
    {

        //safety check 
        if (placedGameObjects.ContainsKey(uniqueId) == false)
        {
            return;
        }

        Destroy(placedGameObjects[uniqueId]);

        //remove the entry from the dictionary 
        placedGameObjects.Remove(uniqueId);
    }
    //Fetch a spawned unit using its ID
    public GameObject GetPlacedObject(int uniqueId)
    {
        if (placedGameObjects.ContainsKey(uniqueId))
        {
            return placedGameObjects[uniqueId];
        }
        return null;
    }
}
