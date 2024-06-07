using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Logger : MonoBehaviour
{
    // helper method to log text into the logger .
    public TMP_Text logText;
    // update the text with a newline and the old logs. 
    public void log(string text) {
        logText.text = text + "\n" + logText.text;
    }
}