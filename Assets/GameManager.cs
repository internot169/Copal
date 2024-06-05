using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{   
    public int turns = 0;
    public int arrows = 0;
    public int coins = 0;
    public int roomNum = 0;
    public int lives = 3;
    public TMP_Text roomText;
    public GameObject wumpusObj;

    public static GameManager instance;

    private bool fighting = false;

    // holding teleporter for movement on ui. 
    private Teleporter tp;

    private GameObject Player;

    private GameObject ArrowUI;

    private TextMeshProUGUI warning;

    private TextMeshProUGUI cointext;

    private GameObject ShopUI;
 
    void Start(){
        instance = this;
        Player = GameObject.Find("Player");
        ArrowUI = GameObject.Find("ArrowUI");
        ShopUI = GameObject.Find("ShopUI");
        cointext = GameObject.Find("Coins").GetComponent<TextMeshProUGUI>();
        ShopUI.SetActive(false);
        ArrowUI.SetActive(false);
        warning = GameObject.Find("Warnings").GetComponent<TextMeshProUGUI>();
    }
    public void bossFight(){
        // hide ui
        ArrowUI.SetActive(false);
        // hide cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        // enable the camera
        Player.GetComponentInChildren<MouseLook>().enabled = true;
        SceneManager.LoadScene("BossTesting", LoadSceneMode.Additive);
        
    }
    public void randomRoom(Collider other){
        Room[] rooms = GameObject.Find("GameManager").GetComponent<RoomGenerator>().rooms;
        roomNum = Random.Range(0, rooms.Length);
        Room random = rooms[roomNum];
        other.transform.position = new Vector3(random.spawnLocation.position.x, random.spawnLocation.position.y+5, random.spawnLocation.position.z);
    }

    // various UI and shop things. 
    public void spend(int amount){
        if (coins == 0){
            lose(0);
        } else {
            coins -= amount;
        }
    }

    public void OpenShop(){
        //update coins
        cointext.text = "coins: " +coins;
        ShopUI.SetActive(true);
        ArrowUI.SetActive(false);
    }

    public void CloseShop(){
        ShopUI.SetActive(false);
        ArrowUI.SetActive(true);
    }

    public void win(int wumpus){
        score(wumpus);
    }
    public void lose(int wumpus){
        if (lives <= 1){
            score(wumpus);
        }else{
            lives-= 1;
        }
    }
    public void score(int wumpus) {
        int score = 100 - turns + coins + (5 * arrows) + wumpus;
        Debug.Log(score);
        // Write to file, rank the scores, remove if not top 10, name, seed
    }
    public void shoot(){
        //shoot wumpus;
        Debug.Log("shoot");
        // move the player
        // do the custom handling here. 
        if (tp is BossTeleporter){
            bossFight();
        }else{
            // added else for safety. 
            move();
        }
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
                win(50);
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