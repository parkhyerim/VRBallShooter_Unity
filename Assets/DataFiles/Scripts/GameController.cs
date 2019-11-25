using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour {
    private TimerController timerController;
    private PauseController pauseController;

    private BallCalculator ballCalculator;
    private BallController ballController;
    private LogController logController;

    private HMDUserController player;
    private NonHMDUserController avatar;
   
    private RightHandColor rightHand;
    private LeftHandColor leftHand;
   
    public TextMesh endText;

    private bool gameReady;
    private bool gameFinished;
    private bool setGameFinishedCalled;
    private bool pauseButtonPressed;

    private float printTimer = -2.5f;
    private bool printOnceCalled;
    private bool interruptionStarted;
    private float time;
    private int pauseNumber;
    private int resumeNumer;

    public string groupName;
    public int studyNumber;

    public bool GameReady { get => gameReady; set => gameReady = value; }
    public bool GameFinished { get => gameFinished; set => gameFinished = value; }
    public bool PauseButtonPressed { get => pauseButtonPressed; set => pauseButtonPressed = value; }

    void Start() {
        timerController = FindObjectOfType<TimerController>();
        pauseController = FindObjectOfType<PauseController>();
        logController = FindObjectOfType<LogController>();
        ballCalculator = FindObjectOfType<BallCalculator>();
        ballController = FindObjectOfType<BallController>();
        avatar = FindObjectOfType<NonHMDUserController>();
        player = FindObjectOfType<HMDUserController>();
        rightHand = FindObjectOfType<RightHandColor>();
        leftHand = FindObjectOfType<LeftHandColor>();

        SetUpLog();
    }

    private void Update() {

        if (setGameFinishedCalled) {
            printTimer += Time.deltaTime;
            if(printTimer > 0) {
                if (!printOnceCalled) {
                    logController.CreateResultList();
                    logController.CreateBallInfoSetLog();
                    printOnceCalled = true;
                }    
            }
        }
    }

    /*
     *  Setup Logfiles
     */
    public void SetUpLog() {
        logController.SetUpLog();
    }

    /*
     *  Pause Management
     */
    public void StartPause() {
        timerController.SetStarBreakTime();
        ballCalculator.SetPauseCounter();
        logController.CreatePauseLog(timerController.RemainingTime, "Pause Started");
    }

    public void EndPause() {
        player.SetEndPause();
        timerController.SetEndBreakTime();
        ballController.CanSpawnBalls = true;
        ballCalculator.SetResumeCounter();
        logController.CreatePauseLog(timerController.RemainingTime, "Pause Ended");
    }

    /*
     *  Interruptions Management
     */
    public void StartInterruption() {
        ballCalculator.IsInterrupting = true;
    }

    public void EndInterruption() {
        avatar.SetEndInterruptionTime();
    }

    /*
     *  Game Start-End
     */
    public void SetGameStart() {
        rightHand.attachedToHand = true;
        leftHand.attachedToHand = true;
    }

    public void SetGameFinished() {
        endText.text = "GAME OVER";
        setGameFinishedCalled = true;
        ballCalculator.GameFinished = true;
        ballController.CanSpawnBalls = false;
        rightHand.attachedToHand = false;
        rightHand.gameFinished = true;
        leftHand.attachedToHand = false;
        leftHand.gameFinished = true;

        ballCalculator.GetPercentageOfShotBall();
    }
}