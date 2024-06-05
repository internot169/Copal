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
    public int TriviaCost = 5;

    public int AugmentCost = 10;
    
    public int LifeCost = 15;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyArrow(){
        gameManager.spend(ArrowCost);
        gameManager.arrows += 1;
        // pretty much setup for an exit button if you wanted to do it. 
        gameManager.CloseShop();
    }

    public void BuyLife(){
        gameManager.spend(LifeCost);
        gameManager.lives += 1;
    }

    public void BuyTrivia(){
        // Secret
    }

    public void BuyAugment(){
        // this is the worst system known to mankind. 
        // it just gives you a random augment and replaces whatever was there before. 
        gameManager.spend(AugmentCost);
        AugmentManager augments = Player.GetComponent<AugmentManager>();
        int pick = Random.Range(0, 11);
        // I have no idea how better to do this, apart from switch statement I guess. 
        // however this does get simplified to switch in compiler so it doesn't matter at all. 
        if(pick == 0){
            augments.ResetMainAll();
            augments.ChangeDOTMain(true);
        }else if(pick == 1){
            augments.ResetMainAll();
            augments.ChangeSlowMain(true);
        }else if(pick == 2){
            augments.ResetMainAll();
            augments.ChangeVampMain(true);
        }else if(pick == 3){
            augments.ResetMainAll();
            augments.ChangeDroneMain(true);
        }else if(pick == 4){
            augments.ResetAltAll();
            augments.ChangeDOTAlt(true);
        }else if(pick == 5){
            augments.ResetAltAll();
            augments.ChangeSlowAlt(true);
        }else if(pick == 6){
            augments.ResetAltAll();
            augments.ChangeVampAlt(true);
        }else if(pick == 7){
            augments.ResetAltAll();
            augments.ChangeDroneAlt(true);
        }else if(pick == 8){
            augments.ResetFieldAll();
            augments.ChangeDOTField(true);
        }else if(pick == 9){
            augments.ResetFieldAll();
            augments.ChangeSlowField(true);
        }else if(pick == 10){
            augments.ResetFieldAll();
            augments.ChangeVampField(true);
        }else if(pick == 11){
            augments.ResetFieldAll();
            augments.ChangeDroneField(true);
        }
        
        gameManager.CloseShop();
    }
}
