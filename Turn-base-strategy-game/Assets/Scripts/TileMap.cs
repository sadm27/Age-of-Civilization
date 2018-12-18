using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticClass { //so game settings can be carried over to the main scene
    public static int chosenMapSizeX {get; set;}
    public static int chosenMapSizeY {get; set;}

}

public class Tile
{
    public enum tileType
    {
        Grassland,
        Marsh,
        Mountain,
        Water,
        Plains,
        Desert
    }
    public enum tileResource
    {
        Wood,
        Stone,
        Food,
        Nothing,
        Gold,
        WatFood
    }

    public tileType type;
    public tileResource resource;
    public int amountOfResource;
    public bool isWalkable;

}

public class TileMap : MonoBehaviour {

    //public GameObject selectedUnit;
    public TileType[] tileTypes;
    public TileResource[] tileResource;


    public Tile[,] map;


    int[,] tiles;    //tile types
    Node[,] graph;  //who every tile is touchiung


    MouseManagerS mouseManagerS;
    

    public int MapSizeX;
    public int MapSizeY;

    void Start()
    {
        if(StaticClass.chosenMapSizeX != 0 && StaticClass.chosenMapSizeY != 0)
        {
            MapSizeX = StaticClass.chosenMapSizeX;
            MapSizeY = StaticClass.chosenMapSizeY;
        }


        
        mouseManagerS = GameObject.Find("MouseManager").GetComponent<MouseManagerS>();
        generateMap();
        generateGraphHelp();
        generateMapVisuals();
        generateSplat(map, MapSizeX, MapSizeY);
        spawnStartingCities();
    }

    void spawnStartingCities()
    {
        if(MapSizeX <= 20)
        {
            smallCitySpawn();
        }
        else
        {
            bigCitySpawn();
        }
    }

    void smallCitySpawn()
    {
        int firstCityX = 0;
        int firstCityY = 0;
        while (true)
        {
            int randX = Random.Range(0, MapSizeX - 1);
            int randY = Random.Range(0, MapSizeY - 1);
            if(pointsForArea(randX, randY) >= 7)
            {
                GameObject cityGO = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/City"), new Vector3(randX, randY, 0), Quaternion.identity);
                assignTags(1, cityGO);
                firstCityX = randX;
                firstCityY = randY;
                break;
            }
        }
        while (true)
        {
            int randX = Random.Range(0, MapSizeX - 1);
            int randY = Random.Range(0, MapSizeY - 1);
            if(tooClose(firstCityX, firstCityY, randX, randY, 5))
            {
                continue;
            }
            if (pointsForArea(randX, randY) >= 7)
            {
                GameObject cityGO = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/City"), new Vector3(randX, randY, 0), Quaternion.identity);
                assignTags(0, cityGO);
                GameObject FoW = cityGO.transform.Find("FoW").gameObject;
                GameObject mesh = cityGO.transform.Find("Cylinder").gameObject;
                mesh.GetComponent<Renderer>().enabled = false;
                FoW.GetComponent<Renderer>().enabled = false;
                mesh.GetComponent<FogOfWarSight>().enabled = false;
                mesh.GetComponent<FogOfWarVisibility>().enabled = true;
                break;
            }
        }
    }


    void bigCitySpawn()
    {
        int firstCityX = 0;
        int firstCityY = 0;
        while (true)
        {
            int randX = Random.Range(0, MapSizeX - 1);
            int randY = Random.Range(0, MapSizeY - 1);
            if (pointsForArea(randX, randY) >= 20)
            {
                GameObject cityGO = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/City"), new Vector3(randX, randY, 0), Quaternion.identity);
                assignTags(1, cityGO);
                firstCityX = randX;
                firstCityY = randY;
                break;
            }
        }
        while (true)
        {
            int randX = Random.Range(0, MapSizeX - 1);
            int randY = Random.Range(0, MapSizeY - 1);
            if (tooClose(firstCityX, firstCityY, randX, randY, 9))
            {
                continue;
            }
            if (pointsForArea(randX, randY) >= 20)
            {
                GameObject cityGO = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/City"), new Vector3(randX, randY, 0), Quaternion.identity);
                assignTags(0, cityGO);
                GameObject FoW = cityGO.transform.Find("FoW").gameObject;
                GameObject mesh = cityGO.transform.Find("Cylinder").gameObject;
                mesh.GetComponent<Renderer>().enabled = false;
                FoW.GetComponent<Renderer>().enabled = false;
                mesh.GetComponent<FogOfWarSight>().enabled = false;
                mesh.GetComponent<FogOfWarVisibility>().enabled = true;
                break;
            }
        }
    }

    int pointsForArea(int mapX, int mapY)
    {
        int pointscore = 0;
        if(map[mapX,mapY].type == Tile.tileType.Mountain || map[mapX, mapY].type == Tile.tileType.Water || map[mapX, mapY].type == Tile.tileType.Marsh)
        {
            return 0;
        }
        if(MapSizeX <= 20)
        {
            for (int i = mapX - 1; i <= (mapX + 1); i++)
            {
                for (int j = mapY - 1; j <= (mapY + 1); j++)
                {
                    if (i == -1 || i == MapSizeX || j == -1 || j == MapSizeY)
                    {
                        continue;
                    }
                    else
                    {
                        if (map[i, j].type == Tile.tileType.Grassland || map[i, j].type == Tile.tileType.Desert || map[i, j].type == Tile.tileType.Plains)
                        {
                            pointscore += 1;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = mapX - 2; i <= (mapX + 2); i++)
            {
                for (int j = mapY - 2; j <= (mapY + 2); j++)
                {
                    if (i == -1 || i == MapSizeX || j == -1 || j == MapSizeY)
                    {
                        continue;
                    }
                    else
                    {
                        if (map[i, j].type == Tile.tileType.Grassland || map[i, j].type == Tile.tileType.Desert || map[i, j].type == Tile.tileType.Plains)
                        {
                            pointscore += 1;
                        }
                    }
                }
            }
        }
        return pointscore;
    }

    bool tooClose(int firstX, int firstY, int secondX, int secondY, int perimeter)
    {
        if(secondX <= (firstX+perimeter) && secondX >= (firstX - perimeter))
        {
            if (secondY <= (firstY + perimeter) && secondY >= (firstY - perimeter))
            {
                return true;
            }
        }
        return false;
    }

    Texture whichSplat(int texture)
    {
        switch (texture)
        {
            case 5:
                return Resources.Load<Texture>("Tiles/MountainText");

            case 4:
                return Resources.Load<Texture>("Tiles/GrassText");

            case 3:
                return Resources.Load<Texture>("Tiles/PlainsText");

            case 2:
                return Resources.Load<Texture>("Tiles/SandText");

            case 1:
                return Resources.Load<Texture>("Tiles/DeepText");

            default:
                return null;
        }
    }

    void generateSplat(Tile[,] map, int sizeX, int sizeY)
    {
        int rightSide;
        int leftSide;
        int upSide;
        int downSide;
        int currentTile;

        //0 = shallow, 1 = deep, 2 = desert, 3 = plains, 4 = grassland, 5 = mountain, 6 = nothing

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                rightSide = getTileType(x + 1, y);
                leftSide = getTileType(x - 1, y);
                upSide = getTileType(x, y+1);
                downSide = getTileType(x, y-1);
                currentTile = getTileType(x, y);

                if(rightSide < 6)
                {
                    if (currentTile < rightSide)
                    {
                        GameObject splat1 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(x, y, -0.0001f), Quaternion.identity, this.transform);//right        
                        Renderer splat1shader = splat1.GetComponentInChildren<Renderer>();
                        splat1shader.material.SetTexture("_MainTex", whichSplat(rightSide));
                        splat1shader.material.SetTexture("_AlphaTex", Resources.Load<Texture>("Tiles/RightBlend"));
                    }
                }

                if (leftSide < 6)
                {
                    if (currentTile < leftSide)
                    {
                        GameObject splat1 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(x, y, -0.0002f), Quaternion.Euler(0f, 0f, 0f), this.transform);//left        
                        Renderer splat1shader = splat1.GetComponentInChildren<Renderer>();
                        splat1shader.material.SetTexture("_MainTex", whichSplat(leftSide));
                        splat1shader.material.SetTexture("_AlphaTex", Resources.Load<Texture>("Tiles/LeftBlend"));
                    }
                }

                if (downSide < 6)
                {
                    if (currentTile < downSide)
                    {
                        GameObject splat1 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(x, y, -0.0004f), Quaternion.Euler(0f, 0f, 0f), this.transform);//down     
                        Renderer splat1shader = splat1.GetComponentInChildren<Renderer>();
                        splat1shader.material.SetTexture("_MainTex", whichSplat(downSide));
                        splat1shader.material.SetTexture("_AlphaTex", Resources.Load<Texture>("Tiles/BottomBlend"));
                    }
                }

                if (upSide < 6)
                {
                    if (currentTile < upSide)
                    {
                        GameObject splat1 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(x, y, -0.0003f), Quaternion.Euler(0f, 0f, 0f), this.transform);//above        
                        Renderer splat1shader = splat1.GetComponentInChildren<Renderer>();
                        splat1shader.material.SetTexture("_MainTex", whichSplat(upSide));
                        splat1shader.material.SetTexture("_AlphaTex", Resources.Load<Texture>("Tiles/UpBlend"));
                    }
                }
            }
        }

        



        /*GameObject splat2 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(-1f, 0, 0.0001f), Quaternion.Euler(0f, 0f,-90f));//down
        Renderer splat2shader = splat2.GetComponentInChildren<Renderer>();
        splat2shader.material.SetTexture("_MainTex", shallow);

        GameObject splat3 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(-1f, 0, 0.0002f), Quaternion.Euler(0f, 0f, -180f));//left
        Renderer splat3shader = splat3.GetComponentInChildren<Renderer>();
        splat3shader.material.SetTexture("_MainTex", plains);

        GameObject splat4 = (GameObject)Instantiate(Resources.Load<GameObject>("Tiles/SplatPrefab"), new Vector3(-1f, 0, 0.0003f), Quaternion.Euler(0f, 0f, 90f));//above
        Renderer splat4shader = splat4.GetComponentInChildren<Renderer>();
        splat4shader.material.SetTexture("_MainTex", deep);*/
    }

    int getTileType(int x, int y)
    {
        if (x < 0 || y < 0 || x >= MapSizeX || y >= MapSizeY)
        {
            return 6;
        }

        switch (map[x,y].type)
        {
            case Tile.tileType.Grassland:
                return 4;

            case Tile.tileType.Marsh:
                return 0;

            case Tile.tileType.Mountain:
                return 5;

            case Tile.tileType.Water:
                return 1;

            case Tile.tileType.Plains:
                return 3;

            case Tile.tileType.Desert:
                return 2;

            default:
                return 6;
        }
    }


    /*
    void createUnit(string type, int player, int mapX, int mapY)
    {
        GameObject unitgo;

        switch (type)
        {
            case "Worker":
                unitgo = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/Worker"), new Vector3(mapX, mapY, 0), Quaternion.identity);
                assignTags(player, unitgo);
                break;

            case "Melee":
                unitgo = (GameObject)Instantiate(Resources.Load<GameObject>("Unit Prefabs/Worker"), new Vector3(mapX, mapY, 0), Quaternion.identity);
                assignTags(player, unitgo);
                break;

            default:
                break;
        }
    }
    */
    void assignTags(int player, GameObject unit)
    {
        if (player == 1)
        {
            unit.tag = "UnitControllerP1";
            foreach(Transform t in unit.transform)
            {
                t.tag = "Unit tag P1";
            }
        }
        if (player == 0)
        {
            unit.tag = "UnitControllerP2";
            foreach (Transform t in unit.transform)
            {
                t.tag = "Unit tag P2";
            }
        }
    }

    void turnOffEnemyVisuals()
    {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach (GameObject unit in enemyUnits)
        {
            Renderer render = unit.GetComponent<Renderer>();
            render.enabled = false;
        }
        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyVision");
        foreach (GameObject unit in enemyUnits)
        {
            Renderer render = unit.GetComponent<Renderer>();
            render.enabled = false;
        }
    }

    /*
    public void SelectUnit(GameObject unit)
    {
        this.selectedUnit = unit;
    }
    */





    public string GetTileResName(int x, int y)
    {
        string ResorceNam = "Nothing";
        Tile.tileResource ans = map[x, y].resource;

        if(ans == Tile.tileResource.Wood)
        {
            ResorceNam = "Wood";
        }
        if (ans == Tile.tileResource.Stone)
        {
            ResorceNam = "Stone";
        }
        if (ans == Tile.tileResource.Food)
        {
            ResorceNam = "Food";
        }
        if(ans == Tile.tileResource.Gold)
        {
            ResorceNam = "Gold";
        }

        if (ans == Tile.tileResource.Nothing)
        {
            ResorceNam = "Nothing";
        }


        return ResorceNam;
    }



    public int GetTileResAmt(int x, int y)
    {
        
        int ans = map[x, y].amountOfResource;

        return ans;
    }

    public void gatherResource(int x, int y, int amount)
    {
        map[x, y].amountOfResource -= amount;
    }

    public void removeResource(int x, int y)
    {
        map[x, y].resource = Tile.tileResource.Nothing;
        string name = "Resource" + x.ToString() + "," + y.ToString();
        Destroy(GameObject.Find(name));
    }



    void generateMap()
    {
        //allocation of map tiles
        tiles = new int[MapSizeX, MapSizeY];



        map = new Tile[MapSizeX, MapSizeY];

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                float height = GetHeight(x, y);
                if (height < .35)
                {
                    map[x, y] = new Tile();
                    map[x, y].type = Tile.tileType.Water;
                    map[x, y].isWalkable = false;
                }
                else if (height < .4)
                {
                    map[x, y] = new Tile();
                    map[x, y].type = Tile.tileType.Marsh;
                    map[x, y].isWalkable = true;
                }
                else if (height < .7)
                {
                    map[x, y] = new Tile();
                    if (GetMoisture(x,y) < .25) {
                        map[x, y].type = Tile.tileType.Desert;
                    }else if(GetMoisture(x,y) < .5){
                        map[x, y].type = Tile.tileType.Plains;
                    }
                    else
                    {
                        map[x, y].type = Tile.tileType.Grassland;
                    }
                    map[x, y].isWalkable = true;
                }
                else
                {
                    map[x, y] = new Tile();
                    map[x, y].type = Tile.tileType.Mountain;
                    map[x, y].isWalkable = false;
                }
            }
        }

        GenerateResources();
    }

    void GenerateResources()
    {

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                int rand = Random.Range((int)0, (int)15);
                if (rand == 0)
                {
                    map[x, y].resource = Tile.tileResource.Food;
                    if (CheckIfNextToMountain(x, y))
                    {
                        int r = Random.Range((int)0, (int)1);
                        if (r == 0)
                        {
                            map[x, y].resource = Tile.tileResource.Stone;
                        }
                        else
                        {
                            map[x, y].resource = Tile.tileResource.Gold;
                        }
                    }
                    map[x, y].amountOfResource = GenerateAmountOfResource();
                }
                else if (rand == 1)
                {
                    map[x, y].resource = Tile.tileResource.Stone;
                    map[x, y].amountOfResource = GenerateAmountOfResource();
                }
                else if (rand == 2)
                {
                    map[x, y].resource = Tile.tileResource.Wood;
                    if (CheckIfNextToMountain(x, y))
                    {
                        int r = Random.Range((int)0, (int)1);
                        if (r == 0)
                        {
                            map[x, y].resource = Tile.tileResource.Stone;
                        }
                        else
                        {
                            map[x, y].resource = Tile.tileResource.Gold;
                        }
                    }
                    map[x, y].amountOfResource = GenerateAmountOfResource();
                }
                else if(rand == 3)
                {
                    map[x, y].resource = Tile.tileResource.Gold;
                    map[x, y].amountOfResource = GenerateAmountOfResource();
                }
                else
                {
                    map[x, y].resource = Tile.tileResource.Nothing;
                    map[x, y].amountOfResource = 0;
                }

            }
        }
    }

    int GenerateAmountOfResource()
    {
        return Random.Range((int)500, (int)1000);

    }


    bool CheckIfNextToMountain(int x, int y)
    {
        for (int i = x - 1; i <= (x + 1); i++)
        {
            for (int j = y - 1; j <= (y + 1); j++)
            {
                if (i == -1 || i == MapSizeX || j == -1 || j == MapSizeY)
                {
                    continue;
                }
                else
                {
                    if (map[i, j].type == Tile.tileType.Mountain)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        return false;
    }

    float GetHeight(int x, int y)
    {
        float xCoords = (float)x / MapSizeX * 8;
        float yCoords = (float)y / MapSizeY * 8;
        int rand = 1;
        //int rand = Random.Range(0,10000);

        float sample = Mathf.PerlinNoise(xCoords + rand, yCoords + rand);
        return sample;
    }

    float GetMoisture(int x, int y)
    {
        float xCoords = (float)x / MapSizeX * 10;
        float yCoords = (float)y / MapSizeY * 10;
        int offset = 0;
        //int offset = Random.Range(10,500);

        float sample = Mathf.PerlinNoise(xCoords + offset, yCoords + offset);
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
        TileType tt = tileTypes[0];
        TileResource tr = tileResource[0];

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {


                switch (map[x, y].type)
                {
                    case Tile.tileType.Grassland:
                        tt = tileTypes[0];
                        break;

                    case Tile.tileType.Marsh:
                        tt = tileTypes[1];
                        break;

                    case Tile.tileType.Mountain:
                        tt = tileTypes[2];
                        break;

                    case Tile.tileType.Water:
                        tt = tileTypes[3];
                        break;

                    case Tile.tileType.Plains:
                        tt = tileTypes[4];
                        break;

                    case Tile.tileType.Desert:
                        tt = tileTypes[5];
                        break;

                    default:
                        break;
                }

                        //tile type of the tiles coordinace is set so the tile can call the correct visual prefab

            GameObject go = (GameObject)Instantiate(tt.TileVisualPrefab, new Vector3(x, y, -.5f), Quaternion.Euler(-90f,0f,0f), this.transform);
                go.name = "Tile" + x.ToString() + "," + y.ToString();

                if(tt.TileMesh != null)
                {
                    GameObject gomesh = (GameObject)Instantiate(tt.TileMesh, new Vector3(x, y, 0f), Quaternion.identity, go.transform);
                    gomesh.name = "TileMesh" + x.ToString() + "," + y.ToString();
                }

                //the game object is initalized and created at set coordinace

                switch (map[x, y].resource)
                {
                    case Tile.tileResource.Food:
                        tr = tileResource[0];
                        break;

                    case Tile.tileResource.Stone:
                        tr = tileResource[1];
                        break;

                    case Tile.tileResource.Wood:
                        tr = tileResource[2];
                        break;

                    case Tile.tileResource.Nothing:
                        tr = tileResource[3];
                        break;

                    case Tile.tileResource.Gold:
                        tr = tileResource[4];
                        break;

                    default:
                        break;
                }

                if (map[x, y].type != Tile.tileType.Water && map[x, y].resource != Tile.tileResource.Nothing && map[x, y].type != Tile.tileType.Mountain)
                {
                    GameObject rgo = (GameObject)Instantiate(tr.ResourceVisualPrefab, new Vector3(x, y, -.5f), Quaternion.Euler(0, 0, Random.Range(0f, 360f)), this.transform);
                    rgo.name = "Resource" + x.ToString() + "," + y.ToString();
                }

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

        return map[x,y].isWalkable;
    }



    //sets the units data on what tile it is on and then set it up visually
    //Dijkstra's algorithm is used for the pathfinding A* search algorithm is another more commonly used pathfinging algorithm that goes in a general direction and is quicker
    //Dijkstra's was used because I did just under a year ago in COMP250
    public void MoveSelectedUnitTo(int x, int y)
    {

        GameObject selectedUnit = mouseManagerS.selectedUnit; //get the selected unit from Mouse Manager

        if (selectedUnit == null)
            return;

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
