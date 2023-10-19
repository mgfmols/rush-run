using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum Form
{
    Corner,
    Straight,
    Junction,
    T_Junction,
    Empty
}

[Serializable]
public class RemovableEdge
{
    public Direction direction;
    public GameObject edge;

    public static Direction LoopDirectionIfNecessary(Direction direction)
    {
        int enumLength = Enum.GetValues(typeof(Direction)).Length;
        int amountExceeded = (int) direction / enumLength;
        if (amountExceeded > 0)
        {
            direction -= Enum.GetValues(typeof(Direction)).Length * amountExceeded;
        }
        return direction;
    }
}

public class RandomizedObject : MonoBehaviour
{
    [SerializeField] public Form form;
    [SerializeField] public List<GameObject> environmentAddons;
    [SerializeField] public List<RemovableEdge> removableEdges;

    public void rotateEdges(int times)
    {
        if (removableEdges.Count > 0)
        {
            foreach (RemovableEdge edge in removableEdges)
            {
                edge.direction = RemovableEdge.LoopDirectionIfNecessary(edge.direction += times);
            }
        }
    }

    // Search through all edges to remove an edge in a given direction
    public void removeEdge(Direction direction)
    {
        RemovableEdge toBeRemoved = new RemovableEdge();
        foreach(RemovableEdge edge in removableEdges)
        {
            if (direction.Equals(edge.direction))
            {
                toBeRemoved = edge;
            }
        }

        // Checks if an edge on that direction is present
        if (toBeRemoved.edge != null)
        {
            Destroy(toBeRemoved.edge);
        }
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(28, 1, 28));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(30, 1, 30));
    }*/
}
