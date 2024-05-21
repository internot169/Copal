using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{   
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

    public void win(){
        // Calculate score and stuff
        // Load to server
        Debug.Log("YOU WON");
    }

    public void Shoot(){
        //shoot wumpus;
        Debug.Log("shoot");
        // move the player
        // do the custom handling here. 
        Move();
    }

    public void Move(){
        Debug.Log("move");
        // hide ui
        ArrowUI.SetActive(false);
        // hide cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        // enable the camera
        Player.GetComponentInChildren<MouseLook>().enabled = true;
        // tell the teleporter it came from to move the player
        // should we null the tp after?
        tp.GetComponent<Teleporter>().MovePlayer(Player.transform);
        
    }

    public void Update(){
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

    public void UpdateWarnings(String warnings){
        warning.SetText(warnings);
    }
}