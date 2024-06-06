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
    public int arrows = 3;
    public int coins = 0;
    private int roomNum = 0;
    public int lives = 3;
    public TMP_Text roomText;
    public GameObject wumpusObj;
    public Transform wumpusSpawnLoc;

    public static GameManager instance;

    private bool lost = false;

    private bool fighting = false;

    // holding teleporter for movement on ui. 
    private Teleporter tp;

    public GameObject Player;

    public GameObject pauseUI;

    public TextMeshProUGUI warning;

    public TextMeshProUGUI cointext;

    public GameObject ShopUI;
    private TextMeshProUGUI Inventory;

    public Room currentRoom(){
        return GetComponent<RoomGenerator>().rooms[roomNum];
    }
    public void pauseGame(){
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void playGame(){
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        Time.timeScale = 1;
    }
 
    void Start(){
        arrows = 3;
        instance = this;
        ShopUI.SetActive(false);
        pauseUI.SetActive(false);
        Inventory = GameObject.Find("Inventory").GetComponent<TextMeshProUGUI>();
    }

    public void bossFight(){
        // load wumpus scene
        wumpusObj.SetActive(true);
        Player.transform.position = wumpusSpawnLoc.position;
        fighting = true;
    }
    public void randomRoom(){
        roomNum = Random.Range(0, 30);
        move(roomNum, true);
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
        pauseUI.SetActive(false);
    }

    public void CloseShop(){
        ShopUI.SetActive(false);
        pauseUI.SetActive(true);
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
            } else {
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
    public void shoot(int roomNum){
        //shoot wumpus;
        Debug.Log("shoot");
        if (arrows > 0){
            arrows--;
            
            // move the player
            // do the custom handling here.
            if (tp is BossTeleporter){
                bossFight();
            } else {
                move(roomNum, true);
            }
        }
    }

    public void move(int room, bool disable){
        Room[] rooms = GameObject.Find("GameManager").GetComponent<RoomGenerator>().rooms;
        rooms[roomNum].visited = true;
        if (disable) {
            rooms[roomNum].gameObject.SetActive(false);
        }
    
        roomNum = room;
        if (!rooms[roomNum].visited){
            coins++;
        }
        turns++;
        
        // hide cursor
        playGame();
        // enable the camera
        Player.GetComponentInChildren<MouseLook>().enabled = true;
        // tell the teleporter it came from to move the player
        // only if it is not being shot
        if (tp != null){
            tp.MovePlayer(Player.GetComponent<Collider>());
            tp = null;
        }
    }

    public void Update(){
        roomText.text = "Room " + roomNum.ToString();
        Inventory.text = "coins: " + coins+"\narrows: " + arrows + "\nlives: " + lives;
        if (fighting) {
            if (wumpusObj.GetComponent<Shootable>().currentHealth <= 0){
                win(50);
                fighting = false;
            }
        }

        if (Input.GetKey(KeyCode.Escape)){
            pauseGame();
            pauseUI.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.P)){
            playGame();
            pauseUI.SetActive(false);
        }
    }

    public void UpdateTp(Teleporter teleporter){
        tp = teleporter;
    }

    public void UpdateWarnings(string warnings){
        warning.SetText(warnings);
    }
}