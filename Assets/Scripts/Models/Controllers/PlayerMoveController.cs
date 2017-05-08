using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;

class PlayerMoveController : SudsyController
{
    public override void OnNotification(string p_event_path, UnityEngine.Object p_target, params object[] p_data)
    {

        GameBoardModel gb = app.game_board_model;
        GameBoardEvent action = null;

        switch (p_event_path)
        {
           

            case "user.press.up":
                {
                    ActorMoveEvent a = gameObject.AddComponent<ActorMoveEventUp>();
                    a.addActor(gb.player);
                    action = a;
                    break;
                }

            case "user.press.down":
                {
                    ActorMoveEvent a = gameObject.AddComponent<ActorMoveEventDown>();
                    a.addActor(gb.player);
                    action = a;
                    break;
                }

            case "user.press.left":
                {
                    ActorMoveEvent a = gameObject.AddComponent<ActorMoveEventLeft>();
                    a.addActor(gb.player);
                    action = a;
                    break;
                }

            case "user.press.right":
                {                    
                    ActorMoveEvent a = gameObject.AddComponent<ActorMoveEventRight>();
                    a.addActor(gb.player);
                    action = a;
                    break;
                }

            case "user.undo":
                {
                    GameBoardEvent a = gameObject.AddComponent<GameBoardUndoEvent>();
                    action = a;
                    break;
                }
        }

        if (action != null)
        {
            gb.events.Enqueue(action);
        }

    }

}

