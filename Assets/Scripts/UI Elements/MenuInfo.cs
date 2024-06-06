using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuInfo : MonoBehaviour
{
    public static MenuInfo instance;
    public string name = "";
    public TMP_Text nameInput;
    void Awake(){
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update(){
        if (nameInput != null){
            name = nameInput.text;
        }
    }
}
