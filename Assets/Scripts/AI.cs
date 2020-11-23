using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public bool DrawLine;

    public string AIObjective;

    public GameObject Player;
    public GameplayManager GameplayManager;

    public List<GameObject> RedSpecialTiles;
    public List<GameObject> GreenSpecialTiles;
    public List<GameObject> BlueSpecialTiles;

    public int AIPositionX;
    public int AIPositionY;

    public int TargetPositionX;
    public int TargetPositionY;

    PathFind.Grid grid;
    public List<PathFind.Point> path;

    public bool[,] Tilesmap;

    int width = 17;
    int height = 32;



    void Start()
    {
        map1AI();

        // create a grid
        grid = new PathFind.Grid(width, height, Tilesmap);
    }

    private void Update()
    {
        if (GameplayManager.PlayerTurn == 2 && DrawLine == true)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(new Vector3(path[i - 1].x + 0.5f, path[i - 1].y + 0.5f), new Vector3(path[i].x + 0.5f, path[i].y + 0.5f), Color.black, 5.0f);
            }
        }  
    }

    private void map1AI()
    {
        RedSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("RedSpecialTile"));
        GreenSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("GreenSpecialTile"));
        BlueSpecialTiles.AddRange(GameObject.FindGameObjectsWithTag("BlueSpecialTile"));

        Tilesmap = new bool[,]
        {
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }
        };
    }

    public void MakePath()
    {
        gameObject.GetComponent<Player>().PathCount = 0;

        AIPositionX = (int)transform.position.x;
        AIPositionY = (int)transform.position.y;

        switch (AIObjective)
        {
            case "Player":
                TargetPositionX = (int)Player.transform.position.x;
                TargetPositionY = (int)Player.transform.position.y;
                break;
            case "Tokens":
                FindSpecialTile();
                break;
            default:
                break;
        }
        

        // create source and target points
        PathFind.Point from = new PathFind.Point(AIPositionX, AIPositionY);
        PathFind.Point to = new PathFind.Point(TargetPositionX, TargetPositionY);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        path = PathFind.Pathfinding.FindPath(grid, from, to);
    }

    public void FindAIObjective()
    {
        switch (gameObject.GetComponent<Player>().CanAttack)
        {
            case true:
                AIObjective = "Player";
                break;
            case false:
                AIObjective = "Tokens";
                break;
        }

        MakePath();
    }

    public void FindSpecialTile()
    {
        int SpecialTileTargetTile = Random.Range(1, 4);

        Debug.Log(SpecialTileTargetTile);

        Vector2 PlayerLocation = new Vector2(Player.transform.position.x, Player.transform.position.y);
        Vector2 AILocation = new Vector2(transform.position.x, transform.position.y);

        float AItoSpecialTile;
        float PlayertoSpecialTile;

        do
        {
            switch (SpecialTileTargetTile)
            {
                case 1:
                    foreach (GameObject RedSpecialTile in RedSpecialTiles)
                    {
                        if (RedSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 RedSpecialTileLocation = new Vector2(RedSpecialTile.transform.position.x, RedSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(RedSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(RedSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                TargetPositionX = (int)RedSpecialTile.transform.position.x;
                                TargetPositionY = (int)RedSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            TargetPositionX = -1;
                            TargetPositionY = -1;
                        }
                    }
                    break;
                case 2:
                    foreach (GameObject GreenSpecialTile in GreenSpecialTiles)
                    {
                        if (GreenSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 GreenSpecialTileLocation = new Vector2(GreenSpecialTile.transform.position.x, GreenSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(GreenSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(GreenSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                TargetPositionX = (int)GreenSpecialTile.transform.position.x;
                                TargetPositionY = (int)GreenSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            TargetPositionX = -1;
                            TargetPositionY = -1;
                        }
                    }
                    break;
                case 3:
                    foreach (GameObject BlueSpecialTile in BlueSpecialTiles)
                    {
                        if (BlueSpecialTile.GetComponent<SpecialToken>().OffCooldown == true)
                        {
                            Vector2 BlueSpecialTileLocation = new Vector2(BlueSpecialTile.transform.position.x, BlueSpecialTile.transform.position.y);

                            AItoSpecialTile = Vector2.SqrMagnitude(BlueSpecialTileLocation - AILocation);
                            PlayertoSpecialTile = Vector2.SqrMagnitude(BlueSpecialTileLocation - PlayerLocation);

                            if (AItoSpecialTile < PlayertoSpecialTile)
                            {
                                TargetPositionX = (int)BlueSpecialTile.transform.position.x;
                                TargetPositionY = (int)BlueSpecialTile.transform.position.y;
                            }
                        }
                        else
                        {
                            TargetPositionX = -1;
                            TargetPositionY = -1;
                        }
                    }
                    break;
                default:
                    FindSpecialTile();
                    break;
            }

        }while (TargetPositionX == -1 && TargetPositionY == -1);
    }
}
