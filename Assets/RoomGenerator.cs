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
    int wumpusRoom = Random.Range(0, 30);
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
            rooms[i].roomNum = i + 1;
            rooms[i].connectedTo = new int[3] {-1, -1, -1};
        }
        for (int i = 0; i < 30; i++)
        {
            // Build a circle first
            // Test mod 30 logic
            rooms[i].doors[0].next = rooms[(i + 1) % 30];
            rooms[i].connectedTo[0] = (i + 1) % 30;
            if ((i + 1) % 30 == wumpusRoom) {
                // cast rooms[i].doors[j] as BossTeleporter
            }

            // Load the rest of the links randomly
            // One feature is to include really necessary rooms at high-degree nodes
            for (int j = 1; j < 3; j++)
            {
                // Pick a random room, add it to connectedTo
                int rand = Random.Range(0, 30);
                // Keep generating until find a room that has not been already connected to by this node
                while (roomAlreadyConnected(rand, rooms[i].connectedTo)){
                    rand = Random.Range(0, 30);
                }
                
                // This indiscriminately adds doors to rooms (without regards to boss)
                rooms[i].doors[j].next = rooms[rand];
                rooms[i].connectedTo[j] = rand;
                if (rand == wumpusRoom){
                    // cast rooms[i].doors[j] as BossTeleporter
                }
            }
        }
    }

    private bool roomAlreadyConnected(int check, int[] connected){
        for (int i = 0; i < 3; i++)
        {
            if (connected[i] == check){
                return true;
            }
        }
        return false;
    }
}