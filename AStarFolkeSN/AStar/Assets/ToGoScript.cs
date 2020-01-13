using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToGoScript : MonoBehaviour
{
    public LayerMask WallMask;
    private Vector2 gridWorldSize = new Vector2(30, 30);
    private float nodeRaidus = 0.5f;
    private float Distance = 0.0f;
    Node[,] grid;
    [HideInInspector]
    public List<Node> FinalPath = new List<Node>();
    private List<Vector3> nodePosition = new List<Vector3>();
    List<Node> NeighbouringNodes = new List<Node>();
    [HideInInspector]
    public GameObject StartObject;
    [HideInInspector]
    public GameObject EndObject;

    public float UpdatePosition = 50f;
    
    float nodeDiameter;
    int GridSizeX, GridSizeY;
    int xCheck, yCheck;
    int randPos = 0;

    private void Start()
    {
        StartObject = Resources.Load("StartPos") as GameObject ;
        EndObject = Resources.Load("EndPos") as GameObject;
        StartObject = Instantiate<GameObject>(StartObject, Vector3.zero, transform.rotation);
        EndObject = Instantiate<GameObject>(EndObject, Vector3.zero, transform.rotation);
        
        nodeDiameter = nodeRaidus + nodeRaidus;
        GridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        GridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        InvokeRepeating("UpdateLocation", UpdatePosition, UpdatePosition);

        CreateGrid();
        StartObject.transform.position = RandomPosition();
        EndObject.transform.position = RandomPosition();
    }
    void UpdateLocation()
    {
        StartObject.transform.position = RandomPosition();
        EndObject.transform.position = RandomPosition();
    }
    Vector3 RandomPosition()
    {
        if (nodePosition.Count < 1)
        {
            foreach (Node node in grid)
            {
                if (!node.isWall)
                    nodePosition.Add(node.Position);
            }
        }
        randPos = Random.Range(0, nodePosition.Count);

        return nodePosition[randPos];
    }
    void CreateGrid()
    {
        grid = new Node[Mathf.RoundToInt(GridSizeX), Mathf.RoundToInt(GridSizeY)];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRaidus) + Vector3.forward * (y * nodeDiameter + nodeRaidus);
                bool Wall = false;

                if (Physics.CheckSphere(worldPoint, nodeRaidus, WallMask))
                {
                    Wall = true;
                }
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }
    bool CheckNextNode(int gridX, int gridY)
    {
        if (gridX >= 0 && gridX < GridSizeX)
        {
            if (gridY >= 0 && gridY < GridSizeY)
            {
                return true;
            }
        }
        return false;
    }
    public List<Node> GetNodes(Node p_Node)
    {
        NeighbouringNodes.Clear();
        xCheck = p_Node.GridX + 1;
        yCheck = p_Node.GridY;
        if(CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX - 1;
        yCheck = p_Node.GridY;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX + 1;
        yCheck = p_Node.GridY + 1;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX + 1;
        yCheck = p_Node.GridY - 1;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX - 1;
        yCheck = p_Node.GridY + 1;

        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }
        xCheck = p_Node.GridX - 1;
        yCheck = p_Node.GridY - 1;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX;
        yCheck = p_Node.GridY + 1;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        xCheck = p_Node.GridX;
        yCheck = p_Node.GridY - 1;
        if (CheckNextNode(xCheck, yCheck))
        {
            NeighbouringNodes.Add(grid[xCheck, yCheck]);
        }

        return NeighbouringNodes;
    }


    public Node NodeFromWorldPosition(Vector3 p_worldPos)
    {
        return grid[Mathf.RoundToInt((GridSizeX - 1) * ((p_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x)),
            Mathf.RoundToInt((GridSizeY - 1) * ((p_worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y))];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }
                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))
                    {
                        Gizmos.color = Color.red;
                    }
                }
                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - Distance));
            }
        }

    }
}
