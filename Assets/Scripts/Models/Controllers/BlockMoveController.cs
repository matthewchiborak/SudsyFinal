using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BlockMoveController : SudsyController
{

    public override void OnNotification(string p_event_path, UnityEngine.Object p_target, params object[] p_data)
    {

        GameBoardModel gb = app.game_board_model;
        GameBoardEvent action = null;

        switch (p_event_path)
        {

            case "user.object.click":
                {
                    //Cast the parameter as a 2d vector
                    Vector2 pos = (Vector2)p_data[0];
                    GameBoardBlockClick a = gameObject.AddComponent<GameBoardBlockClick>();

                    //y = row, x = column
                    a.setPos((int)pos.y, (int)pos.x);
                   

                    action = a;
                    break;
                }

            case "user.object.clearclick":
                {
                    //maybe clear current block here?
                    break;
                }

            case "user.object.drag":
                {

                    //Cast the parameter as a 2d vector
                    Vector2 pos = (Vector2)p_data[0];
                    GameBoardBlockDrag a = gameObject.AddComponent<GameBoardBlockDrag>();

                    //y = row, x = column
                    a.setPos((int)pos.y, (int)pos.x);


                    action = a;
                    break;
                }
            
        }

        //If no action for is in the movement phase where blocks cannot be moved
        if (action != null) gb.events.Enqueue(action);

    }

}

