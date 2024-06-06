using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowShoot : MonoBehaviour
{
    public TMP_Text[] roomTexts;
    public GameManager gameManager;
    public void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < 3; i++)
        {
            roomTexts[i].transform.parent.gameObject.SetActive(false);
        }
    }
    public void enableChoices(){
        Room room = gameManager.currentRoom();
        for (int i = 0; i < 3; i++)
        {
            roomTexts[i].transform.parent.gameObject.SetActive(!roomTexts[i].transform.parent.gameObject.activeSelf);
            roomTexts[i].text = room.connectedTo[i].ToString();
        }
    }

    public void fire(int buttonIndex){
        int chosen = int.Parse(roomTexts[buttonIndex].text);
        for (int i = 0; i < 3; i++)
        {
            roomTexts[i].transform.parent.gameObject.SetActive(false);
        }
        gameManager.shoot(chosen);
    }
}
