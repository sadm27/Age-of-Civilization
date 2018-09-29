using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TileMap : MonoBehaviour {

    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;    //tile types
    Node[,] graph;  //who every tile is touchiung


    

    int MapSizeX = 10;
    int MapSizeY = 10;

    void Start()
    {
        generateMap();
        generateGraphHelp();
        generateMapVisuals();
    }





    void generateMap()
    {
        //allocation of map tiles
        tiles = new int[MapSizeX, MapSizeY];



        //initialize map tiles
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        tiles[4, 4] = 2;
        tiles[5, 4] = 1;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;

        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }

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


    void generateGraphHelp()
    {
        //initilize the graph
        graph = new Node[MapSizeX, MapSizeY];


        //initalize a spot for each node
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                graph[x, y] = new Node();


                graph[x, y].NodeX = x;
                graph[x, y].NodeY = y;
            }
        }


        //neighbor calc
        for (int x = 0; x < MapSizeX; x++)
         {
             for (int y = 0; y < MapSizeY; y++)
              {
                

                if (y < MapSizeY - 1 && x > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y +1]);  //top left
                }
                if (y < MapSizeY - 1)
                {
                    graph[x, y].neighbors.Add(graph[x, y + 1]); //top center
                }
                if (y < MapSizeY - 1 && x < MapSizeX - 1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y + 1]);  //top right
                }
                if (x > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y]); //center left
                }
                if (x < MapSizeX - 1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y]); //center right
                }
                if (y > 0 && x > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y - 1]); //bottem left
                }
                if (y > 0)
                {
                    graph[x, y].neighbors.Add(graph[x, y - 1]); //bottem center
                }
                if (y > 0 && x < MapSizeX - 1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y - 1]);  //top left
                }



            }
        }

    }


    //creates tiles visually on the map
        void generateMapVisuals()
    {

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                //tile type of the tiles coordinace is set so the tile can call the correct visual prefab

                GameObject go = (GameObject)Instantiate(tt.TileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );
                //the game object is initalized and created at set coordinace

                tileClicker CT = go.GetComponent<tileClicker>();
                //gets components of the tile game object and sets them to the tile clicker to help that

                CT.Xtile = x;
                CT.Ytile = y;
                CT.map = this;
            }
        }

    }

    //get the tile coordinace on the map and relates that to the worlds coordinace
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    //sets the units data on what tile it is on and then set it up visually
    public void MoveSelectedUnitTo(int x, int y)
    {
        //clear path
        selectedUnit.GetComponent<Unit>().CurrPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //setting up the unvisited nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            selectedUnit.GetComponent<Unit>().Xtile, 
                            selectedUnit.GetComponent<Unit>().Ytile
                            ];

        Node target = graph[x, y];


        dist[source] = 0;
        prev[source] = null;

        //initialize everything to have an infinete distance since we don't know right now also possible that you can't reach some nodes from the source
        //which makes infinity valid
        foreach (Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            //u is unvisited node with the smallest distance
            Node u = null;

            //helps find the unvisited node with the smallest distance
            foreach(Node PossU in unvisited)
            {
                if(u == null || dist[PossU] < dist[u])
                {
                    u = PossU;
                }
            }

            if( u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbors)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if(alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        //here is shortest route found or no route found

        if(prev[target] == null)
        {
            //no route
            return;
        }

        List<Node> CurrPath = new List<Node>();
        Node curr = target;

        //flip prev list to create path
        while(curr != null)
        {
            CurrPath.Add(curr);
            curr = prev[curr];
        }

        CurrPath.Reverse();

        selectedUnit.GetComponent<Unit>().CurrPath = CurrPath;

    }

}
