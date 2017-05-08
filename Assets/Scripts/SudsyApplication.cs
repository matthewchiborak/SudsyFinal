using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SudsyElement : MonoBehaviour
{

    // Gives access to the application and all instances.
    public SudsyApplication app { get { return GameObject.FindObjectOfType<SudsyApplication>(); } }
}



public class SudsyApplication : MonoBehaviour
{

    //For access
    public PhaseSwitchController phase_switch_controller;
    public PlanningPhaseInputView planning_phase_input_view;
    public ActionPhaseInputView action_phase_input_view;
    public GameBoardModel game_board_model;
    public BoardTileView board_tile_view;
    public Button action_btn;

    public SoundEffectController sfx_controller;
    public BackgroundMusicController bg_music_controller;

    public Image endScreen;

    //Keep track of what level the player is currently on
    public int start_level;

    // Iterates all Controllers and delegates the notification data
    public void Notify(string p_event_path, Object p_target, params object[] p_data)
    {
        SudsyController[] controller_list = GetAllControllers();
        foreach (SudsyController c in controller_list)
        {
            c.OnNotification(p_event_path, p_target, p_data);
        }
    }

    // Fetches all scene Controllers.
    public SudsyController[] GetAllControllers()
    {
        return GameObject.FindObjectsOfType<SudsyController>();
    }

    // Use this for initialization
    void Start()
    {
        ActorEnemyFactoryMethods.AttachApp(this);

        //start_level = 1;
        start_level = int.Parse(File.ReadAllText("Assets/Last_level.txt"));
        phase_switch_controller.startlevel(start_level);


        Vector3 newCamPos = new Vector3(game_board_model.width / 2 - 0.5f, game_board_model.height / 2, -7.5f);
        Camera.main.transform.position = (newCamPos);
    }
}

