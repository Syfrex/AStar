using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    ToGoScript Grid;
    private Node StartNode;
    private Node EndNode;
    private int MoveCost;
    private List<Node> OpenList = new List<Node>();
    List<Node> Finalpath = new List<Node>();
    Node CurrentNode;
    private HashSet<Node> ClosedList = new HashSet<Node>();

    private void Awake()
    {
        Grid = GetComponent<ToGoScript>();    
    }
    private void Update()
    {
        OpenList = new List<Node>();
        ClosedList = new HashSet<Node>();
        AStarPath(Grid.StartObject.transform.position, Grid.EndObject.transform.position);
    }
    void AStarPath(Vector3 StartPos, Vector3 EndPos)
    {
        StartNode = Grid.NodeFromWorldPosition(StartPos);
        OpenList.Add(StartNode);      
        EndNode = Grid.NodeFromWorldPosition(EndPos);
        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ManhattanCost < CurrentNode.ManhattanCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);
            if (CurrentNode == EndNode)
            {
                GetFinalPath(StartNode, EndNode);
                break;
            }
            foreach (Node NeighbourNodes in Grid.GetNodes(CurrentNode))
            {
                if (NeighbourNodes.isWall || ClosedList.Contains(NeighbourNodes))
                {
                    continue;
                }
                MoveCost = CurrentNode.MoveCost + ManhattanDistance(CurrentNode, NeighbourNodes);
                if (MoveCost < NeighbourNodes.MoveCost || !OpenList.Contains(NeighbourNodes))
                {
                    NeighbourNodes.MoveCost = MoveCost;
                    NeighbourNodes.ManhattanCost = ManhattanDistance(NeighbourNodes, EndNode);
                    NeighbourNodes.Parent = CurrentNode;
                    if (!OpenList.Contains(NeighbourNodes))
                    {
                        OpenList.Add(NeighbourNodes);
                    }
                }
            }
        }
    }

    void GetFinalPath(Node StartNode, Node EndNode)
    {
        Finalpath.Clear();
        CurrentNode = EndNode;
        while (CurrentNode != StartNode)
        {
            Finalpath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        Finalpath.Reverse();
        Grid.FinalPath = Finalpath;
    }

    int ManhattanDistance(Node p_NodeA, Node p_NodeB)
    {
        return Mathf.Abs(p_NodeA.GridX - p_NodeB.GridX) + Mathf.Abs(p_NodeA.GridY - p_NodeB.GridY);
    }
}
