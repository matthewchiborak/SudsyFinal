using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class MoveHistory
{
    public ActorPlayer player;
    public List<MoveableTile> blocks;    
    public List<List<Tile>> board;

    public List<ActorEnemy> enemies;
    public bool chain;

    public MoveHistory(GameBoardModel gb, bool chain = false)
    {
        //clone the player
        this.player = gb.player.clone();

        //clone the moveable blocks
        this.blocks = new List<MoveableTile>();
        foreach(MoveableTile t in gb.blocks)
        {
            this.blocks.Add(t.clone());
        }

        this.board = new List<List<Tile>>();
        foreach(List<Tile> row in gb.board)
        {
            List<Tile> row_copy = new List<Tile>();
            foreach(Tile t in row)
            {
                row_copy.Add(t.clone());
            }
            this.board.Add(row_copy);
        }

        this.enemies = new List<ActorEnemy>();
        foreach(ActorEnemy enm in gb.enemies)
        {
            this.enemies.Add(enm.clone());
        }

        this.chain = chain;
    }

    public void restore(GameBoardModel gb)
    {
        gb.player = this.player;
        gb.blocks = this.blocks;
        gb.board = this.board;
        gb.enemies = this.enemies;

        //Update the actors for the scripts
        foreach (ActorEnemy enm in gb.enemies)
        {
            enm.script.addActor(enm);
        }
    }
}

public class GameBoardModel : SudsyModel
{

    public SafeQueue<GameBoardEvent> events = new SafeQueue<GameBoardEvent>();
    public ActorPlayer player = new ActorPlayer();
    public List<ActorEnemy> enemies;
    public List<MoveableTile> blocks;

    public Stack<MoveHistory> history;

    public List<List<Tile>> board;

    

    public MoveableTile curBlock;

    public Tile end_tile;
    public Tile start_tile;

    public int height, width;

    public bool is_win = false;
    public bool is_lose = false;
    
    private void Start()
    {
        curBlock = null;
    }

    private void Update()
    {
        //Exit is won (TODO: add check for death when enemies exist)
        if (is_win) return;
        if (is_lose) return;

        if( (events != null) && (!events.isEmpty()))
        {            
            GameBoardEvent e = events.Dequeue();
            bool run_ai_script = e.doEvent();

            is_lose = Check_Lose();

            if (is_lose) return;

            //Move enemies making sure we are in action phase and the player has moved
            if (!(e is ActorMoveEvent) ) return;
            if (!run_ai_script) return;

            foreach(ActorEnemy act in enemies)
            {
                
                if(act.script != null)
                {
                    
                    act.script.doEvent();
                    
                }

            }

            is_lose = Check_Lose();
        }

        is_win = checkWin();        
    }

    public MoveableTile GetBlockAt(int row, int col)
    {

        MoveableTile result = null;

        //Check if a tile is in this position.
        foreach (MoveableTile t in blocks)
        {

            if ( (t.row == row) && ( (t.col) == col) )
            {
                result = t;
                break;
            }
        }

        return result;
    }

    //Save history
    public void saveHistory(bool chain = false)
    {
        this.history.Push(new MoveHistory(this, chain));
    }

    public void restoreHistory()
    {
        if(this.history.Count > 0)
        {
            this.history.Pop().restore(this);
        }
    }

    private bool Check_Lose()
    {
        foreach(ActorEnemy enm in enemies)
        {
            if ((player.row == enm.row) && (player.col == enm.col))
            {
                app.Notify(SudysNotifications.GameLose, this);
                return true;
            }
        }        

        return false;
    }

    private bool checkWin()
    {

        GameBoardModel gb = app.game_board_model;
        if (gb.player.currentTile == null) return false;

        if (gb.player.row != gb.end_tile.row) return false;
        if (gb.player.col != gb.end_tile.col) return false;

        for (int row = 0; row < gb.height; row++)
        {
            for (int col = 0; col < gb.width; col++)
            {
                if (gb.board[row][col].type == TileType.Dirty) return false;
            }
        }

        app.Notify(SudysNotifications.GameWin, this);
        app.sfx_controller.playWinSound();

        return true;
    }

    public string loadLevel(string fname)
    {
        string result = "Error loading file.";

        is_win = false;
        is_lose = false;

        if(!File.Exists("Assets/Levels/" + fname))
        {
            return result;
        }

        //Empty the event queue so that events from the previous level do not effect the next level
        events = new SafeQueue<GameBoardEvent>();
        player = new ActorPlayer();
        
        curBlock = null;


        history = new Stack<MoveHistory>();
        board = new List<List<Tile>>();

        using (var streamReader = new StreamReader("Assets/Levels/" + fname, Encoding.UTF8))
        {

            JSONNode json = JSON.Parse(streamReader.ReadToEnd());

            
            if (json["height"].IsNull)  return "Error loading height;";            
            height = json["height"].AsInt;
            

            if (json["width"].IsNull) return "Error loading width;";
            width = json["width"].AsInt;
            

            //Load the default gameboard
            for(int r = 0; r < height; r++)
            {

                List<Tile> tile_row = new List<Tile>();
                for (int c = 0; c < width; c++)
                {
                    Tile t = TileFactoryMethods.TileFactory(TileType.Dirty);
                    t.setPos(r, c);
                    tile_row.Add(t);
                    
                }

                board.Add(tile_row);
            }

            //Validate data
            if (json["start"].IsNull || json["start"]["row"].IsNull || json["start"]["col"].IsNull)
                return "Error loading level stating tile.";

            if (json["end"].IsNull || json["end"]["row"].IsNull || json["end"]["col"].IsNull)
                return "Error loading level end tile.";

            if ((json["start"]["row"] == json["end"]["row"]) && (json["start"]["col"] == json["end"]["col"]))
                return "Error: Start and end cannot be the same tile";

            int row, col;

            //Add the start tile
            row = json["start"]["row"].AsInt;
            col = json["start"]["col"].AsInt;

            if ((row < 0) || (row >= height) || (col < 0) || (col >= width))
                return "Start tile is not in board dimensions.";

            Tile tile = TileFactoryMethods.TileFactory(TileType.Clean);
            tile.setPos(row, col);
            board[row][col] = tile;
            start_tile = tile;
            //cHANGE
            player.col = col;
            player.row = row;

            //Add the end tile
            row = json["end"]["row"].AsInt;
            col = json["end"]["col"].AsInt;

            if ((row < 0) || (row >= height) || (col < 0) || (col >= width))
                return "end tile is not in board dimensions.";

            tile = TileFactoryMethods.TileFactory(TileType.End);
            tile.setPos(row, col);
            board[row][col] = tile;
            end_tile = tile;

            //Load the special tiles
            MoveableTile movetile;
            blocks = new List<MoveableTile>();
            JSONArray tiles = json["tiles"].AsArray;
            foreach(JSONObject j in tiles)
            {
                int brow = j["row"].AsInt;
                int bcol = j["col"].AsInt;
                int moves = j["moves"].IsNull ? 0 : j["moves"].AsInt;
                string type = j["type"];

                movetile = (MoveableTile) TileFactoryMethods.TileFactory(type);

                movetile.setPos(brow, bcol);
                movetile.setMoves(moves);

                board[brow][bcol] = movetile;
                blocks.Add(movetile);
            }

            //Load the enemies
            enemies = new List<ActorEnemy>();
            JSONArray enemy_array = json["enemy"].AsArray;
            foreach(JSONObject j in enemy_array)
            {
                
                int erow = j["row"].AsInt;
                int ecol = j["col"].AsInt;                
                string type = j["type"];
                
                ActorEnemy act = ActorEnemyFactoryMethods.ActorEnemyFactory(type);
                act.row = erow;
                act.col = ecol;
                
                enemies.Add(act);
            }

            result = "File Loaded";
        }

        app.sfx_controller.playResetSound();

        return result;
    }
}

