using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;


public class PhaseSwitchController : SudsyController
{

    int level = 1;
    public int maxLevel = 39;
    string level_string;

    public void moveToAction()
    {
        //Change the input recievers and remove the selected block
        app.action_phase_input_view.enabled = true;
        app.planning_phase_input_view.enabled = false;
        app.game_board_model.curBlock = null;
    }

    public void restart()
    {

 
        app.action_phase_input_view.enabled = false;
        app.planning_phase_input_view.enabled = true;

        resetGameBoard();

    }

    public void startlevel(int level)
    {

        this.level = level;
        level_string = level + ".txt";

        resetGameBoard();
        app.board_tile_view.reloadLevel();
       
    }

    //Load a new gameboard
    private void resetGameBoard()
    {

        app.action_btn.interactable = true;  

        app.action_phase_input_view.enabled = false;
        app.planning_phase_input_view.enabled = true;

        GameBoardModel old_gb = app.game_board_model;
        GameBoardModel new_gb = app.gameObject.AddComponent<GameBoardModel>();
        new_gb.loadLevel(level_string);

        //Swap and destroy old model
        app.game_board_model = new_gb;
        Destroy(old_gb);
    }

    //Check for notifactions
    public override void OnNotification(string p_event_path, UnityEngine.Object p_target, params object[] p_data)
    {

        switch (p_event_path)
        {


            case "game.win":
                {

                    //disable input
                    app.action_phase_input_view.enabled = false;
                    app.planning_phase_input_view.enabled = false;

                    // Play display the congratualtions screen.
                    if (level + 1 > maxLevel)
                    {
                        app.endScreen.enabled = true;
                        app.bg_music_controller.switchTracks(2);
                    }

                    //send end of game event here.
                    level = (level < maxLevel) ? level + 1 : maxLevel;

                    //Save the players progress
                    save_progress();

                    startlevel(level);

                    break;
                }
            case "game.lose":
                {

                    //disable input
                    app.action_phase_input_view.enabled = false;
                    app.planning_phase_input_view.enabled = false;

                    

                    resetGameBoard();

                    break;
                }

        }


    }

    public void save_progress()
    {
        System.IO.File.WriteAllText("Assets/Last_level.txt", level.ToString());

    }

}

