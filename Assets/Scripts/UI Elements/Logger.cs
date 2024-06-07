using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Logger : MonoBehaviour
{
    // helper method to log text into the logger .
    public TMP_Text logText;

    // Logs data to logger text
    public void log(string text) {
        logText.text = text + "\n" + logText.text;
    }
}