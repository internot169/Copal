using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorialObj;
    public GameObject leaderboard;
    public void playGame(){
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void leaderBoard(){
        leaderboard.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void backLeaderboard(){
        mainMenu.SetActive(true);
        leaderboard.SetActive(false);
    }

    public void tutorial(){
        tutorialObj.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void backTutorial(){
        mainMenu.SetActive(true);
        tutorialObj.SetActive(false);
    }
}
