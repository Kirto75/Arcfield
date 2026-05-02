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

    private void Update()
    {
        //Gets lastPosition for the mouse 
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        //Gets the cell position where the mouse is  
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //Moves the indicator to the mouse position 
        mouseIndecator.transform.position = mousePosition;

        //prints the indicator over the tile where the mouse is 
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
