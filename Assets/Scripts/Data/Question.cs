using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    // Hint for the question
    public string lorePrereq { get; set; }
    // Question and answers
    public string question { get; set; }
    public string answer1 { get; set; }
    public string answer2 { get; set; }
    public string answer3 { get; set; }
    public string answer4 { get; set; }
    // Answer index
    public int answer = 3;
    // Answer choice array
    public string[] choices;
    public bool known = false;    
    public bool answered = false;
    // Initialize array and randomize locations of answers
    public void setup(){
        this.choices = new string[]{answer1, answer2, answer3, answer4};
        int index = Random.Range(0, choices.Length);
        swap(choices, answer, index);
        answer = index;
    }
    
    void swap(string[] arr, int i, int j){
        string temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}