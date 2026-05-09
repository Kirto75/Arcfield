using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //The game indicators(mouse indicator mesh render is disabled) 
    [SerializeField]
    private GameObject mouseIndecator, cellIndicator;
    
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private HeroesDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    public GridData gridData;
    private Renderer previewRenderer;

    [SerializeField]
    private HeroPlacer heroPlacer;
    IBuildingState buildingState;

    private void Awake()
    {
        StopPlacement();
        gridData = new GridData();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        buildingState = new PlacementState(ID, grid, database, gridData, heroPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        buildingState = new RemovingState(grid, gridData, heroPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
            return;
        
        //Gets lastPosition for the mouse
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        //Gets the cell position where the mouse is
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) 
            return;

        //Gets lastPosition for the mouse 
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        //Gets the cell position where the mouse is  
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.UpdateState(gridPosition, previewRenderer);

        //Moves the indicator to the mouse position 
        mouseIndecator.transform.position = mousePosition;

        //prints the indicator over the tile where the mouse is 
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
