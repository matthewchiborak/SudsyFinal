using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum TileType { Clean, Dirty, Block, Start, End, Water }

class TileFactoryMethods
{

        

    public static Tile TileFactory(TileType t)
    {

        switch (t)
        {

            case TileType.Clean:
                return new Tile(t, new MoveBehaviorCleanTile());

            case TileType.Dirty:
                return new Tile(t, new MoveBehaviorDefult());             

            case TileType.Start:
                return new Tile(t, new MoveBehaviorBlock());

            case TileType.End:
                return new Tile(t, new MoveBehaviorDefult());

            case TileType.Block:
                return new MoveableTile(t, new MoveBehaviorBlock());

            case TileType.Water:
                return new MoveableTile(t, new MoveBehaviourWater());

            default:
                return new MoveableTile(t, new MoveBehaviorDefult());

        }
           

    }

    public static Tile TileFactory(string t)
    {

        switch (t)
        {

            case "clean":
                return  TileFactory(TileType.Clean);

            case "dirty":
                return TileFactory(TileType.Dirty);

            case "start":
                return TileFactory(TileType.Start);

            case "end":
                return TileFactory(TileType.End);

            case "block":
                return TileFactory(TileType.Block);

            case "water":
                return TileFactory(TileType.Water);

            default:
                return TileFactory(t);

        }


    }

}

