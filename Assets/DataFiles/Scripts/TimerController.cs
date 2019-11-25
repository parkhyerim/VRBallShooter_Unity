using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour {
    public TextMesh timerText;
//    public TextMesh timerTextForTester;

    private GameController gameController;
    private NonHMDUserController avatarController;
    private HMDUserController player;
    private BallCalculator ballCalculator;
    private BallController ballController;
    private LogController logController;
    private RightHandColor rightHand;
    private LeftHandColor leftHand;

    private bool gameReady;           // When all the pre-balls have been shot, the gameReady is true
    private bool gameFinished;
    private bool resumeGameReady;
    private bool closeDoorChecked;

    [Header("<Setting> Time Value")]
    public float totalPlayTime;       // total playing time (defalut = 120 seconds)
    public float timerTimer = -1.5f;

    [Header("Game Time Info.")]
    [SerializeField]
    private float remainingTime;    // the remainingTime until the end of the game
    private float pPurePlayTime;

    [Header("Avatar(Bystander) Time Info.")]
    [SerializeField]
    private float avrEnterTime;          // The time the avater enters the room
    [SerializeField]
    private float avrSignalTime;
    [SerializeField]
    private float avrAskingTime, avrEndAskingTime;
    [SerializeField]
    private float avrLeavingTime;       // The time the avatar started to leave the room by shooting(the player) the resume button.
    [SerializeField]
    private float avrLeavingTimeALAP;   // ALAP(As late as possible) : If the player forgets or misses to click the resume button.
    [SerializeField]
    private float avrCloseDoorTime;

    [Header("Player Time Info.")]
    [SerializeField]
    private float pFirstStartBreakTime; // The time the player clicks the pause button (starts to take a break)
    [SerializeField]
    private float pFirstEndBreakTime;   // The time the player clicks the resume button (starts to continue the game)
    [SerializeField]
    private float pFirstBreakDuration;
    [SerializeField]
    private float pRtFromAvrEnter, pRtFromAvrSignal, pRtFromAvrAsking, pRtFromStartOfGame;
    [SerializeField]
    public List<float> pStartBreakTimeList = new List<float>();
    public List<float> pEndBreakTimeList = new List<float>();
    public List<float> pBreakDurationList = new List<float>();

    [Header("Pause Information")]
    public bool pauseButtonPressed;
    // public bool resumeButtonPressed;
    private bool firstPauseDone;
    private bool firstResumeDone;
    private bool firstDurationCalcDone;

    private float firstStartBreakTimeCD;
    private float measureTimeWithIntrUnitCD;
    private float measureTimeWithBreakStartUnitCD;
    private float measureTimeWithSignalUnitCD;

    /*
     *  public setter and getter methods to access and update the value of a private variable.
     */
    public float RemainingTime { get => remainingTime; set => remainingTime = value; }

    public float AvrEnterTime { get => avrEnterTime; set => avrEnterTime = value; }
    public float AvrSignalTime { get => avrSignalTime; set => avrSignalTime = value; }
    public float AvrAskingTime { get => avrAskingTime; set => avrAskingTime = value; }
    public float AvrEndAskingTime { get => avrEndAskingTime; set => avrEndAskingTime = value; }
    public float AvrLeavingTime { get => avrLeavingTime; set => avrLeavingTime = value; }
    public float AvrLeavingTimeALAP { get => avrLeavingTimeALAP; set => avrLeavingTimeALAP = value; }
    public float AvrCloseDoorTime { get => avrCloseDoorTime; set => avrCloseDoorTime = value; }

    public float PlrRtFromAvrEnter { get => pRtFromAvrEnter; set => pRtFromAvrEnter = value; }
    public float PlrRtFromAvrSignal { get => pRtFromAvrSignal; set => pRtFromAvrSignal = value; }
    public float PlrRtFromStartOfGame { get => pRtFromStartOfGame; set => pRtFromStartOfGame = value; }
    public float PlrPurePlayTime { get => pPurePlayTime; set => pPurePlayTime = value; }
    public float PlrRtFromAvrAsking { get => pRtFromAvrAsking; set => pRtFromAvrAsking = value; }
    public float PlrFirstBreakDuration { get => pFirstBreakDuration; set => pFirstBreakDuration = value; }

    public bool GameReady { get => gameReady; set => gameReady = value; }
    public bool ResumeGameReady { get => resumeGameReady; set => resumeGameReady = value; }

    void Start() {
        gameController = FindObjectOfType<GameController>();
        avatarController = FindObjectOfType<NonHMDUserController>();
        player = FindObjectOfType<HMDUserController>();
        logController = FindObjectOfType<LogController>();
        ballCalculator = FindObjectOfType<BallCalculator>();
        ballController = FindObjectOfType<BallController>();
        rightHand = FindObjectOfType<RightHandColor>();
        leftHand = FindObjectOfType<LeftHandColor>();

        // Total Playing Time Setting:  If the game playing time is not assigned, the default setting is 120 seconds.
        if (totalPlayTime == 0) {
            totalPlayTime = 120f;
        }
        // Timer Setting
        RemainingTime = totalPlayTime;
        timerText.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
       // timerTextForTester.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
    }

    void Update() {
        // THE START OF GAME : When all the pre-balls are shot, gameReady is true and the game is not finished
        if (gameController.GameReady && !gameFinished) {
            timerTimer += Time.deltaTime;
            if (timerTimer > 0f) {
                RemainingTime = RemainingTime - Time.deltaTime;
                timerText.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
               // timerTextForTester.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
                gameController.SetGameStart();
            }
        }

        // THE END OF GAME
        if (RemainingTime < 1f) {
            gameController.GameReady = false;
            //   GameReady = false;
            if (!gameFinished) {
                gameController.SetGameFinished();
                gameFinished = true;
            }

            // ballCalculator.GameFinished = true;
            // SceneManager.LoadScene("EndScene");   // When the total playing time is over, it moves to the end scene.
            //ballController.canSpawnBalls = false;
            timerText.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
           // timerTextForTester.text = "Time:\n" + Mathf.Floor(RemainingTime).ToString();
        }

        // THE END OF INTERRUPTION : The bystander leaves the room and closes the door
        if (RemainingTime <= avatarController.CloseDoorTimeCD && RemainingTime >= 1f) {
            if (!closeDoorChecked) {
                ballCalculator.IsInterrupting = false;
                logController.CreateInterruptionLog(avatarController.CloseDoorTimeCD, "End: Close Door");
                closeDoorChecked = true;
            }
        }
    }

    public void SetStarBreakTime() {
        gameController.PauseButtonPressed = true;
        //  pauseButtonPressed = true;

        pStartBreakTimeList.Add(totalPlayTime - RemainingTime);
        player.SetStartBreakTimeList(totalPlayTime - RemainingTime);

        if (!firstPauseDone) {
            pFirstStartBreakTime = totalPlayTime - RemainingTime;
            firstStartBreakTimeCD = RemainingTime;

            PlrRtFromAvrEnter = pFirstStartBreakTime - AvrEnterTime;
            PlrRtFromAvrSignal = pFirstStartBreakTime - AvrSignalTime;
            PlrRtFromAvrAsking = pFirstStartBreakTime - AvrAskingTime;
            PlrRtFromStartOfGame = pFirstStartBreakTime;

            // Send time information to the player class
            player.FirstStartBreakTime = pFirstStartBreakTime;
            player.RtFromAvrEnter = PlrRtFromAvrEnter;
            player.RtFromAvrSignal = PlrRtFromAvrSignal;
            player.RtFromAvrAsking = PlrRtFromAvrAsking;
            player.RtFromStartOfGame = PlrRtFromStartOfGame;

            firstPauseDone = true;
        }
    }

    public float GetStartBreakTime() {
        return pFirstStartBreakTime;
    }

    public void SetEndBreakTime() {
        gameController.PauseButtonPressed = false;
        //  pauseButtonPressed = false;
        //resumeButtonPressed = true; // ????????????
        pEndBreakTimeList.Add(totalPlayTime - RemainingTime);
        player.SetEndBreakTimeList(totalPlayTime - RemainingTime);

        if (!firstResumeDone) {
            pFirstEndBreakTime = totalPlayTime - RemainingTime;
            float firstEndBreakTimeCD = RemainingTime;
            measureTimeWithIntrUnitCD = firstEndBreakTimeCD - AvrEnterTime;

            measureTimeWithBreakStartUnitCD = firstEndBreakTimeCD - pFirstEndBreakTime;
            measureTimeWithSignalUnitCD = firstEndBreakTimeCD - AvrSignalTime;

            /*
            Debug.Log("first end break time CD: " + firstEndBreakTimeCD + "Ava Enter time: " + AvrEnterTime + "Measure time unit with interruption Unit: " + measureTimeWithIntrUnitCD);
            Debug.Log("first end break time CD: " + firstEndBreakTimeCD + "pFirst start BreakTime: " + pFirstStartBreakTime + "MEAsure time unit with start break Unit: " + measureTimeWithBreakStartUnitCD);
            Debug.Log("first end break time CD: " + firstEndBreakTimeCD + "AvrSignalTime: " + AvrSignalTime + "MEAsure time unit with signal unit: " + measureTimeWithSignalUnitCD);
            */
            // SetEndInterruptionTime();

            // Send time information to the player class
            player.FirstEndBreakTime = pFirstEndBreakTime;
            firstResumeDone = true;
        }
        CalculateBreakDuration();
    }

    public float GetEndBreakTime() {
        return pFirstEndBreakTime;
    }

    public void CalculateBreakDuration() {
        if (!firstDurationCalcDone) {
            PlrFirstBreakDuration = pFirstEndBreakTime - pFirstStartBreakTime;
            player.FirstBreakDuration = PlrFirstBreakDuration;
            pBreakDurationList.Add(PlrFirstBreakDuration);
            player.SetBreakDurationList(PlrFirstBreakDuration);

            firstDurationCalcDone = true;
            //Debug.Log("Duration num:" + pBreakDurationList.Count);
        }
        else {
            if (pStartBreakTimeList.Count > 1 && pEndBreakTimeList.Count > 1) {
                int numOfstartListIndex = pStartBreakTimeList.Count;
                int numOfendListIndex = pEndBreakTimeList.Count;
                if (numOfstartListIndex == numOfendListIndex) {
                    float startTime = pStartBreakTimeList[numOfstartListIndex - 1];
                    float endTime = pEndBreakTimeList[numOfendListIndex - 1];
                    float duration = endTime - startTime;
                    pBreakDurationList.Add(duration);
                    player.SetBreakDurationList(duration);
                    // Debug.Log("Duration num:" + pBreakDurationList.Count);
                }
            }
        }
    }

    public void CalculatePurePlayTime() {
        float sumOfPlayTime = 0f;
        if (pBreakDurationList.Count > 0) {
            // Debug.Log("The Sum of playtime: " + sumOfPlayTime);
            for (int i = 0; i < pBreakDurationList.Count; i++) {
                sumOfPlayTime = sumOfPlayTime + pBreakDurationList[i];
                // Debug.Log("The Sum of playtime: " + i + "th index value" + pBreakDurationList[i] +" added" + sumOfPlayTime);
            }
        }
        PlrPurePlayTime = totalPlayTime - sumOfPlayTime;
    }
}