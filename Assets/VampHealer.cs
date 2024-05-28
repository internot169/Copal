using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampHealer : AOEApplier
{
    // Start is called before the first frame update

    PlayerInfo Player;

    public int healing;
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void MarkStacks()
    {
        Player.Heal(healing);
    }
}
