using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePauseController : MonoBehaviour {
    public TextMesh displayText;
    public TextMesh instructionsText;

    //private bool pausePossibleMode;
    private bool wasPausedOnce;
    private bool wasResumedOnce;

    private HMDUserController player;

    private BallCalculator ballCalc;


    // Start is called before the first frame update
    void Start() {
      
        player = FindObjectOfType<HMDUserController>();
        
       

        displayText.text = "MENU";
    }

    public void SetPauseMode() {
        //  ballCalc.PauseCounter();
        
        displayText.text = "PAUSED";
    }

    public void SetResumeGameMode() {
        
        //  ballCalc.ResumeCounter();
        displayText.text = "GAME AGAIN";
    }




    public void SetPausePossibleMode() {
        //  pausePossibleMode = true;

        if (!wasPausedOnce) {
            // displayText.text = "PAUSED";
        }
    }

    /*
    public bool GetPausePossibleMode() {
       return pausePossibleMode;
    }

    */


}
