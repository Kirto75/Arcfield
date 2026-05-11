using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) 
                Debug.LogError($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }   
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacementObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
    public void MoveObject(Vector3Int currentGridPosition, Vector3Int newGridPosition, Vector2Int objectSize)
    {
        //make sure there is an object on the starting tile 
        if (placedObjects.ContainsKey(currentGridPosition) == false)
        {
            Debug.LogWarning($"No object found at {currentGridPosition} to move ");
            return;
        }


        //save the data of the object we are going to move 
        PlacementData dataToMove = placedObjects[currentGridPosition];

        //clear its old position form the dictionary
        RemoveObjectAt(currentGridPosition);

        //calculate the new position
        List<Vector3Int> newPositionToOccupy = CalculatePositions(newGridPosition, objectSize);

        //update the data's internal list
        dataToMove.occupiedPositions = newPositionToOccupy;


        //register the object in the dictionary at its new position(to block those tiles)
        foreach (var pos in newPositionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                Debug.Log($"Dictionary already contains this cell position {pos} while moving");
            }
            placedObjects[pos] = dataToMove;
        }

    }

        public void ClearArea(Vector3Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int tilePosition = position + new Vector3Int(x, 0, y);

                if (placedObjects.ContainsKey(tilePosition))
                {
                    placedObjects.Remove(tilePosition);
                }
            }
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacementObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.ID = ID;
        PlacementObjectIndex = placedObjectIndex;
    }
}
