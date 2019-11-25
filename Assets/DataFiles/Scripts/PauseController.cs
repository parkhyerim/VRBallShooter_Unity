using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {
    public TextMesh displayText;
    public TextMesh instructionsText;

    //private bool pausePossibleMode;
    private bool wasPausedOnce;
    private bool wasResumedOnce;

    private GameController gameController;
    private TimerController timeController;
    private HMDUserController player;
    private NonHMDUserController avatar;
    private BallCalculator ballCalc;
  

    // Start is called before the first frame update
    void Start() {
        gameController = FindObjectOfType<GameController>();
        timeController = FindObjectOfType<TimerController>();
        player = FindObjectOfType<HMDUserController>();
        avatar = FindObjectOfType<NonHMDUserController>();
        ballCalc = FindObjectOfType<BallCalculator>();

        displayText.text = "MENU";
    }

    public void SetPauseMode() {
      //  ballCalc.PauseCounter();
        gameController.StartPause();
        displayText.text = "PAUSED";
    }

    public void SetResumeGameMode() {
        gameController.EndPause();
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
