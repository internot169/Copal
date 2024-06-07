using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public delegate void Callback(bool answer);

public class Trivia : MonoBehaviour
{
    public string path = "triviav3";

    public GameObject triviaUI;

    public TMP_Text questionText;
    public TMP_Text[] choiceTexts;

    public Question[] questions;

    public Question[] answered;

    void Awake(){
        LoadData();
    }
    public List<Question> readJsonArray(string json){
        List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(json);
        return questions;
    }
    void LoadData(){
        var file = Resources.Load<TextAsset>(path);
        Debug.Log((string) file.text);
        List<Question> list = readJsonArray((string) file.text);
        questions = new Question[list.Count];
        for (int i = 0; i < questions.Length; i++){
            list[i].setup();
            questions[i] = list[i];
        }
    }

    int questionIndex = 0;
    bool answeredQ = false;
    bool correct = false;
    public string getUnknownAnswer(){
        int index = Random.Range(0, questions.Length);
        while (questions[index].answered || questions[index].known){
            index = Random.Range(0, questions.Length);
        }
        questions[index].known = true;
        return questions[index].lorePrereq;
    }
    public IEnumerator LoadTrivia(int count, int needed, Callback callback, bool play = true){
        GameManager mg = GameObject.Find("GameManager").GetComponent<GameManager>();
        mg.spend(count);
        triviaUI.SetActive(true);
        mg.pauseGame();
        int correctlyAnswered = 0;
        for (int i = 0; i < count; i++){
            questionIndex = Random.Range(0, questions.Length);

            int iterations = 0;
            while ((questions[questionIndex].answered || !questions[questionIndex].known) && iterations < (2 * questions.Length)){
                questionIndex = Random.Range(0, questions.Length);
                iterations++;
            }
            if (iterations >= (2 * questions.Length)){
                // GPT question
            }
            questionText.text = questions[questionIndex].question;


            // TODO: Make sure you know it else GPT above
            // TODO: add GPT randomly
            for (int j = 0; j < choiceTexts.Length; j++){
                choiceTexts[j].text = questions[questionIndex].choices[j];
            }
            
            yield return new WaitUntil(() => answeredQ);
            answeredQ = false;
            if (correct){
                correctlyAnswered++;
            }
            correct = false;          
        }
        if (play){  
            mg.playGame();
        }
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