using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MoveBehavior
{

    public virtual Boolean move(Actor actor, Tile block, GameBoardModel gb)
    {

        return false;

    }

}

public class MoveBehaviorDefult : MoveBehavior
{

    public override Boolean move(Actor actor, Tile block, GameBoardModel gb)
    {

        //Store the previous state info if the player has moved
        if(actor is ActorPlayer) gb.saveHistory();

        //move the player
        actor.row = block.row;
        actor.col = block.col;


        if (actor is ActorPlayer)
        {
            Tile t = TileFactoryMethods.TileFactory(TileType.Clean);
            t.setPos(actor.row, actor.col);
            gb.board[actor.row][actor.col] = t;
            block = t;
        }


        //update the players location
        actor.currentTile = block;

        return true;

    }

}

public class MoveBehaviourWater : MoveBehavior
{
    public override Boolean move(Actor actor, Tile block, GameBoardModel gb)
    {

        //Store the previous state info
        if (actor is ActorPlayer) gb.saveHistory(true);

        ActorMoveEvent action = null;

        //Chain move along row
        if(actor.col != block.col)
        {
            //moving up
            if(actor.col < block.col)
            {
                action = gb.gameObject.AddComponent<ActorMoveEventRight>();
            }
            //moving down
            else
            {
                action = gb.gameObject.AddComponent<ActorMoveEventLeft>();
            }

        }
        //moving along column
        else
        {
            //moving right
            if (actor.row < block.row)
            {
                action = gb.gameObject.AddComponent<ActorMoveEventUp>();
            }
            //moving left
            else
            {
                action = gb.gameObject.AddComponent<ActorMoveEventDown>();
            }
        }

        //Add action;
        action.addActor(actor);
        gb.events.Enqueue(action);

        //move the player
        actor.row = block.row;
        actor.col = block.col;

        

        //Set the current tile to clean if the actor is a player
        if (actor is ActorPlayer)
        {
            Tile t = TileFactoryMethods.TileFactory(TileType.Clean);
            t.setPos(actor.row, actor.col);
            gb.board[actor.row][actor.col] = t;
            block = t;
        }
        

        //update the players location
        actor.currentTile = block;

        //or true? effects enemy ai
        return false;

    }
}

public class MoveBehaviorBlock : MoveBehavior
{

    public override Boolean move(Actor actor, Tile block, GameBoardModel gb)
    {

        return false;

    }
}

public class MoveBehaviorCleanTile : MoveBehavior
{
    public override Boolean move(Actor actor, Tile block, GameBoardModel gb)
    {
        if(actor is ActorPlayer)        return false;
        MoveBehaviorDefult pass_move = new MoveBehaviorDefult();
        return pass_move.move(actor, block, gb);
    }
}

