using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tile
{

    //insert move behaviour
    public int row, col;
    public TileType type ;
    protected MoveBehavior moveBehaviour;

    public Tile(TileType type, MoveBehavior mvb)
    {
        this.type = type;
        moveBehaviour = mvb;

    }

    public Boolean doMoveEvent(Actor act, GameBoardModel gb)
    {
        
        return moveBehaviour.move(act, this, gb);

    }

    public Boolean setPos(int row, int col)
    {

        this.row = row;
        this.col = col;

        return true;

    }

    public Tile clone()
    {
        Tile result = new Tile(this.type, this.moveBehaviour);
        result.setPos(this.row, this.col);

        return result;
    }

}


public class MoveableTile : Tile
{
    int moves = 2;

    //stating position of the tile.
    int srow, scol;

    public MoveableTile(TileType type, MoveBehavior mvb) : base(type, mvb)
    {

    }

    public new bool setPos(int row, int col)
    {
        this.row = row;
        this.col = col;
        this.srow = row;
        this.scol = col;

        return true;
    }

    public Boolean Move(int row, int col)
    {

        //If the new position is reachable from the starting postition
        if(InRange(row, col))
        {
            this.row = row;
            this.col = col;

            return true;
        }

        return false;
    }

    public Boolean InRange(int row, int col)
    {
        //Calculate distance to new move from starting position
        int dist = Math.Abs(row - this.srow) + Math.Abs(col - this.scol);

        return dist <= moves;
    }

    public Boolean setMoves(int moves)
    {
        this.moves = moves;

        return true;
    }

    public new MoveableTile clone()
    {
        MoveableTile result = new MoveableTile(this.type, this.moveBehaviour);
        result.setMoves(this.moves);
        result.setPos(this.srow, this.scol);
        result.row = this.row;
        result.col = this.col;

        return result;
    }
}


