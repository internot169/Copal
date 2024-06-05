using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using JetBrains.Annotations;
using System.Net.Http;

public class GameManager : MonoBehaviour
{   
    private static readonly HttpClient client = new HttpClient();
    private static readonly string url = "http://localhost:5000/addscore";
    public int turns = 0;
    public int arrows = 0;
    public int coins = 0;
    public int roomNum = 0;
    public int lives = 3;
    public TMP_Text roomText;
    public GameObject wumpusObj;

    public static GameManager instance;

    private bool lost = false;

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
        wumpusObj.SetActive(true);
        fighting = true;
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
            lose();
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
        // Display win screen
        score(wumpus);
    }
    public bool lose(){
        if (!lost){
            if (lives <= 1){
                score(0);
                lost = true;
                return true;
            }else{
                lives-= 1;
                return false;
            }
        } else {
            return true;
        }
    }
    public async void score(int wumpus) {
        int score = 100 - turns + coins + (5 * arrows) + wumpus;
        
        var values = new Dictionary<string, string>
        {
            { "password", "resin" },
            { "name", "JS" },
            { "score", score.ToString() },
            { "turns", turns.ToString() },
            { "coins", coins.ToString() },
            { "arrows", arrows.ToString() },
            { "wumpus", wumpus.ToString() }
        };
        
        var content = new FormUrlEncodedContent(values);
        try{
            var response = await client.PostAsync(url, content);
        } catch (HttpRequestException e){
            Debug.Log(e.Message);
        }
    }
    public void shoot(){
        //shoot wumpus;
        Debug.Log("shoot");
        // Decrease arrow
        // move the player
        // do the custom handling here.
        if (tp is BossTeleporter){
            bossFight();
        }
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