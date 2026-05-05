using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    HeroesDatabaseSO database;
    GridData gridData;
    HeroPlacer heroPlacer;

    public PlacementState(int id, Grid grid, HeroesDatabaseSO database, 
                          GridData gridData, HeroPlacer heroPlacer)
    {
        ID = id;
        this.grid = grid;
        this.database = database;
        this.gridData = gridData;
        this.heroPlacer = heroPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
            throw new System.Exception($"No object with ID {id}");
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        //generates a new instance of the selected hero
        int index = heroPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0? gridData : gridData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // for scalling purposes...
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0? gridData : gridData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition, Renderer previewRenderer)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRenderer.material.color = placementValidity? Color.green: Color.white;        
    }

    public void EndState()
    {
        
    }
}
