using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    
    public float roomLength = 100f;
    public float roomBuffer = 100f;
    public Room[] rooms = new Room[30];
    public int wumpusRoom;
    public int batRoom;
    public int pitRoom;
    public void Awake(){
        wumpusRoom = Random.Range(1, 30);
        batRoom = Random.Range(1, 30);
        pitRoom = Random.Range(1, 30);
        // Make the base room
        createRooms();
    }

    public void moveMob(string mobType, int currRoom){
        int room = 0;
        if (mobType == "Bat"){
            room = Random.Range(0, 30);
            while(room == currRoom || room == wumpusRoom || room == pitRoom){
                room = rooms[room].connectedTo[Random.Range(0, 3)];
            }
            batRoom = room;
        } else if (mobType == "Wumpus"){
            int range = Random.Range(2, 4);
            // Randomly traverse the graph two rooms forward
            for (int i = 0; i < range; i++)
            {
                room = rooms[room].connectedTo[Random.Range(0, 3)];
            }
            while(room == currRoom || room == batRoom || room == pitRoom){
                room = rooms[room].connectedTo[Random.Range(0, 3)];
            }
            wumpusRoom = room;
        }
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Remove old
                if (rooms[i].doors[j].next.roomNum == currRoom){
                    GameObject obj = rooms[i].doors[j].gameObject;
                    Room tempRoom = rooms[i].doors[j].next;
                    
                    Destroy(obj.GetComponent<Teleporter>());
                    rooms[i].doors[j] = obj.AddComponent<Teleporter>();
                    rooms[i].doors[j].next = tempRoom;
                    rooms[i].doors[j].Awake();
                }
                // Add new
                changeTeleporters(i, j, room);
            }
        }
    }
    
    public void createRooms(){
        // We're loading the rooms along the x axis
        float nextRoomPos = 0f;
        
        while (batRoom == wumpusRoom){
            batRoom = Random.Range(0, 30);
        }
        while (pitRoom == wumpusRoom || pitRoom == batRoom){
            pitRoom = Random.Range(0, 30);
        }
        
        for (int i = 0; i < 30; i++)
        {
            GameObject ob = Instantiate(roomPrefabs[Random.Range(0, 6)], new Vector3(nextRoomPos, 0, 0), Quaternion.identity);
            nextRoomPos += roomLength + roomBuffer;
            ob.name = "Room " + ((int) i).ToString();
            if (i != 0){
                ob.SetActive(false);
            }
            rooms[i] = (Room) ob.GetComponent<Room>();
            rooms[i].roomNum = i;
            rooms[i].connectedTo = new int[3] {-1, -1, -1};
        }
        for (int i = 0; i < 30; i++)
        {
            // Build a circle first
            rooms[i].doors[0].next = rooms[(i + 1) % 30];
            rooms[i].connectedTo[0] = (i + 1) % 30;

            changeTeleporters(i, 0, (i + 1) % 30);
            
            // Load the rest of the links randomly
            for (int j = 1; j < 3; j++)
            {
                // Pick a random room, add it to connectedTo
                int rand = Random.Range(0, 30);
                // Keep generating until find a room that has not been already connected to by this node
                while (roomAlreadyConnected(rand, rooms[i].connectedTo) || rand == i){
                    rand = Random.Range(0, 30);
                }

                changeTeleporters(i, j, rand);
                
                rooms[i].doors[j].next = rooms[rand];
                rooms[i].connectedTo[j] = rand;
            }
        }
    }

    void changeTeleporters(int i, int j, int toCheck){
        if (toCheck == wumpusRoom){
            // Change the teleporter to a boss teleporter
            GameObject obj = rooms[i].doors[j].gameObject;
            Room tempRoom = rooms[i].doors[j].next;

            // Destroy old teleporter
            Destroy(obj.GetComponent<Teleporter>());

            // Create new teleporter and assign attribute
            rooms[i].doors[j] = obj.AddComponent<BossTeleporter>();
            rooms[i].doors[j].next = tempRoom;
            rooms[i].doors[j].Awake();
        } else if (toCheck == batRoom){
            // Change the teleporter to a boss teleporter
            GameObject obj = rooms[i].doors[j].gameObject;
            Room tempRoom = rooms[i].doors[j].next;

            // Destroy old teleporter
            Destroy(obj.GetComponent<Teleporter>());

            // Create new teleporter and assign attribute
            // Component Type changes based on teleporter type
            rooms[i].doors[j] = obj.AddComponent<BatTeleporter>();
            rooms[i].doors[j].next = tempRoom;
            rooms[i].doors[j].Awake();
        } else if (toCheck == pitRoom) {
            // Change the teleporter to a boss teleporter
            GameObject obj = rooms[i].doors[j].gameObject;
            Room tempRoom = rooms[i].doors[j].next;

            // Destroy old teleporter
            Destroy(obj.GetComponent<Teleporter>());

            // Create new teleporter and assign attribute
            // Component Type changes based on teleporter type
            rooms[i].doors[j] = obj.AddComponent<PitTeleporter>();
            rooms[i].doors[j].next = tempRoom;
            rooms[i].doors[j].Awake();
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