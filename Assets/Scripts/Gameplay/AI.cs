using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //scripts in the Pathfinding folder used for the AI Pathfinding are not made by me
    //you can find them in here: https://github.com/RonenNess/Unity-2d-pathfinding
    //pretty easy to use in case you find pathfinding difficult like me

    public bool DrawLine;

    public string AIObjective;

    public GameObject Player;
    public GameplayManager GameplayManager;
    private Player aiController;

    private List<GameObject> redSpecialTiles = new List<GameObject>();
    private List<GameObject> greenSpecialTiles = new List<GameObject>();
    private List<GameObject> blueSpecialTiles = new List<GameObject>();

    private int trust; //level of trust depending on number of tokens the AI has that can go from 1 to 5;
    private int confidence; //confidence of the AI, which is random between 1 and 5
    private int risk; //level of risk that determines AI next move | risk = trust + confidence | min risk = 0 | max risk = 10

    private int aiPositionX;
    private int aiPositionY;

    public int TargetPositionX;
    public int TargetPositionY;

    private int targetTokenPositionX;
    private int targetTokenPositionY;

    PathFind.Grid grid;
    public List<PathFind.Point> Path;

    public bool[,] Tilesmap;

    int width;
    int height;

    public void StartAI()
    {
        aiController = gameObject.GetComponent<Player>();

        MakeGrid();

        aiController.EnoughTokensToAttack();
        FindAIObjective();
    }

    private void Update()
    {
        //debug purposes, it just draws a line that represents the path from the AI to his target (token tile or player)
        if (GameplayManager.PlayerTurn == 2 && DrawLine == true)
        {
            for (int i = 1; i < Path.Count; i++)
            {
                Debug.DrawLine(new Vector3(Path[i - 1].x + 0.5f, Path[i - 1].y + 0.5f), new Vector3(Path[i].x + 0.5f, Path[i].y + 0.5f), Color.black, 5.0f);
            }
        }  
    }

    //map function that is used to help the AI distinguish between walkable and non walkable tiles
    //also puts the every object with special tiles tag in 3 different arrays depending on color
    public void MapAI() //map1 function
    {
        redSpecialTiles.Clear();
        greenSpecialTiles.Clear();
        blueSpecialTiles.Clear();

        redSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("RedSpecialTile"));
        greenSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("GreenSpecialTile"));
        blueSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("BlueSpecialTile"));

        switch (GameplayManager.Map)
        {
            case 1:
                Tilesmap = new bool[,]
                {
                   {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, false, false, true, true, true, true, false, false, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true, true, true, true, true, false},
                   {false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
                   {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
                };
                break;
            case 2:
                Tilesmap = new bool[,]
                {
                    {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, false, false, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, false, false, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, false, false, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, false, false, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, true, true, true, true, true, false},
                    {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
                };
                break;
            case 3:
                Tilesmap = new bool[,]
                {
                    {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
                    {false, false, false, false, false, true, true, true, true, true, true, false, false, false, false, false},
                    {false, false, false, true, true, true, true, true, true, true, true, true, true, false, false, false},
                    {false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false},
                    {false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false},
                    {false, true, true, true, true, true, false, false, false, false, true, true, true, true, true, false},
                    {false, true, true, true, true, false, false, false, false, false, false, true, true, true, true, false},
                    {false, true, true, true, false, false, false, false, false, false, false, false, true, true, true, false},
                    {false, true, true, true, false, false, false, false, false, false, false, false, true, true, true, false},
                    {false, true, true, true, false, false, false, false, false, false, false, false, true, true, true, false},
                    {false, true, true, true, true, false, false, false, false, false, false, true, true, true, true, false},
                    {false, true, true, true, true, true, false, false, false, false, true, true, true, true, true, false},
                    {false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false},
                    {false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false},
                    {false, false, false, true, true, true, true, true, true, true, true, true, true, false, false, false},
                    {false, false, false, false, false, true, true, true, true, true, true, false, false, false, false, false},
                    {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
                };
                break;
            default:
                break;
        }

        width = Tilesmap.GetLength(0);
        height = Tilesmap.GetLength(1);
    }

    public void MakeGrid()
    {
        // create a grid
        grid = new PathFind.Grid(width, height, Tilesmap);
    }
    
    public void MakePath() //function used to create a path for the AI depending on target
    {
        gameObject.GetComponent<Player>().PathCount = 0;

        aiPositionX = (int)transform.position.x;
        aiPositionY = (int)transform.position.y;

        switch (AIObjective)
        {
            case "Player":
                TargetPositionX = (int)Player.transform.position.x;
                TargetPositionY = (int)Player.transform.position.y;
                break;
            case "Tokens": //targetTokenPosition is so that the AI doesn't always change target(special tile) when making a path
                TargetPositionX = targetTokenPositionX;
                TargetPositionY = targetTokenPositionY;
                break;
            default:
                break;
        }

        if (aiPositionX == TargetPositionX && aiPositionY == TargetPositionY)
        {
            FindAIObjective();
        }

        Debug.Log($"Target X: {TargetPositionX}  |  Target Y: {TargetPositionY}");


        // create source and target points
        PathFind.Point from = new PathFind.Point(aiPositionX, aiPositionY);
        PathFind.Point to = new PathFind.Point(TargetPositionX, TargetPositionY);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        Path = PathFind.Pathfinding.FindPath(grid, from, to);
    }


    public void FindAIObjective() //find a target for the AI depending whether or not he can attack
    {
        do
        {
            if (!aiController.CanAttack)
            {
                AIObjective = "Tokens";
                FindSpecialTile();
            }
            else
            {
                int numberAttackTokens = aiController.Tokens[0] + aiController.Tokens[2] + aiController.Tokens[4];

                switch (numberAttackTokens)
                {
                    case 1: //if AI has only 1 attack token of any type
                        trust = 1;
                        break;
                    case 2: //if AI has 2 attack tokens of any type
                        trust = 4;
                        break;
                    default:
                        break;
                }

                confidence = Random.Range(1, 6);

                risk = trust + confidence;

                if (risk >= 5)
                {
                    AIObjective = "Player";
                }
                else if (risk < 5)
                {
                    AIObjective = "Tokens";
                    if (CheckTileAvailability())
                    {
                        FindSpecialTile();
                    }
                    else
                    {
                        targetTokenPositionX = (int)Player.transform.position.x;
                        targetTokenPositionY = (int)Player.transform.position.y;
                    }
                }
            }
        } while (targetTokenPositionX == -1 && targetTokenPositionY == -1);      

            
        MakePath();
    }

    private bool CheckTileAvailability() //verifies if there are tiles off cooldown
    {
        foreach (GameObject RedSpecialTile in redSpecialTiles)
        {
            if (RedSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
            {
                return true;
            }
        }

        foreach (GameObject GreenSpecialTile in greenSpecialTiles)
        {
            if (GreenSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
            {
                return true;
            }
        }

        foreach (GameObject BlueSpecialTile in blueSpecialTiles)
        {
            if (BlueSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
            {
                return true;
            }
        }

        return false;
    }

    public void FindSpecialTile() //in case AI chooses a token tile as a target, this function will select which specific tile he is going to
    {
        //just to make sure
        targetTokenPositionX = -1;
        targetTokenPositionY = -1;

        bool foundTarget = false;

        int whichTileSide;

        Vector2 AILocation = new Vector2(transform.position.x, transform.position.y);

        if (AILocation.y < 9) //AI will choose the token depending on the color and where he is on the map
        {
            whichTileSide = 0;
        }
        else
        {
            whichTileSide = 1;
        }

        int SpecialTileTargetTile = Random.Range(1, 4); //1 = red, 2 = green, 3 = blue

        Debug.Log(SpecialTileTargetTile);

        switch (SpecialTileTargetTile)
        {
            case 1:
                foundTarget = decideSpecialTile(redSpecialTiles, whichTileSide, foundTarget);
                break;
            case 2:
                foundTarget = decideSpecialTile(greenSpecialTiles, whichTileSide, foundTarget);
                break;
            case 3:
                foundTarget = decideSpecialTile(blueSpecialTiles, whichTileSide, foundTarget);
                break;
            default:
                break;
        }

        if (foundTarget == false)
        {
            targetTokenPositionX = -1;
            targetTokenPositionY = -1;
        }
    }

    private bool decideSpecialTile(List<GameObject> colorSpecialTiles, int whichTileSide, bool foundTarget)
    {
        for (int i = 0; i < colorSpecialTiles.Count; i++)
        {
            if (colorSpecialTiles[i].GetComponent<SpecialToken>().OffCooldown == true && i == whichTileSide && foundTarget == false)
            {
                targetTokenPositionX = (int)colorSpecialTiles[i].transform.position.x;
                targetTokenPositionY = (int)colorSpecialTiles[i].transform.position.y;
                foundTarget = true;
                Debug.Log($"Found Target at X: {targetTokenPositionX} | Y: {targetTokenPositionY}");
            }
        }

        return foundTarget;
    }
}
