using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    // TODO: Make it room specific
    public float roomLength;
    public float roomBuffer;
    public void Awake(){
        // Make the base room
        createRooms();
    }
    public void createRooms(){
        // TODO: CHANGE THE PREFAB BASED ON THE TYPE OF ROOM WE ARE LOADING
        // We're loading the rooms along the x axis
        float nextRoomPos = 0f;
        Room[] rooms = new Room[30];
        for (int i = 0; i < 30; i++)
        {
            GameObject ob = Instantiate(roomPrefab, new Vector3(nextRoomPos, 0, 0), Quaternion.identity);
            nextRoomPos += roomLength + roomBuffer;
            ob.name = "Room " + ((int) i + 1).ToString();
            rooms[i] = (Room) ob.GetComponent<Room>();
        }
        for (int i = 0; i < 30; i++)
        {
            Room curr = rooms[i];
            for (int j = rooms[i].nextToAssign; j < 3; j++)
            {
                // Pick a random room, add it to connectedTo
                int rand = Random.Range(0, 30);
                Debug.Log(rand);
                // Keep generating until find an empty room
                while (rooms[rand].nextToAssign > 2){
                    rand = Random.Range(0, 30);
                }

                // TODO: UNIQUENESS
                
                // This indiscriminately adds doors to rooms (without regards to boss)
                rooms[i].doors[j].next = rooms[rand];

                // This indiscriminately adds doors to rooms (without regards to boss)
                // IE, the boss system is currently not working and we need some reinstantiation logic if we have a room that is a boss
                rooms[rand].doors[rooms[rand].nextToAssign].next = rooms[i];
                rooms[rand].nextToAssign++;
            }
        }
    }
}