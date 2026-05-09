using UnityEngine;

public class ShopUiManager : MonoBehaviour
{
    [SerializeField]
    private PlacementSystem placementSystem;
    
    public void SelectKnight()
    {
        //knight is 0
        placementSystem.StartPlacement(0);
    }
    public void SelectArhcer()
    {
        //Archer is 1
        placementSystem.StartPlacement(1);
    }
    public void SelectMage()
    {
        //Mage is 2
        placementSystem.StartPlacement(2);
    }
    public void SelectRemoveTool()
    {
        placementSystem.StartRemoving();
    }

}
