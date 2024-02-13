public class GameManager
{
    public List<Room> cave;

    public int arrows = 0;
    public int gold = 0;
    public int turns = 0;

    public int wumpus = 0;

    public void moreTurn(int count){
        turns += count;
    }

    public void moreGold(int count){
        gold += count
    }

    public void moreArrows(int count){
        arrows += count;
    }

    public void win(){
        wumpus += 50;
        return calculateScore();
    }

    public int calculateScore(){
        return (100 - turns) + gold + (5 * arrows) + 50;
    }
}

public class Room
{
    List<Room> nextRooms;
    public bool visited = false;
}

public class GamePlay
{
    public GameManager gm;
    public Room current = new Room();
    public void move(){
        if (!current.visited){
            gm.moreGold(1);
            current.visited = true;
        }
        gm.moreTurn(1);
    }

    public void defeatWumpus(){
        int stuff = gm.win();
    }

}