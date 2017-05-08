using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ActorEnemyFactoryMethods
{
    private static SudsyApplication app;

    public enum EnemyType { Basic, Vertical, Horizontal };
    
    public static void AttachApp(SudsyApplication a)
    {
        app = a;
    }

    public static ActorEnemy ActorEnemyFactory(EnemyType t)
    {
        switch (t)
        {
            case EnemyType.Basic:
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    return enm;
                }

            case EnemyType.Vertical:
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    enm.script = app.gameObject.AddComponent<EnemyMoveEventVerticalUp>();
                    enm.script.addActor(enm);

                    return enm;
                }

            case EnemyType.Horizontal:
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    enm.script = app.gameObject.AddComponent<EnemyMoveEventVerticalRight>();
                    enm.script.addActor(enm);

                    return enm;
                }
            default:
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    return enm;
                }
        }
    }

    public static ActorEnemy ActorEnemyFactory(string t)
    {
        switch (t)
        {
            case "stationary":
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    return enm;
                }
                
            case "vertical":
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    enm.script = app.gameObject.AddComponent<EnemyMoveEventVerticalUp>();
                    enm.script.addActor(enm);

                    return enm;
                }
            case "horizontal":
                {
                    return ActorEnemyFactory(EnemyType.Horizontal);
                }
            default:
                {
                    ActorEnemyStationary enm = new ActorEnemyStationary();
                    return enm;
                }
        }
    }

}

