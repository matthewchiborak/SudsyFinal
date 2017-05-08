using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameBoardEvent : SudsyElement
{
    protected GameBoardModel gb;

    public GameBoardEvent()
    {
        
    }

    private void Start()
    {
        gb = app.game_board_model;
    }

    public virtual Boolean doEvent()
    {
        return false;
    }

}

public class EnemyMoveEvent : ActorMoveEvent
{
    public EnemyMoveEvent() : base()
    {

    }

    public override Boolean doEvent()
    {

        return true;
    }
}

//Handles vertical movement of enemies
public class EnemyMoveEventVerticalUp : EnemyMoveEvent
{
    
    public EnemyMoveEventVerticalUp() : base()
    {

    }

    public override Boolean doEvent()
    {
        ActorEnemy enm = (ActorEnemy)act;

        //Check if we are at the top of the board
        if(act.row < (gb.height - 1))
        {
            Tile t = gb.board[act.row + 1][act.col];
            t.doMoveEvent(act, gb);
        }
        if((act.row >= (gb.height - 1)) || (gb.board[act.row + 1][act.col].type == TileType.Block))
        {
            enm.script = gameObject.AddComponent<EnemyMoveEventVerticalDown>();
            enm.script.addActor(enm);
        }

        return true;
    }

}

public class EnemyMoveEventVerticalDown : EnemyMoveEvent
{

    public EnemyMoveEventVerticalDown() : base()
    {
        
    }

    public override Boolean doEvent()
    {
        ActorEnemy enm = (ActorEnemy)act;

        //Check if we are at the top of the board
        if (act.row > 0 )
        {
            Tile t = gb.board[act.row - 1][act.col];
            t.doMoveEvent(act, gb);
        }
        if ((act.row <= 0) || (gb.board[act.row - 1][act.col].type == TileType.Block))
        {
            enm.script = gameObject.AddComponent<EnemyMoveEventVerticalUp>();
            enm.script.addActor(enm);
        }

        return true;
    }

}

public class EnemyMoveEventVerticalRight : EnemyMoveEvent
{

    public EnemyMoveEventVerticalRight() : base()
    {

    }

    public override Boolean doEvent()
    {
        ActorEnemy enm = (ActorEnemy)act;

        //Check if we are at the top of the board
        if (act.col < (gb.width - 1))
        {
            Tile t = gb.board[act.row][act.col + 1];
            t.doMoveEvent(act, gb);
        }
        if ((act.col >= (gb.width - 1)) || (gb.board[act.row][act.col + 1].type == TileType.Block))
        {
            enm.script = gameObject.AddComponent<EnemyMoveEventVerticalLeft>();
            enm.script.addActor(enm);
        }

        return true;
    }

}

public class EnemyMoveEventVerticalLeft : EnemyMoveEvent
{

    public EnemyMoveEventVerticalLeft() : base()
    {

    }

    public override Boolean doEvent()
    {
        ActorEnemy enm = (ActorEnemy)act;

        //Check if we are at the top of the board
        if (act.col > 0)
        {
            Tile t = gb.board[act.row][act.col - 1];
            t.doMoveEvent(act, gb);
        }
        if ((act.col <= 0) || (gb.board[act.row][act.col - 1].type == TileType.Block))
        {
            enm.script = gameObject.AddComponent<EnemyMoveEventVerticalRight>();
            enm.script.addActor(enm);
        }

        return true;
    }

}

//Handles clicking during the planning phase
public class GameBoardBlockClick : GameBoardEvent
{
    private int row, col ;

    public GameBoardBlockClick() : base() { }    

    public override Boolean doEvent()
    {

        if(gb == null)
        {
            gb = app.game_board_model;
        }
        MoveableTile hit = gb.GetBlockAt(row, col);

        //If we hit something new, update what is selected
        if((hit != null) && (gb.curBlock != hit))
        {
            gb.curBlock = hit;
            return true;
        }

        //Ignore case of hitting same block

        //If we didn't click on anything attempt to move the current block (if it exists) to that tile
        if ((hit == null) && (gb.curBlock != null))
        {

            int oldrow = gb.curBlock.row;
            int oldcol = gb.curBlock.col;

            //Try and move the block
            if (gb.curBlock.Move(row, col))
            {
                //Update the game board tiles if necessary
                gb.board[oldrow][oldcol] = TileFactoryMethods.TileFactory(TileType.Dirty);
                gb.board[row][col] = gb.curBlock;
            }

        }        

        return true;

    }

    public void setPos(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    private void Start()
    {
        gb = app.game_board_model;

    }
}

public class GameBoardBlockDrag : GameBoardEvent
{
    private int row, col;

    public GameBoardBlockDrag() : base() { }

    public override Boolean doEvent()
    {

        if (gb == null)
        {
            gb = app.game_board_model;
        }
        MoveableTile hit = gb.GetBlockAt(row, col);

        //If we hit something do nothing
        if ((hit != null)) return false;


        //If we didn't click on anything attempt to move the current block (if it exists) to that tile
        if ((hit == null) && (gb.curBlock != null))
        {

            int oldrow = gb.curBlock.row;
            int oldcol = gb.curBlock.col;

            //Try and move the block
            if (gb.curBlock.Move(row, col))
            {
                //Update the game board tiles if necessary
                gb.board[oldrow][oldcol] = TileFactoryMethods.TileFactory(TileType.Dirty);
                gb.board[oldrow][oldcol].row = oldrow;
                gb.board[oldrow][oldcol].col = oldcol;

                gb.board[row][col] = gb.curBlock;
            }

        }

        return true;

    }

    public void setPos(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    private void Start()
    {
        gb = app.game_board_model;

    }
}

//Undo move event
public class GameBoardUndoEvent : GameBoardEvent
{

    public GameBoardUndoEvent() : base() { }

    public override Boolean doEvent()
    {

        gb.restoreHistory();

        //If we need to undo more events
        if ( (gb.history.Count > 0) && (gb.history.Peek().chain) )
        {
            GameBoardUndoEvent ev = gb.gameObject.AddComponent<GameBoardUndoEvent>();
            gb.events.Enqueue(ev);
        }

        return false;

    }

}


public class ActorMoveEvent : GameBoardEvent
{
    protected Actor act;

    public ActorMoveEvent() : base()
    {         }

    public void addActor(Actor act)
    {
        this.act = act;
    }

    public override Boolean doEvent()
    {
        return false;
    }

}


public class ActorMoveEventUp : ActorMoveEvent
{

    public ActorMoveEventUp() : base() { }

    public override Boolean doEvent()
    {

        if (act.row >= (gb.height - 1)) return false;

        Tile tile = gb.board[act.row + 1][act.col];

        return tile.doMoveEvent(act, gb);

    }

}

public class ActorMoveEventDown : ActorMoveEvent
{

    public ActorMoveEventDown() : base() { }

    public override Boolean doEvent()
    {

        if (act.row <= 0) return false;

        Tile tile = gb.board[act.row - 1][act.col];

        return tile.doMoveEvent(act, gb);

    }

}

public class ActorMoveEventLeft : ActorMoveEvent
{
    public ActorMoveEventLeft() : base() { }

    public override Boolean doEvent()
    {

        if (act.col <= 0) return false;

        Tile tile = gb.board[act.row][act.col - 1];

        return tile.doMoveEvent(act, gb);

    }

}

public class ActorMoveEventRight : ActorMoveEvent
{

    public ActorMoveEventRight() : base() { }

    public override Boolean doEvent()
    {
        
        if (act.col >= (gb.width - 1)) return false;

        Tile tile = gb.board[act.row][act.col + 1];

        return tile.doMoveEvent(act, gb);

    }

}
