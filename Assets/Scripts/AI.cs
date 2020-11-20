using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject Player;

    public int AIPositionX;
    public int AIPositionY;

    public int PlayerPositionX;
    public int PlayerPositionY;

    PathFind.Grid grid;
    public List<PathFind.Point> path;

    public bool[,] Tilesmap;

    int width = 17;
    int height = 32;



    void Start()
    {   
        // create the tiles map
        // Tilesmap = new bool[width, height];
        // set values here....
        // true = walkable, false = blocking

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


        // create a grid
        grid = new PathFind.Grid(width, height, Tilesmap);

        MakePath();
    }

    private void Update()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(new Vector3(path[i - 1].x + 0.5f, path[i - 1].y + 0.5f), new Vector3(path[i].x + 0.5f, path[i].y + 0.5f), Color.black, 5.0f);
        }
        
    }

    public void MakePath()
    {
        AIPositionX = (int)transform.position.x;
        AIPositionY = (int)transform.position.y;

        PlayerPositionX = (int)Player.transform.position.x;
        PlayerPositionY = (int)Player.transform.position.y;

        // create source and target points
        PathFind.Point from = new PathFind.Point(AIPositionX, AIPositionY);
        PathFind.Point to = new PathFind.Point(PlayerPositionX, PlayerPositionY);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        path = PathFind.Pathfinding.FindPath(grid, from, to);
    }
}
