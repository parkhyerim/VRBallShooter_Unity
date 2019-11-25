using System.Collections.Generic;
using UnityEngine;

public class BallCalculator : MonoBehaviour {
    private LogController logController;
    public TextMesh shotBallsText;
    public TextMesh missedBallText;

    [Header("Pause and Resume Counter")]
    [SerializeField]
    private int pauseCounter;
    [SerializeField]
    private int resumeCounter;
    private bool isInterrupting, gameFinished;

    [Header("Counter List After Pause")]
    public List<int> thrBallCountListAP;  // thrown Ball Count List After Pause
    public List<int> mBallCountListAP;    // Missed Ball Count List After Pause
    public List<int> sBallCountListAP;    // Shot Ball Count List After Pause

    [Header("Ball Counter")]
    [SerializeField]
    private int thrBallCounter;
    [SerializeField]
    private int shotBallCounter;   
    [SerializeField]
    private int missedBallCounter;
    [SerializeField]
    private int thrBallCounterBP, shotBallCounterBP, missedBallCounterBP;    // Before Pause
    [SerializeField]
    private int thrBallCounterIntr, mBallCounterIntr, sBallCounterIntr;   // During Interruption

    private double shotBallPercent;
    private double shotBallPercentBP;
    private double shotBallPercentAFP;  // After first pause

    /*
     *  public setter and getter methods to access and update the value of a private variable.
     */
    public int ThrBallCounter { get => thrBallCounter; set => thrBallCounter = value; }
    public int ThrBallCounterBP { get => thrBallCounterBP; set => thrBallCounterBP = value; }

    public int ShotBallCounter { get => shotBallCounter; set => shotBallCounter = value; }
    public int ShotBallCounterBP { get => shotBallCounterBP; set => shotBallCounterBP = value; }

    public int MissedBallCounter { get => missedBallCounter; set => missedBallCounter = value; }
    public int MissedBallCounterBP { get => missedBallCounterBP; set => missedBallCounterBP = value; }

    public int ThrBallCounterIntr { get => thrBallCounterIntr; set => thrBallCounterIntr = value; }
    public int MBallCounterIntr { get => mBallCounterIntr; set => mBallCounterIntr = value; }
    public int SBallCounterIntr { get => sBallCounterIntr; set => sBallCounterIntr = value; }

    public bool GameFinished { get => gameFinished; set => gameFinished = value; }
    public bool IsInterrupting { get => isInterrupting; set => isInterrupting = value; }


    // Start is called before the first frame update
    void Start() {
        missedBallText.text = "Missed:\n00";
        shotBallsText.text = "Score:\n00";

        logController = FindObjectOfType<LogController>();
    }

    void Update() {
        missedBallText.text = "Missed:\n" + ((GetMissedBallCounter() < 10) ? "0" + GetMissedBallCounter().ToString() : GetMissedBallCounter().ToString());
    }

    public void SetPauseCounter() {
        pauseCounter = pauseCounter + 1;
    }

    public int GetPauseCounter() {
        return pauseCounter;
    }

    public void SetResumeCounter() {
        resumeCounter = resumeCounter + 1;

        thrBallCountListAP.Add(0);    
        mBallCountListAP.Add(0);
        sBallCountListAP.Add(0);
    }

    public int GetResumeCounter() {
        return resumeCounter;
    }

    public void ThrownBallsCount() {
        // total thrown balls during the game
        ThrBallCounter = ThrBallCounter + 1;

        if (thrBallCountListAP.Count == 0) {            // still no pause and resume
            ThrBallCounterBP = ThrBallCounterBP + 1;
        }
        else if (thrBallCountListAP.Count > 0) {         // the player has paused and resumed at least once 
            int listLength = thrBallCountListAP.Count;
            int ballCounter = thrBallCountListAP[listLength - 1];
            thrBallCountListAP[listLength - 1] = ballCounter + 1;
        }

        if (IsInterrupting) {
            thrBallCounterIntr = thrBallCounterIntr + 1;
        }
        // MissedBallsCount();
    }

    public void ShotBallsCount() {
        // total shot balls during the game
        ShotBallCounter = ShotBallCounter + 1;
        shotBallsText.text = "Score:\n" + ((shotBallCounter < 10) ? "0" + ShotBallCounter.ToString() : ShotBallCounter.ToString());

        if (sBallCountListAP.Count == 0) {               // still no pause and resume
            shotBallCounterBP = shotBallCounterBP + 1;
        }
        else if (sBallCountListAP.Count > 0) {         // the player has paused and resumed at least once 
            int listLength = sBallCountListAP.Count;
            int ballCounter = sBallCountListAP[listLength - 1];
            sBallCountListAP[listLength - 1] = ballCounter + 1;
        }

        if (IsInterrupting) {
            sBallCounterIntr = sBallCounterIntr + 1;
        }
    }

    public void MissedBallsCount() {
        MissedBallCounter = MissedBallCounter + 1;

        if (mBallCountListAP.Count == 0) {
            MissedBallCounterBP = MissedBallCounterBP + 1;
        }
        else if (mBallCountListAP.Count > 0) {
            int listLength = mBallCountListAP.Count;
            int ballCounter = mBallCountListAP[listLength - 1];
            mBallCountListAP[listLength - 1] = ballCounter + 1;
        }

        if (IsInterrupting) {
            //  missedBallsDuringInterruption = ThrownBallsDuringInterruption - shotBallsDuringInterruption;
            mBallCounterIntr = mBallCounterIntr + 1;
        }
    }

    public int GetMissedBallCounter() {
        return MissedBallCounter;
    }

    public double GetPercentageOfShotBall() {
        if (ThrBallCounter > 0) {
            shotBallPercent = ShotBallCounter / ThrBallCounter;
        }
        return shotBallPercent;
    }

    public double GetPercentageOfShotBallBP() {
        if (thrBallCounterBP > 0) {
            shotBallPercentBP = ShotBallCounterBP / ThrBallCounterBP;
        }
        return shotBallPercentBP;
    }

    public double GetPercentageOfShotBallAFP() {
        if(!(thrBallCountListAP.Count == 0)) {
            int thrBalls = thrBallCountListAP[0];
            int shotBalls = sBallCountListAP[0];
            shotBallPercentAFP = shotBalls / thrBalls;
        }
        return shotBallPercentAFP;
    }
}
