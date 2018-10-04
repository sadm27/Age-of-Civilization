using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> neighbors;
    public int NodeX;
    public int NodeY;


    //just initilizes list
    public Node()
    {
        neighbors = new List<Node>();
    }

    public float DistanceTo(Node n)
    {


        return Vector2.Distance(new Vector2(NodeX, NodeY), new Vector2(n.NodeX, n.NodeY));
    }

}

