using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Tile
{
    public enum Type
    {
        grassland,
        mountain,
        marsh,
        water
    }

    public enum Resource
    {
        wood,
        food,
        stone
    }

}

public class TileMap : MonoBehaviour {

    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;    //tile types
    Node[,] graph;  //who every tile is touchiung

    Tile[,] map;

    

    int MapSizeX = 20;
    int MapSizeY = 20;

    //200 by 200 test

    void Start()
    {
        //selectedUnit.GetComponent<Unit>().Xtile = selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().Xtile = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().Ytile = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        generateMap();
        generateGraphHelp();
        generateMapVisuals();
    }





    void generateMap()
    {
        //allocation of map tiles
        map = new Tile[MapSizeX, MapSizeY];


        
        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                float height = GetHeight(x, y);
                if (height < .35)
                {
                    map[x, y] = 3;
                }
                else if (height < .4)
                {
                    tiles[x, y] = 1;
                }
                else if (height < .7)
                {
                    tiles[x, y] = 0;
                }
                else { tiles[x, y] = 2; }
            }
        }
        

        /*
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

        */
    }


    

    float GetHeight(int x, int y)
    {
        float xCoords = (float)x / MapSizeX * 10;
        float yCoords = (float)y / MapSizeY * 10;

        float sample = Mathf.PerlinNoise(xCoords, yCoords);
        return sample;
    }

    



    public float CostToEnterTile(int sourcex, int sourceY, int targetX, int targetY)
    {
        //for civ 5 cost into hills method where a unit has any movment points left
        //they can move into the hill tiles you would calculate that here
        TileType tt = tileTypes[tiles[targetX, targetY]];

        if(EnterTileCheck(targetX, targetY) == false)
        {
            return Mathf.Infinity;
        }

        float cost = tt.moveCost;

        if(sourcex != targetX && sourceY != targetY)
        {
            //diagonal moves makes diagonals cost more getting rid of stupid zig zags
            cost += 0.001f;

        }

        return cost;
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

    //get the tile coordinace on the map and relates that to the worlds coordinace  STATIC CAN BE ADDED IN TO HANDLE LARGE SCALLING OF MAP IF PROBLEMS HAPPEN REMOVE IT
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }





    public bool EnterTileCheck(int x, int y)
    {
        //test unit type to trerrain

        return tileTypes[ tiles[x,y] ].isWalkable;
    }



    //sets the units data on what tile it is on and then set it up visually
    //Dijkstra's algorithm is used for the pathfinding A* search algorithm is another more commonly used pathfinging algorithm that goes in a general direction and is quicker
    //Dijkstra's was used because I did just under a year ago in COMP250
    public void MoveSelectedUnitTo(int x, int y)
    {
        //clear path
        selectedUnit.GetComponent<Unit>().CurrPath = null;


        if(EnterTileCheck(x, y) == false)
        {
            //mountain click or non-move tile click
            return;
        }


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
                //float alt = dist[u] + u.DistanceTo(v);    also NodeX = x  NodeY = y
                float alt = dist[u] + CostToEnterTile(u.NodeX, u.NodeY, v.NodeX, v.NodeY);
                if (alt < dist[v])
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
