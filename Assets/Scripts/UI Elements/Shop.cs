using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update

    GameManager gameManager;

    GameObject Player;

    [Header("prices")]
    public int ArrowCost = 3;
    public int SecretCost = 3;

    public int AugmentCost = 20;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveArrow(bool correct){
        if (correct) {
            gameManager.arrows += 2;
            gameManager.logger.log("Bought 2 arrows");
        } else {
            gameManager.logger.log("Failed trivia, didn't buy arrows");
        }
        // pretty much setup for an exit button if you wanted to do it. 
        gameManager.CloseShop();
    }

    public void BuyArrow(){
        gameManager.turns++;
        if(gameManager.coins > ArrowCost){
            Callback receiver;
            receiver = receiveArrow;
            StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
        } else {
            gameManager.logger.log("Not enough coins to play trivia for Arrows.");
        }
        
    }

    public void receiveSecret(bool correct){
        if (correct){
            int pick = Random.Range(0, 10);
            string secret = "";
            if (pick == 0){
                secret = "Bat Room Number: " + gameManager.gameObject.GetComponent<RoomGenerator>().batRoom.ToString();
            } else if (pick == 1) {
                secret = "Pit Room Number: " + gameManager.gameObject.GetComponent<RoomGenerator>().pitRoom.ToString();
            } else if (pick == 2){
                secret = "Wumpus Room Number: " + gameManager.gameObject.GetComponent<RoomGenerator>().wumpusRoom.ToString();
            } else if (pick == 3) {
                secret = "Current Room Number: " + gameManager.currentRoom().roomNum.ToString();
            } else if (pick == 4) {
                secret = "Trying for a secret? ChatGPT will destroy you.";
            } else if (pick == 5) {
                secret = "You're a fool for buying this. Buy NVIDIA stock instead, I heard AI was really cool.";
            } else if (pick == 6) {
                secret = "Put down the videogames and go outside. Or, visit https://groq.com/. You'll thank me later.";
            } else if (pick == 7) {
                secret = "Anguilla's government is profiting from the AI boom! See, I'm good for the world.";
            } else if (pick == 8) {
                secret = "Making OpenAI API calls is for sure an AI app.";
            } else if (pick == 9){
                secret = "Sorry Microsoft, but I don't like Copilot";
            } else if (pick == 10){
                secret = "What's your AI strategy? I'm sure it's worse than mine.";
            }
            gameManager.logger.log(secret);
        } else {
            gameManager.logger.log("Failed trivia, didn't buy secret");
        }
    }

    public void BuySecret(){
        gameManager.turns++;
        if(gameManager.coins > SecretCost){
            Callback receiver;
            receiver = receiveSecret;
            StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver));
        } else {
            gameManager.logger.log("Not enough coins to play trivia for secret.");
        }
        // pretty much setup for an exit button if you wanted to do it. 
        gameManager.CloseShop();
    }
    

    public void BuyAugment(){
        gameManager.turns++;
        // it just gives you a random augment and replaces whatever was there before. 
        if (gameManager.coins > AugmentCost){
            gameManager.spend(AugmentCost);
            AugmentManager augments = Player.GetComponent<AugmentManager>();
            int pick = Random.Range(0, 11);
            // I have no idea how better to do this, apart from switch statement I guess. 
            // however this does get simplified to switch in compiler so it doesn't matter at all. 
            if(pick == 0){
                augments.ResetMainAll();
                augments.ChangeDOTMain(true);
                gameManager.logger.log("Bought DOT Main");
            }else if(pick == 1){
                augments.ResetMainAll();
                augments.ChangeSlowMain(true);
                gameManager.logger.log("Bought Slow Main");
            }else if(pick == 2){
                augments.ResetMainAll();
                augments.ChangeVampMain(true);
                gameManager.logger.log("Bought Vamp Main");
            }else if(pick == 3){
                augments.ResetMainAll();
                augments.ChangeDroneMain(true);
                gameManager.logger.log("Bought Drone Main");
            }else if(pick == 4){
                augments.ResetAltAll();
                augments.ChangeDOTAlt(true);
                gameManager.logger.log("Bought DOT Alt");
            }else if(pick == 5){
                augments.ResetAltAll();
                augments.ChangeSlowAlt(true);
                gameManager.logger.log("Bought Slow Alt");
            }else if(pick == 6){
                augments.ResetAltAll();
                augments.ChangeVampAlt(true);
                gameManager.logger.log("Bought Vamp Alt");
            }else if(pick == 7){
                augments.ResetAltAll();
                augments.ChangeDroneAlt(true);
                gameManager.logger.log("Bought Drone Alt");
            }else if(pick == 8){
                augments.ResetFieldAll();
                augments.ChangeDOTField(true);
                gameManager.logger.log("Bought DOT Field");
            }else if(pick == 9){
                augments.ResetFieldAll();
                augments.ChangeSlowField(true);
                gameManager.logger.log("Bought Slow Field");
            }else if(pick == 10){
                augments.ResetFieldAll();
                augments.ChangeVampField(true);
                gameManager.logger.log("Bought Vamp Field");
            }else if(pick == 11){
                augments.ResetFieldAll();
                augments.ChangeDroneField(true);
                gameManager.logger.log("Bought Drone Field");
            }
        }  else {
            gameManager.logger.log("Not enough coins to buy Augment.");
        }
        gameManager.CloseShop();
    }
}
