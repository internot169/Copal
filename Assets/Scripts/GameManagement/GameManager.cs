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
    public Logger logger;
    public string name = "";

    bool paused = false;

    bool escape_released = true;

    public MenuInfo info;
    public static GameManager instance;

    private static readonly HttpClient client = new HttpClient();
    private static readonly string url = "http://localhost:5000/addscore";

    public int turns = 0;
    public int arrows = 3;
    public int coins = 0;
    private int roomNum = 0;
    public int lives = 3;
    public TMP_Text roomText;
    public GameObject wumpusRoom;
    public GameObject bossObject;
    public Transform wumpusSpawnLoc;

    private bool lost = false;

    private bool fighting = false;

    public GameObject Player;

    public GameObject pauseUI;

    public TextMeshProUGUI warning;


    public bool testmode;

    public TMP_Text testUI;

    public GameObject ShopUI;
    public TextMeshProUGUI Inventory;

    public GameObject BatUI;
    public GameObject PitUI;
    public GameObject WinUI;
    public GameObject LoseUI;

    public Room currentRoom(){
        return GetComponent<RoomGenerator>().rooms[roomNum];
    }
    public void pauseGame(){
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0;
        paused = true;
    }

    public void playGame(){
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        Time.timeScale = 1;
        paused = false;
    }
 
    void Start(){
        arrows = 3;
        instance = this;
        ShopUI.SetActive(false);
        pauseUI.SetActive(false);
        BatUI.SetActive(false);
        PitUI.SetActive(false);
        WinUI.SetActive(false);
        LoseUI.SetActive(false);
        Player.transform.position = currentRoom().spawnLocation.position;
        info = MenuInfo.instance;
        name = info.name;
        if (name == "test"){
            testmode = true;
        }
    }

    public void bossFight(){
        // load wumpus scene
        wumpusRoom.SetActive(true);
        Player.transform.position = wumpusSpawnLoc.position;
        fighting = true;
    }
    public void randomRoom(){
        roomNum = Random.Range(0, 30);
        move(roomNum, true, null);
    }

    // various UI and shop things. 
    public void spend(int amount){
        if (coins - amount < 1){
            lose();
        } else {
            coins -= amount;
        }
    }

    public void OpenShop(){
        ShopUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void CloseShop(){
        ShopUI.SetActive(false);
        pauseUI.SetActive(true);
    }


    public void win(int wumpus){
        // Display win screen
        pauseGame();
        WinUI.SetActive(true);
        score(wumpus);
    }
    public bool lose(){
        if (!lost){
            if (lives <= 1){
                score(0);
                lost = true;
                pauseGame();
                LoseUI.SetActive(true);
                return true;
            } else {
                lives-= 1;
                return false;
            }
        } else {
            pauseGame();
            LoseUI.SetActive(true);
            return true;
        }
    }
    public async void score(int wumpus) {
        int score = 100 - turns + coins + (5 * arrows) + wumpus;
        
        var values = new Dictionary<string, string>
        {
            { "password", "resin" },
            { "name", name },
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
    public void shoot(int roomNum, Teleporter tp){
        //shoot wumpus;
        if (arrows > 0){
            arrows--;
            
            // move the player
            // do the custom handling here.
            if (tp is BossTeleporter){
                logger.log("Beep bop beep ... you hit me. Now, I'm angry. Unleashing backpropagation. Prepare to die.");
                bossFight();
                playGame();
            } else {
                logger.log("Ha! Missed me! I might automate you away after all ...");
                move(roomNum, true, tp);
            }
        }
    }

    public void move(int room, bool disable, Teleporter tp){
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
        
        playGame();

        logger.log(GameObject.Find("Trivia").GetComponent<Trivia>().getUnknownAnswer());

        // enable the camera
        Player.GetComponentInChildren<MouseLook>().enabled = true;
        // tell the teleporter it came from to move the player
        // only if it is not being moved randomly
        if (tp != null){
            tp.MovePlayer(Player.GetComponent<Collider>());
            tp = null;
        }
    }

    public void Update(){
        roomText.text = "Room " + roomNum.ToString();
        Inventory.text = "Coins: " + coins+ "\nArrows: " + arrows + "\nLives: " + lives + "\nTurns: " + turns;
        if (fighting) {
            if (bossObject.GetComponent<Shootable>().currentHealth <= 0){
                win(50);
                fighting = false;
            }
        }

        // will only check once per frame. 
        // thus, there is no case of it flickering on and off. 
        Debug.Log(escape_released);
        if (Input.GetKey(KeyCode.Escape) && !paused && escape_released){
            pauseGame();
            pauseUI.SetActive(true);
            if (testmode){
                testUI.gameObject.SetActive(true);
            } else {
                testUI.gameObject.SetActive(false);
            }
            // since the escape key was just pressed, mark it as being unable to be used until the key gets released. 
            escape_released = false;
        }
        // check or no check for paused, either is fine. 
        else if (Input.GetKey(KeyCode.Escape) && paused && escape_released){
            playGame();
            pauseUI.SetActive(false);
            // since the escape key was just pressed, mark it as being unable to be used until the key gets released. 
            escape_released = false;
        }
        // when escape gets released, Input.GetKeyUp returns true for 1 frame. 
        // This will persist and reset the ability for the escape button to be repressed. 
        if (Input.GetKeyUp(KeyCode.Escape)){
            escape_released = true;
        }
    }
}