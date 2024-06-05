using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    // TODO: Make it room specific
    public float roomLength;
    public float roomBuffer;
    public Room[] rooms = new Room[30];
    int wumpusRoom;
    public void Awake(){
        wumpusRoom = Random.Range(0, 30);
        // Make the base room
        createRooms();
    }

    public void moveMob(string mobType, int currRoom){
        int room = 0;
        if (mobType == "Bat"){
            room = Random.Range(0, 30);
        } else if (mobType == "Wumpus"){
            int range = Random.Range(2, 4);
            // Randomly traverse the graph two rooms forward
            for (int i = 0; i < range; i++)
            {
                room = rooms[room].connectedTo[Random.Range(0, 3)];
            }
        }
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Remove old
                if (rooms[i].doors[j].next.roomNum == currRoom){
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Destroy(obj.GetComponent<Teleporter>());
                    rooms[i].doors[j] = obj.AddComponent<Teleporter>();
                }
                // Add new
                if (rooms[i].doors[j].next.roomNum == room){
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Destroy(obj.GetComponent<Teleporter>());
                    if (mobType == "Bat") {
                        rooms[i].doors[j] = obj.AddComponent<BatTeleporter>();
                    } else if (mobType == "Wumpus") {
                        rooms[i].doors[j] = obj.AddComponent<BossTeleporter>();
                    }
                }
            }
        }
    }
    
    public void createRooms(){
        // TODO: CHANGE THE PREFAB BASED ON THE TYPE OF ROOM WE ARE LOADING
        // We're loading the rooms along the x axis
        float nextRoomPos = 0f;

        int wumpusRoom = Random.Range(0, 30);
        int batRoom = -1;
        int pitRoom = -1;
        while (batRoom != wumpusRoom){
            batRoom = Random.Range(0, 30);
        }
        while (pitRoom != wumpusRoom && pitRoom != batRoom){
            pitRoom = Random.Range(0, 30);
        }
        
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
            rooms[i].doors[0].next = rooms[(i + 1) % 30];
            rooms[i].connectedTo[0] = (i + 1) % 30;

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
                
                // This should for all links to the specific room change the teleporter to the correct type
                if (rand == wumpusRoom){
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Destroy(obj.GetComponent<Teleporter>());
                    rooms[i].doors[j] = obj.AddComponent<BossTeleporter>();
                } else if (rand == batRoom){
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Destroy(obj.GetComponent<Teleporter>());
                    rooms[i].doors[j] = obj.AddComponent<BatTeleporter>();
                } else if (rand == pitRoom) {
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Destroy(obj.GetComponent<Teleporter>());
                    rooms[i].doors[j] = obj.AddComponent<PitTeleporter>();
                }

                rooms[i].doors[j].next = rooms[rand];
                rooms[i].connectedTo[j] = rand;
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