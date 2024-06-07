using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Http;
using Newtonsoft.Json;

public class Menu : MonoBehaviour
{
    // Main Menu code
    public GameObject mainMenu;
    public GameObject tutorialObj;
    public GameObject leaderboard;

    public Transform scoresTransform;
    public ScoreUI scoreUI;

    private static readonly HttpClient client = new HttpClient();
    private static readonly string url = "http://localhost:5000/";

    public List<Score> readJsonArray(string json){
        // Newtonsoft takes care of file integrity
        List<Score> scores = JsonConvert.DeserializeObject<List<Score>>(json);
        return scores;
    }

    public async void getscores() {       
        try{
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
    
            var jsonResponse = await response.Content.ReadAsStringAsync();
            List<Score> scores = readJsonArray(jsonResponse);
            foreach (Score s in scores){
                scoreUI.nameText.text += "\n" + s.name;
                scoreUI.scoreText.text += "\n" + s.score.ToString();
                scoreUI.turnsText.text += "\n" + s.turns.ToString();
                scoreUI.coinsText.text += "\n" + s.coins.ToString();
                scoreUI.arrowsText.text += "\n" + s.arrows.ToString();
                scoreUI.wumpusText.text += "\n" + s.wumpus.ToString();
            }
        } catch (HttpRequestException e){
            Debug.Log(e.Message);
            // show error
        }
    }

    public void playGame(){
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void leaderBoard(){
        getscores();
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
