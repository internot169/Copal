using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
2. Trivia/Coins
Game win
3. Leaderboard + score
Wumpus Move, Bat/Pit
4. Check rubric for anything else
5. Unit Tests

Make stuff happen when in room with Wumpus

Dynamic Room Gen

TODO: Game End + Local leaderboard
Bat/Pit
Trivia/Coins
Dynamic Room Size
Shop as a Room + BUFFS
Save State
Unit Tests
Make Wumpus Fight
Tests + Music
Level system + good looking prefab
Minimap + wumpus fight
*/

public class Trivia : MonoBehaviour
{
    public string path;

    public GameObject triviaUI;

    public TMP_Text questionText;
    public TMP_Text[] choiceTexts;

    public Question[] questions;
    void Awake(){
        LoadData();
    }
    void LoadData(){
        var csvFile = Resources.Load<TextAsset>(path);
        var data = csvFile.text.Split('\n');
        questions = new Question[data.Length - 1];
        for (int i = 1; i < questions.Length; i++){
            var row = data[i].Split(',');
            questions[i] = new Question(row[0], row[1], row[2], row[3], row[4], row[5]);
        }
    }

    int questionIndex = 0;
    bool answered = false;
    bool correct = false;
    public IEnumerator LoadTrivia(int count, int needed){
        triviaUI.SetActive(true);
        int correctlyAnswered = 0;
        for (int i = 0; i < count; i++){
            // TODO: pre req needs to be numerical and regenerate
            questionIndex = Random.Range(0, questions.Length);
            questionText.text = questions[questionIndex].question;
            for (int j = 0; j < choiceTexts.Length; j++){
                choiceTexts[j].text = questions[questionIndex].choices[j];
            }
            yield return new WaitUntil(() => answered);
            answered = false;
            if (correct){
                correctlyAnswered++;
            }
            correct = false;
        }
        triviaUI.SetActive(false);
        if (correctlyAnswered >= needed){
            yield return true;
        }
        else{
            yield return false;
        }
    }    

    public void Answer(int index){
        answered = true;
        if (index == questions[questionIndex].answer){
            answered = true;
            correct = true;
        }
    }

}