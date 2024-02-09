using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : Teleporter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // so basically teleporter always calls an activate unique function
    public override void ActivateUnique(RoomManager nextRoom){
        nextRoom.isBossRoom();
    }
}
