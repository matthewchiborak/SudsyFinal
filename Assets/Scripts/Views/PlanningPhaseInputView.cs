using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 
/// Handles mouse input during the planning phase of the game. 
/// 
/// Enable this in a controller during the planning phase, disable when user moves to 
/// action phase.
/// 
/// </summary>
public class PlanningPhaseInputView : SudsyView
{
    bool mouse_drag = false;
    Vector2 prevGridPosition;

    void Update()
    {
        //Mouse down
        if (!mouse_drag && Input.GetButtonDown("Fire1"))
        {
            mouse_drag = true;

            //Figure out which object was clicked on
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casts the ray and get the first game object hit            
            RaycastHit hit;

            //If there is a hit
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.transform.position;

                //Notify the game board of the clicked on point.
                prevGridPosition = point;

                app.Notify(SudysNotifications.UserObjectClick, this, prevGridPosition);

            }
            else
            {
                //prevGridPosition = voidGridPosition;
                //app.Notify(SudysNotifications.UserObjectClearclick, this);
            }
        }

        //Check for dragging events
        if (mouse_drag && Input.GetButton("Fire1"))
        {

            //Do another ray cast and see it player has dragged the mouse to a different location. Update the position of the dragged object accordingly
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Casts the ray and get the first game object hit
            if (Physics.Raycast(ray, out hit))
            {

                Vector3 point = hit.transform.position;

                Vector2 newGridPosition = point;

                //If we're over a new position, update the game board
                if (newGridPosition != prevGridPosition)
                {

                    app.Notify(SudysNotifications.UserObjectDrag, this, newGridPosition);
                }

                prevGridPosition = newGridPosition;
            }
        }

        if (mouse_drag && Input.GetButtonUp("Fire1"))
        {
            mouse_drag = false;
        }
    }

}

