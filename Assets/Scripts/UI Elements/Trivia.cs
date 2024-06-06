using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void Callback(bool answer);

public class Trivia : MonoBehaviour
{
    public string path;

    public GameObject triviaUI;

    public TMP_Text questionText;
    public TMP_Text[] choiceTexts;

    public Question[] questions;

    public Question[] answered;
    void Awake(){
        LoadData();
    }
    void LoadData(){
        var csvFile = Resources.Load<TextAsset>(path);
        var data = csvFile.text.Split('\n');
        questions = new Question[data.Length - 1];
        answered = new Question[data.Length - 1];
        for (int i = 1; i < questions.Length; i++){
            var row = data[i].Split(',');
            questions[i] = new Question(row[0], row[1], row[2], row[3], row[4], row[5]);
        }
    }

    int questionIndex = 0;
    bool answeredQ = false;
    bool correct = false;
    public string getUnknownAnswer(){
        int index = Random.Range(0, questions.Length);
        while (questions[index] == null){
            index = Random.Range(0, questions.Length);
        }

        return questions[index].choices[questions[index].answer];
    }
    public IEnumerator LoadTrivia(int count, int needed, Callback callback){
        GameManager mg = GameObject.Find("GameManager").GetComponent<GameManager>();
        mg.spend(count);
        triviaUI.SetActive(true);
        mg.pauseGame();
        int correctlyAnswered = 0;
        for (int i = 0; i < count; i++){
            questionIndex = Random.Range(0, questions.Length);
            while (questions[questionIndex] == null){
                questionIndex = Random.Range(0, questions.Length);
            }
            questionText.text = questions[questionIndex].question;
            answered[questionIndex] = questions[questionIndex];


            // TODO: randomize choices
            for (int j = 0; j < choiceTexts.Length; j++){
                choiceTexts[j].text = questions[questionIndex].choices[j];
            }
            
            yield return new WaitUntil(() => answeredQ);
            answeredQ = false;
            if (correct){
                correctlyAnswered++;
            }
            correct = false;

            questions[questionIndex] = null;            
        }
        mg.playGame();
        triviaUI.SetActive(false);
        if (correctlyAnswered >= needed){
            callback(true);
        }
        else{
            callback(false);
        }
    }    

    public void Answer(int index){
        answeredQ = true;
        if (index == questions[questionIndex].answer){
            correct = true;
        }
    }

}