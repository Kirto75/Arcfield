using UnityEngine;

public class PathNode 
{
   public Vector3Int position;

    //Distance from the start
   public int gCost;

    //Distance to the target (Heuristic)
   public int hCost;
    

    //The node we came from
   public PathNode parentNode ;


   public PathNode(Vector3Int pos)
    {  
        position = pos;
    }

    //F Cost: (g + h)
    public int fCost
    {
        get { return gCost + hCost; }
    }
}
