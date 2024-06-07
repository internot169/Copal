using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    // reference to gm and player, assigned on start. 
    GameManager gameManager;

    GameObject Player;

    // prices, just default values
    // can and likely are reassigned in editor. 
    [Header("prices")]
    public int ArrowCost = 3;
    public int SecretCost = 3;

    public int AugmentCost = 20;

    // assign the pointers upon start
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player");
    }

    // this method is called after ending the trivia. It will update arrows
    // or not and log the results.
    public void receiveArrow(bool correct){
        // if correct
        if (correct) {
            // log the arrows in gm and print out to the logger
            gameManager.arrows += 2;
            gameManager.logger.log("Bought 2 arrows");
        } else {
            // otherwise, just do a printout and don't update values. 
            gameManager.logger.log("Failed trivia, didn't buy arrows");
        }
        // close the shop. 
        gameManager.CloseShop();
    }

    // this method enters the trivia game to attempt to buy an arrow
    public void BuyArrow(){
        // buying takes a turn. 
        gameManager.turns++;
        // if can buy, then start the trivia game and create a callback. 
        if(gameManager.coins > ArrowCost){
            Callback receiver;
            receiver = receiveArrow;
            StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver, false));
        } else {
            // otherwise, log that there's not enough money. 
            gameManager.logger.log("Not enough coins to play trivia for Arrows.");
        }
        
    }

    // this is a callback that actually give a secret. 
    public void receiveSecret(bool correct){
        // depending on result, give a secret or log a failure. 
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
            // give the secret in the logs. 
            gameManager.logger.log(secret);
        } else {
            // log failure. 
            gameManager.logger.log("Failed trivia, didn't buy secret");
        }
    }

    // method for buttons to call when buying a secret. 
    public void BuySecret(){
        // buying costs a turn. 
        gameManager.turns++;
        // create callback if you can buy, and then start trivia game to buy. 
        if(gameManager.coins > SecretCost){
            Callback receiver;
            receiver = receiveSecret;
            StartCoroutine(GameObject.Find("Trivia").GetComponent<Trivia>().LoadTrivia(3, 2, receiver, false));
        } else {
            // otherwise, log a failure to buy. 
            gameManager.logger.log("Not enough coins to play trivia for secret.");
        }
        // close the shop. 
        gameManager.CloseShop();
    }
    
    // method to buy augments. 
    public void BuyAugment(){
        // buying costs a turn. 
        gameManager.turns++;
        // it just gives you a random augment and replaces whatever was there before. 
        if (gameManager.coins > AugmentCost){
            gameManager.spend(AugmentCost);
            AugmentManager augments = Player.GetComponent<AugmentManager>();
            int pick = Random.Range(0, 11);
            // I have no idea how better to do this, apart from switch statement I guess. 
            // however this does get simplified to switch in compiler so it doesn't matter at all. 
            // from the random number picked, resets that class of augment and gives you the new one. 
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
            // log failure to buy. 
            gameManager.logger.log("Not enough coins to buy Augment.");
        }
        // close the shop
        gameManager.CloseShop();
    }
}
