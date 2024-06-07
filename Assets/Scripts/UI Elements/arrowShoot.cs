using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager.Requests;

public class ArrowShoot : MonoBehaviour
{
    // list of text elements that will be changed to reflect the room to shoot arrows to. 
    public TMP_Text[] roomTexts;
    // reference to game manager to access room numbers later. 
    // public so that we can assign in editor in case. 
    public GameManager gameManager;
    public void Awake(){
        // assign and find gm
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // hide the buttons. 
        for (int i = 0; i < 3; i++)
        {
            roomTexts[i].transform.parent.gameObject.SetActive(false);
        }
    }

    // method to enable the choice of room to shoot arrows into. 
    public void enableChoices(){
        // get the current room from gm. 
        Room room = gameManager.currentRoom();
        // find the next rooms, and attach their numbers to the buttons. 
        for (int i = 0; i < 3; i++)
        {
            roomTexts[i].transform.parent.gameObject.SetActive(!roomTexts[i].transform.parent.gameObject.activeSelf);
            roomTexts[i].text = room.connectedTo[i].ToString();
        }
    }

    // fire arrow into the room. 
    public void fire(int buttonIndex){
        // get the current room, then find the room that the button shot into. 
        // grab from text as it's not attached to the button. 
        Room room = gameManager.currentRoom();
        int chosen = int.Parse(roomTexts[buttonIndex].text);
        for (int i = 0; i < 3; i++)
        {
            // reset the texts and hide them. 
            roomTexts[i].transform.parent.gameObject.SetActive(false);
        }
        // call the shoot method to the door from gm. 
        gameManager.shoot(chosen, room.doors[buttonIndex]);
    }
}
