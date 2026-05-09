using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinder 
{   
    //Pass startposition, tragetPosition and gridDate so the algortithm now the walls and other units
    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos, GridData gridData)
    {
        //OpenSet : Tiles we want to evaluate
        List<PathNode> openSet = new List<PathNode>();

        //ClosedSet : Tiles we already checked (hash is fast for checking true fasle)
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();


        //A dictionary to quickly find our temporary PathNodes by their position
        Dictionary<Vector3Int, PathNode> nodeLookup = new Dictionary<Vector3Int, PathNode>();



        PathNode startNode = new PathNode(startPos);
        PathNode targetNode = new PathNode(targetPos);

        openSet.Add(startNode);
        nodeLookup.Add(startPos, startNode);

        while (openSet.Count > 0)
        {   
            //Find the tile in the openSet with the lowest cose (fcost)
            PathNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //Check the fCost if they are equal we determine by hCost
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            //Move current tile from open to closed
            openSet.Remove(currentNode);
            closedSet.Add(currentNode.position);

            //Check if we reached the target
            if(currentNode.position == targetPos)
            {
                return RetracePath(startNode, currentNode);
            }
            

            //check all surrounding tiles
            foreach(Vector3Int neighborPos in  GetNeighbors(currentNode.position))
            {
                //check if the tile is blocked
                bool isBlocked = !gridData.CanPlaceObjectAt(neighborPos, Vector2Int.one);

                //allow evaluating the target tile even if its blocked
                if (closedSet.Contains(neighborPos) || (isBlocked && neighborPos != targetPos))
                {
                    continue; //skip this neighbor because its either wall or already checked 
                }


                //
                int moveCostToNeighbor = currentNode.gCost + GetDistance(currentNode.position, neighborPos);
                

                PathNode neighborNode;
                if(!nodeLookup.TryGetValue(neighborPos, out neighborNode))
                {
                    neighborNode = new PathNode(neighborPos);
                    nodeLookup.Add(neighborPos, neighborNode);
                }

                // If this path is shorter, or we haven't evaluated this neighbor yet
                if (moveCostToNeighbor < neighborNode.gCost || !openSet.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborPos, targetPos);
                    neighborNode.parentNode = currentNode; // Leave a breadcrumb!

                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                    }
                }
            }

        }
        // If the loop finishes and we are here, the target is completely trapped 
        Debug.LogWarning("Pathfinder: No valid path found to target!");
        return null;

    }

    // Walk backward from the target to the start using the parent
    private List<Vector3Int> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        PathNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parentNode;
        }

        path.Reverse(); // we trace backward ,so flip it
        return path;
        
    }   
    //Calculate distance between two tiles (diagonal allowed)
    private int GetDistance(Vector3Int nodeA, Vector3Int nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.z);
        int dstZ = Mathf.Abs(nodeA.z - nodeB.x);

        // 14 is the math cost of diagonal, 10 is the math cost of straight lines
        if(dstX > dstZ)
        {
            return 14 * dstZ + 10 * (dstX - dstZ);
        }
        return 14 * dstZ + 10 * (dstX - dstZ);
    }



    //Get the 4 surrounding tiles
    private List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        

        // Right & Left
        neighbors.Add(new Vector3Int(pos.x + 1, 0, pos.z)); 
        neighbors.Add(new Vector3Int(pos.x - 1, 0, pos.z)); 
        
        // Forward & Backward
        neighbors.Add(new Vector3Int(pos.x, 0, pos.z + 1)); 
        neighbors.Add(new Vector3Int(pos.x, 0, pos.z - 1)); 

        return neighbors;
        
    }
}
