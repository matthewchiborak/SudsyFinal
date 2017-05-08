using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTileView : SudsyView {

    public GameObject bgtile;
    public GameObject mudtile;
    public GameObject player_prefab;
    public GameObject clean_tile;
    public GameObject end_tile;
    public GameObject block_tile;
    public GameObject water_tile;
    public GameObject enemy_sprite;

    int speed = 15;

    private List<List<GameObject>> clean_array;
    private List<List<GameObject>> mud_array;
    private List<List<GameObject>> tile_array;
    private List<GameObject> block_array;
    private List<GameObject> enemy_array;


    private GameObject player;

    //Need to capture the end point object for it can be removed when loading a new level
    private GameObject endPoint;


    float tile_layer = 0f;
    float mud_layer = 0f;
    float obj_layer = -.05f;


    private bool started = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (!started) return;

        updateMud();
        updateClean();
        updatePlayerPos();
        updateBlockPos();
        updateEnemyPos();
        updateInRangeOfCurrentBlock();

    }    

    public void init()
    {

        GameBoardModel gb = app.game_board_model;

        mud_array = new List<List<GameObject>>();
        clean_array = new List<List<GameObject>>();
        tile_array = new List<List<GameObject>>();        

        for (int col = 0; col < gb.width; col++)
        {

            List<GameObject> mud_row = new List<GameObject>();
            List<GameObject> clean_row = new List<GameObject>();
            List<GameObject> tile_row = new List<GameObject>();

            for (int row = 0; row < gb.height; row++)
            {

                //tile_row.Add(Instantiate(bgtile, new Vector3(row, col, tile_layer), Quaternion.identity));
                //mud_row.Add(Instantiate(mudtile, new Vector3(row, col, mud_layer), Quaternion.identity));
                //clean_row.Add(Instantiate(clean_tile, new Vector3(row, col, mud_layer), Quaternion.identity));

                //CHANGE
                tile_row.Add(Instantiate(bgtile, new Vector3(col, row, tile_layer), Quaternion.identity));
                mud_row.Add(Instantiate(mudtile, new Vector3(col, row, mud_layer), Quaternion.identity));
                clean_row.Add(Instantiate(clean_tile, new Vector3(col, row, mud_layer), Quaternion.identity));
            }

            tile_array.Add(tile_row);
            mud_array.Add(mud_row);
            clean_array.Add(clean_row);

        }

        //draw the starting tile
        Tile t = gb.end_tile;
        endPoint = Instantiate(end_tile, new Vector3(t.col, t.row, obj_layer), Quaternion.identity);

        //draw the player
        player = Instantiate(player_prefab, new Vector3(gb.start_tile.col, gb.start_tile.row, obj_layer), Quaternion.identity);

        //draw the blocks
        block_array = new List<GameObject>();
        foreach(Tile block in gb.blocks)
        {
            GameObject tile_type = GetTileType(block.type);
            block_array.Add(Instantiate(tile_type, new Vector3(block.col, block.row, obj_layer), Quaternion.identity));
        }

        //Draw the enemies
        enemy_array = new List<GameObject>();
        foreach(ActorEnemy e in gb.enemies)
        {

            enemy_array.Add(Instantiate(enemy_sprite, new Vector3(e.col, e.row, obj_layer), Quaternion.identity));

        }

        //CHANGE
        //Move the camera to the centre of the grid
        Vector3 newCamPos = new Vector3(gb.width / 2 - 0.5f, gb.height / 2, -7.5f);
        Camera.main.transform.position = (newCamPos);

        started = true;
    }

    private GameObject GetTileType(TileType t)
    {
        switch (t)
        {
            case TileType.Block:
                return block_tile;

            case TileType.Water:
                return water_tile;

            default:
                return block_tile;
        }
    }

    //Removes all the current game objects from the scene so can load a new level. Then creates the new objects
    public void reloadLevel()
    {
        ///If the game has not been loaded, load the game.
        if (!started)
        {
            this.init();
            return;
        }

        //Remove all the existing objects
        for (int i = 0; i < clean_array.Count; i++)
        {
            for(int j = 0; j < clean_array[i].Count; j++)
            {
                Destroy(clean_array[i][j]);
            }
        }
        for (int i = 0; i < mud_array.Count; i++)
        {
            for (int j = 0; j < mud_array[i].Count; j++)
            {
                Destroy(mud_array[i][j]);
            }
        }
        for (int i = 0; i < tile_array.Count; i++)
        {
            for (int j = 0; j < tile_array[i].Count; j++)
            {
                Destroy(tile_array[i][j]);
            }
        }
        for (int i = 0; i < block_array.Count; i++)
        {
            Destroy(block_array[i]);
        }

        for (int i = 0; i < enemy_array.Count; i++)
        {
            Destroy(enemy_array[i]);
        }

        Destroy(endPoint);
        Destroy(player);

        this.init();
    }

    //Update enemy positions
    private void updateEnemyPos()
    {
        GameBoardModel gb = app.game_board_model;

        for (int i = 0; i < gb.enemies.Count; i++)
        {
            ActorEnemy t = gb.enemies[i];
            GameObject tObj = this.enemy_array[i];

            Vector3 dest = new Vector3(t.col, t.row, obj_layer);
            tObj.transform.position = Vector3.Lerp(tObj.transform.position, dest, speed * Time.deltaTime);
        }
    }

    //Update all block positions
    private void updateBlockPos()
    {
        GameBoardModel gb = app.game_board_model;

        for(int i = 0; i < gb.blocks.Count; i++)
        {
            Tile t = gb.blocks[i];
            GameObject tObj = this.block_array[i];

            Vector3 dest = new Vector3(t.col, t.row, obj_layer);
            tObj.transform.position = Vector3.Lerp(tObj.transform.position, dest, speed * Time.deltaTime);
        }
    }

    //Update the player position
    private void updatePlayerPos()
    {
        GameBoardModel gb = app.game_board_model;
        Vector3 dest = new Vector3(gb.player.col, gb.player.row, obj_layer);

        player.transform.position = Vector3.Lerp(player.transform.position, dest, speed * Time.deltaTime);
    }

    private void updateClean()
    {
        GameBoardModel gb = app.game_board_model;

        for (int col = 0; col < gb.width; col++)
        {

            for (int row = 0; row < gb.height; row++)
            {

                bool clean = (gb.board[row][col].type == TileType.Clean);
                //clean_array[row][col].gameObject.GetComponent<Renderer>().enabled = clean;
                //CHANGE
                clean_array[col][row].gameObject.GetComponent<Renderer>().enabled = clean;
            }

        }
    }

    private void updateMud()
    {
        GameBoardModel gb = app.game_board_model;

        for (int col = 0; col < gb.width; col++)
        {
            for (int row = 0; row < gb.height; row++)
            {
                bool muddy = (gb.board[row][col].type == TileType.Dirty);
                //mud_array[row][col].gameObject.GetComponent<Renderer>().enabled = muddy;
                //CHANGE
                mud_array[col][row].gameObject.GetComponent<Renderer>().enabled = muddy;
            }

        }
    }

    private void updateInRangeOfCurrentBlock()
    {
        GameBoardModel gb = app.game_board_model;

        MoveableTile t = gb.curBlock;

        for (int col = 0; col < tile_array.Count; col++)
        {
            List<GameObject> tile_row = tile_array[col];

            for(int row = 0; row < tile_row.Count; row++)
            {
                

                bool inrange = (t == null)?false:t.InRange(row, col);

                //GameObject obj = tile_array[row][col].gameObject;
                //CHANGE
                GameObject obj = tile_array[col][row].gameObject;
                Renderer rend = obj.GetComponent<Renderer>();
                rend.material.color = inrange ? Color.blue : Color.white;
            }
        }
    }
}
