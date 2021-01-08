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

    private int targetPositionX;
    private int targetPositionY;

    PathFind.Grid grid;
    public List<PathFind.Point> Path;

    private bool[,] tilesmap;

    int width;
    int height;

    public void StartAI()
    {
        aiController = gameObject.GetComponent<Player>();

        // create a grid
        grid = new PathFind.Grid(width, height, tilesmap);

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
                tilesmap = new bool[,]
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
                break;
            case 3:
                break;
            default:
                break;
        }

        width = tilesmap.GetLength(0);
        height = tilesmap.GetLength(1);
    }
    
    public void MakePath() //function used to create a path for the AI depending on target
    {
        gameObject.GetComponent<Player>().PathCount = 0;

        aiPositionX = (int)transform.position.x;
        aiPositionY = (int)transform.position.y;

        switch (AIObjective)
        {
            case "Player":
                targetPositionX = (int)Player.transform.position.x;
                targetPositionY = (int)Player.transform.position.y;
                break;
            case "Tokens":
                if (CheckTileAvailability())
                {
                    FindSpecialTile();
                }
                else
                {
                    targetPositionX = (int)Player.transform.position.x;
                    targetPositionY = (int)Player.transform.position.y;
                }
                break;
            default:
                break;
        }
        

        // create source and target points
        PathFind.Point from = new PathFind.Point(aiPositionX, aiPositionY);
        PathFind.Point to = new PathFind.Point(targetPositionX, targetPositionY);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        Path = PathFind.Pathfinding.FindPath(grid, from, to);
    }


    public void FindAIObjective() //find a target for the AI depending whether or not he can attack
    {
        if (!aiController.CanAttack)
        {
            AIObjective = "Tokens";
        }
        else
        {
            int numberAttackTokens = aiController.Tokens[0] + aiController.Tokens[2] + aiController.Tokens[4];
            //int tokencount = System.Convert.ToInt32(aiController.Tokens[0] > 0) + System.Convert.ToInt32(aiController.Tokens[2] > 0) + System.Convert.ToInt32(aiController.Tokens[4] > 0);

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
            }
        }
        
        MakePath();
    }

    private bool CheckTileAvailability() //verifies if there are tiles available
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
        Vector2 PlayerLocation = new Vector2(Player.transform.position.x, Player.transform.position.y);
        Vector2 AILocation = new Vector2(transform.position.x, transform.position.y);

        //variables that will help the AI decide whether which [insert color] tile is near him and far from the player
        float AItoSpecialTile;
        float PlayertoSpecialTile;

        //do while that runs until AI doesn't find a token tile to go to
        do
        {
            int SpecialTileTargetTile = Random.Range(1, 4); //1 = red, 2 = green, 3 = blue

            Debug.Log(SpecialTileTargetTile);

            switch (SpecialTileTargetTile)
            {
                case 1:
                    foreach (GameObject RedSpecialTile in redSpecialTiles)
                    {
                        if (RedSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 RedSpecialTileLocation = new Vector2(RedSpecialTile.transform.position.x, RedSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(RedSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(RedSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                targetPositionX = (int)RedSpecialTile.transform.position.x;
                                targetPositionY = (int)RedSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            targetPositionX = -1;
                            targetPositionY = -1;
                        }
                    }
                    break;
                case 2:
                    foreach (GameObject GreenSpecialTile in greenSpecialTiles)
                    {
                        if (GreenSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 GreenSpecialTileLocation = new Vector2(GreenSpecialTile.transform.position.x, GreenSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(GreenSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(GreenSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                targetPositionX = (int)GreenSpecialTile.transform.position.x;
                                targetPositionY = (int)GreenSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            targetPositionX = -1;
                            targetPositionY = -1;
                        }
                    }
                    break;
                case 3:
                    foreach (GameObject BlueSpecialTile in blueSpecialTiles)
                    {
                        if (BlueSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 BlueSpecialTileLocation = new Vector2(BlueSpecialTile.transform.position.x, BlueSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(BlueSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(BlueSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                targetPositionX = (int)BlueSpecialTile.transform.position.x;
                                targetPositionY = (int)BlueSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            targetPositionX = -1;
                            targetPositionY = -1;
                        }
                    }
                    break;
                default:
                    FindSpecialTile();
                    break;
            }

        }while (targetPositionX == -1 && targetPositionY == -1);
    }
}
