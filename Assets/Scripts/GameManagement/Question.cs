using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string lorePrereq;
    public string question;
    public int answer = 0;
    public string[] choices;
    public bool known = false;
    public bool answered = false;
    public Question(string lorePrereq, string question, string answer1, string answer2, string answer3, string answer4){
        this.lorePrereq = lorePrereq;
        this.question = question;
        this.choices = new string[]{answer1, answer2, answer3, answer4};
        shuffle();
    }

    // Sattolo's Algorithm
    void shuffle(){
        int n = choices.Length - 1;
        while (n > 1) 
        {
            int k = Random.Range(0, n);
            if (n == answer){
                answer = k;
            }
            string temp = choices[n];
            choices[n] = choices[k];
            choices[k] = temp;
            n--;
        }
    }
}