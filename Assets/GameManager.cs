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