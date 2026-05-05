using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            OnClicked?.Invoke();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    
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
