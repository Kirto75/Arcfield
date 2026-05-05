using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    GridData gridData;
    HeroPlacer heroPlacer;

    public RemovingState(Grid grid, GridData gridData, 
                         HeroPlacer heroPlacer)
    {
        this.grid = grid;
        this.gridData = gridData;
        this.heroPlacer = heroPlacer;
    }
    
    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (gridData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false) 
        {
            selectedData = gridData;
        }
        
        if (selectedData == null)
        {
            // sound
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
            heroPlacer.RemoveObjectAt(gameObjectIndex);
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int gameObjectIndex)
    {
        // for scalling purposes...
        return !gridData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
    }

    public void UpdateState(Vector3Int gridPosition, Renderer previewRenderer)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, gameObjectIndex);
        previewRenderer.material.color = placementValidity? Color.red: Color.white;        
    }

    public void EndState()
    {
        
    }
}
