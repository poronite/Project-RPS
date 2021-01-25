using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//used this for reference: https://gamedev.stackexchange.com/questions/150917/how-to-get-all-tiles-from-a-tilemap
//clears the snow from the tile located at the player position
public class Snow : MonoBehaviour
{
    private Tilemap SnowTiles;
    private BoundsInt borders;
    private TileBase[] allSnowTiles;

    public TileBase SnowTile;

    private void Awake()
    {
        SnowTiles = gameObject.GetComponent<Tilemap>();

        borders = SnowTiles.cellBounds;
        allSnowTiles = SnowTiles.GetTilesBlock(borders);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("AI"))
        {
            Vector3Int collisionPosition = new Vector3Int((int)collision.transform.position.x, (int)collision.transform.position.y, 0);

            SnowTiles.SetTile(collisionPosition, null);            
        }
    }

    public void ResetSnowTiles()
    {
        for (int i = 0; i < allSnowTiles.Length; i++)
        {
            allSnowTiles[i] = SnowTile;
        }

        SnowTiles.SetTilesBlock(borders, allSnowTiles);
    }
}
