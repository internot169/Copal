using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;


public class GameManager : MonoBehaviour
{   
    public int coins = 0;
    public int roomNum = 0;
    public TMP_Text roomText;
    public GameObject wumpusObj;

    public static GameManager instance;

    private bool fighting = false;

    // holding teleporter for movement on ui. 
    private Teleporter tp;

    private GameObject Player;

    private GameObject ArrowUI;

    private TextMeshProUGUI warning;
 
    void Start(){
        instance = this;
        Player = GameObject.Find("Player");
        ArrowUI = GameObject.Find("ArrowUI");
        ArrowUI.SetActive(false);
        warning = GameObject.Find("Warnings").GetComponent<TextMeshProUGUI>();
    }
    public void bossFight(){
        wumpusObj.SetActive(true);
        fighting = true;
    }
    public void randomRoom(Collider other){
        Room[] rooms = GameObject.Find("GameManager").GetComponent<RoomGenerator>().rooms;
        roomNum = Random.Range(0, rooms.Length);
        Room random = rooms[roomNum];
        other.transform.position = new Vector3(random.spawnLocation.position.x, random.spawnLocation.position.y+5, random.spawnLocation.position.z);
    }

    public void spend(int amount){
        if (coins == 0){
            lose();
        } else {
            coins -= amount;
        }
    }

    public void win(){
        // Calculate score and stuff
        Debug.Log("YOU WON");
    }
    public void lose(){
        // Calculate score and stuff
        Debug.Log("YOU LOST");
    }
    public void shoot(){
        //shoot wumpus;
        Debug.Log("shoot");
        // move the player
        // do the custom handling here. 
        move();
    }

    public void move(){
        Debug.Log("move");
        // hide ui
        ArrowUI.SetActive(false);
        // hide cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        // enable the camera
        Player.GetComponentInChildren<MouseLook>().enabled = true;
        // tell the teleporter it came from to move the player
        tp.MovePlayer(Player.GetComponent<Collider>());
    }

    public void Update(){
        roomText.text = "Room " + roomNum.ToString();
        if (fighting) {
            if (wumpusObj.GetComponent<Shootable>().currentHealth <= 0){
                win();
                fighting = false;
            }
        }
    }

    public void UpdateTp(Teleporter teleporter){
        tp = teleporter;
    }

    public void UpdateWarnings(string warnings){
        warning.SetText(warnings);
    }
}