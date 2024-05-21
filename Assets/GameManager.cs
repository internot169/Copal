using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{   
    public GameObject wumpusObj;

    public static GameManager instance;

    private bool fighting = false;
 
    void Start(){
        instance = this;
    }
    public void bossFight(){
        wumpusObj.SetActive(true);
        fighting = true;
    }
    public void randomRoom(Collider other){
        Room[] rooms = GameObject.Find("GameManager").GetComponent<RoomGenerator>().rooms;
        Room random = rooms[Random.Range(0, rooms.Length)];
        other.transform.position = new Vector3(random.spawnLocation.position.x, random.spawnLocation.position.y+5, random.spawnLocation.position.z);
    }

    public void win(){
        // Calculate score and stuff
        // Load to server
        Debug.Log("YOU WON");
    }

    public void Update(){
        if (fighting) {
            if (wumpusObj.GetComponent<Shootable>().currentHealth <= 0){
                win();
                fighting = false;
            }
        }
    }
}