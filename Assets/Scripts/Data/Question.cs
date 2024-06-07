using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string lorePrereq { get; set; }
    public string question { get; set; }
    public string answer1 { get; set; }
    public string answer2 { get; set; }
    public string answer3 { get; set; }
    public string answer4 { get; set; }
    public int answer = 3;
    public string[] choices;
    public bool known = false;    
    public bool answered = false;
    public void setup(){
        this.choices = new string[]{answer1, answer2, answer3, answer4};
        int index = Random.Range(0, choices.Length);
        swap(choices, index, answer);
        answer = index;
    }
    
    void swap(string[] arr, int i, int j){
        string temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}