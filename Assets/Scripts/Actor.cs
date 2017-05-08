using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Actor
{

    public int row, col;
    
    public Tile currentTile;

    //Deafult Constructor
    public Actor()
    {
        row = col = 0;
    }

    //Make a deep copy
    public Actor clone()
    {
        Actor result = new Actor();

        result.row = this.row;
        result.col = this.col;
        result.currentTile = this.currentTile;

        return result;
    }
}

public class ActorPlayer : Actor
{

    public ActorPlayer() : base()
    {


    }

    public new ActorPlayer clone()
    {
        ActorPlayer result = new ActorPlayer();

        result.row = this.row;
        result.col = this.col;
        result.currentTile = this.currentTile;

        return result;
    }
}

public class ActorEnemy : Actor
{
    public EnemyMoveEvent script;
    public ActorEnemyFactoryMethods.EnemyType type;

    public ActorEnemy() : base() {
        type = ActorEnemyFactoryMethods.EnemyType.Basic;
    }

    public new ActorEnemy clone()
    {
        ActorEnemy result = new ActorEnemy();

        result.row = this.row;
        result.col = this.col;
        result.type = this.type;
        result.currentTile = this.currentTile;
        result.script = this.script;

        return result;
    }
    

}

public class ActorEnemyStationary : ActorEnemy
{

    public ActorEnemyStationary() : base()
    {
        type = ActorEnemyFactoryMethods.EnemyType.Basic;
    }

    public new ActorEnemyStationary clone()
    {
        ActorEnemyStationary result = new ActorEnemyStationary();

        result.row = this.row;
        result.col = this.col;
        result.type = this.type;
        result.currentTile = this.currentTile;
        result.script = this.script;

        return result;
    }

}

public class ActorEnemyVertical : ActorEnemy
{

    public ActorEnemyVertical() : base()
    {
        type = ActorEnemyFactoryMethods.EnemyType.Vertical;
    }

    public new ActorEnemyVertical clone()
    {
        ActorEnemyVertical result = new ActorEnemyVertical();

        result.row = this.row;
        result.col = this.col;
        result.type = this.type;
        result.currentTile = this.currentTile;
        result.script = this.script;

        return result;
    }

}

public class ActorEnemyHorizontal : ActorEnemy
{

    public ActorEnemyHorizontal() : base()
    {
        type = ActorEnemyFactoryMethods.EnemyType.Vertical;
    }

    public new ActorEnemyHorizontal clone()
    {
        ActorEnemyHorizontal result = new ActorEnemyHorizontal();

        result.row = this.row;
        result.col = this.col;
        result.type = this.type;
        result.currentTile = this.currentTile;
        result.script = this.script;

        return result;
    }

}

