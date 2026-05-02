using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Camera refrence
    [SerializeField]
    private Camera sceneCamera;

    //Last position the mouse hit
    private Vector3 lastPostion;

    //Limit raycast to speciefic layers
    [SerializeField]
    private LayerMask placementLayermask;
    
    //Check where the mouse points and return world position
    public Vector3 GetSelectedMapPosition()
    {
        //current mouse position on the screen 
        Vector3 mousePos = Mouse.current.position.ReadValue();
        
        //Casts a ray from the camere to the mouse position 
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        //Stores info if ray hits 
        RaycastHit hit ;

        //Cast a ray from(origin , direction, ReycastHit , MaxDistance, Layermask )
        if (Physics.Raycast(ray, out hit , 100 ,placementLayermask))
        {
            //if the ray hits return the position
            lastPostion = hit.point ;
        }
        //if it didn't hit anything return the last position it hit  
        return lastPostion;
    }
}
