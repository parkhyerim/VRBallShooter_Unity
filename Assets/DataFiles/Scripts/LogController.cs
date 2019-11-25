using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogController : MonoBehaviour {
    private TimerController timerController;
    private BallCalculator ballCalculator;
    private GameController gameController;

    private string groupName;
    private int studyNumber;

    private int pauseNumber, resumeNumer;

    private bool gameFinished;
    private string logText;
    private int ballCounter;
    private Dictionary<float, bool> balls = new Dictionary<float, bool>();

    /*
     * Texts(Names) in Log files
     */
    private string logTextStr;
    private string intrInfoStr, intrStartTimeStr, intrEndTimeStr, intrSignalTimeStr, intrAskingTimeStr, intrEndAskingTimeStr, intrLeavingTimeStr, intrDurationStr;
    private string playerPauseInfoStr, pauseNumberStr, pauseStartStr, pauseEndStr, pauseDurationStr, pauseDurationPercentageStr;
    private string responseTimeInfoStr, responseTimeStr, responseTimeFromIntStartStr, responseTimeFromSignalStr, responseTimeFromTalkingStr, responseTimeFromEndAskingStr;
    private string ballInfoStr, playTimeStr, thrownBallsStr, shotBallsStr, missedBallsStr;
    private string ballInfoTotalGameStr;
    private string ballInfoBeforeBreakStr, thrownBallsBeforeStr, shotBallsBeforeStr, missedBallsBeforeStr;
    private string ballInfoAfterPauseStr, thrownBallsAfterPauseStr, shotBallsAfterPauseStr, missedBallsAfterPauseStr;
    private string ballInfoDuringIntStr, thrownBallsDuringIntStr, shotBallsDuringIntStr, missedBallsDuringIntStr;

    public bool GameFinished { get => gameFinished; set => gameFinished = value; }

    // Start is called before the first frame update
    void Start() {
        timerController = FindObjectOfType<TimerController>();
        ballCalculator = FindObjectOfType<BallCalculator>();
        gameController = FindObjectOfType<GameController>();

        // the name of groups and participants in user study
        groupName = gameController.groupName;
        studyNumber = gameController.studyNumber;
    }

    public void SetUpLog() {
        SetTitlesForInterruptionLog();
        SetTitlesForBreakTimeLog();
        SetTitlesForResponseTimeLog();
        SetTitlesForShotBallLog();
    }

    public void AddBallInfoToList(float time, bool shootingResult) {
        if (!balls.ContainsKey(time)) {
            balls.Add(time, shootingResult);
        }
    }


    /*
     * BALL INFO SET LOG 
     */
    public void CreateBallInfoSetLog() {
        if (balls.Count == 0) {
            logText = "Empty";
        }
        else {
            foreach (KeyValuePair<float, bool> ball in balls) {
                logText = logText + "\n" + ball;
            }
        }
        string dateTime = System.DateTime.Now.ToString();
        string fileName = "(Ball Info Set Log) " + groupName + "_" + studyNumber + ".txt";
        string path = "Assets/LogFiles/" + fileName;

        // Create a File if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "\n\n****************BALL INFO SET LOG******************");
        }

        // Content of the log-files
        string content = "\n\nStudy Date and Time: " + dateTime + "\n"
            + "Group: " + groupName + "\n"
            + "Number: " + studyNumber + "\n"
            + logText;

        File.AppendAllText(path, content);
    }



    /*
     * CHRONOLOGICAL EVENT LOG
     */

    public void CreateInterruptionLog(float time, string msg) {
        string DateTime = System.DateTime.Now.ToString();
        string logMsg = msg;
        float eventTimeCD = time;
        float eventTime = timerController.totalPlayTime - time;

        string fileName = "(Chronological Event Log) " + groupName + "_" + studyNumber + ".txt";
        string path = "Assets/LogFiles/" + fileName;

        // Create File if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "\n\n****************CHRONOLOGICAL EVENT LOG******************\n\nDate and Time: " + DateTime + "\n" + "Group: " + groupName + "\n"
            + "Number: " + studyNumber + "\n");
        }

        // Content of the files
        string content = "\nTime: " + eventTime
            + ",  Time(CD): " + eventTimeCD 
            + ",  Interruption Event: " 
            + logMsg + "\n";        
        // Add some to text to it
        File.AppendAllText(path, content);
    }

    public void CreatePauseLog(float time, string msg) {
        string DateTime = System.DateTime.Now.ToString();
        string logmsg = msg;
        float eventTimeCD = time;
        float eventTime = timerController.totalPlayTime - time;

        string fileName = "(Chronological Event Log) " + groupName + "_" + studyNumber + ".txt";
        string path = "Assets/LogFiles/" + fileName;

        // Create File if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "\n\n****************CHRONOLOGICAL EVENT LOG******************\n\nDate and Time: " + DateTime + "\n" + "Group: " + groupName + "\n"
                      + "Number: " + studyNumber + "\n");
        }

        // Content of the files
        string content = "\nTime: " + eventTime 
            + ",  Time(CD): " + eventTimeCD 
            +  ",  Pause Event: " 
            + logmsg + "\n";
        // Add some to text to it
        File.AppendAllText(path, content);
    }

    public void CreateBallLog(float spawnTime, bool lifeState) {
        string DateTime = System.DateTime.Now.ToString();
        float ballSpawnTime = timerController.totalPlayTime -  spawnTime;
        float ballSpawnTimeCD = spawnTime;
        
        bool ballLifeState = lifeState;
        string fileName = "(Chronological Event Log) " + groupName + "_" + studyNumber + ".txt";
        string path = "Assets/LogFiles/" + fileName;
        ballCounter = ballCounter + 1;

        // Create File if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "\n\n****************CHRONOLOGICAL EVENT LOG******************\n\nDate and Time: " + DateTime + "\n" + "Group: " + groupName + "\n"
                      + "Number: " + studyNumber + "\n");
        }

        // Content of the files
        string content = "\nTime: " + ballSpawnTime + ",  Time(CD): " + ballSpawnTimeCD + ",   State: " + ballLifeState + ",  Number: " + ballCounter + "\n";
        // Add some to text to it
        File.AppendAllText(path, content);
    }


    public void CreateResultList() {

        WriteInterruptionLog();
        WritePauseLog();
        WriteResponseTimeLog();
        WriteShootingResultLog();

        //LOG TEXT  
        logTextStr = intrInfoStr + "\n\n"
            + playerPauseInfoStr + "\n\n"
            + responseTimeInfoStr + "\n\n"
            + ballInfoStr + "\n"
            + ballInfoTotalGameStr + "\n\n"
            + ballInfoBeforeBreakStr + "\n\n"
            + ballInfoAfterPauseStr + "\n\n"
            + ballInfoDuringIntStr;

        CreateTextFile();
    }


    /*
    * LOG CONTENT SETUP
    */

    public void SetTitlesForInterruptionLog() {
        intrInfoStr = "<Interruption(Intr) Time Info>";
        intrStartTimeStr = "\n (Intr Start) The time the bystander entered the room: ";
        intrSignalTimeStr = "\n (Intr Signal/Touch) The time the bystander gave the player a siganl: ";
        intrAskingTimeStr = "\n (Intr Asking) The time the bystander start asking the player a question: ";
        intrEndAskingTimeStr = "\n (Intr End Asking) The time the bystander end askig: ";
        intrLeavingTimeStr = "\n (Intr Leaving) The time the bystander start leaving the room: ";
        intrEndTimeStr = "\n (Intr End) The time the bystander closed the door: ";
        intrDurationStr = "\n (Intr Duration) The total time the bystander stayed in the room:";
    }

    public void SetTitlesForBreakTimeLog() {
        playerPauseInfoStr = "<Pause Time Info>";
        pauseNumberStr = "\n Number of breaks: ";
        pauseStartStr = "\n (Break Start) The time the player pressed the pause button: ";
        pauseEndStr = "\n (Break End) The time the player ended the break: ";
        pauseDurationStr = "\n (Break Duration): ";
        pauseDurationPercentageStr = "\n (Break Time/Total Play Time) Percentage of pause time in total time: ";
    }

    public void SetTitlesForResponseTimeLog() {
        responseTimeInfoStr = "<Response Time(RT) Info>";
        responseTimeStr = "\n RT after the start of the game: ";
        responseTimeFromIntStartStr = "\n RT after the appearance of the bystander: ";
        responseTimeFromSignalStr = "\n RT after receiving a signal/touch: ";
        responseTimeFromTalkingStr = "\n RT after receiving question: ";
        responseTimeFromEndAskingStr = "\n RT after listening a question: ";
    }

    public void SetTitlesForShotBallLog() {
        ballInfoStr = "<<BALL Info>>";
        ballInfoTotalGameStr = "<TOTAL GAME>";
        ballInfoBeforeBreakStr = "<BEFORE BREAK>";
        ballInfoAfterPauseStr = "<AFTER THE FIRST PAUSE>";
        ballInfoDuringIntStr = "<WHILE THE BYSTANDER IS IN THE ROOM>";
        playTimeStr = "\n play time: ";
        thrownBallsStr = "\n thrown Balls: ";
        shotBallsStr = "\n shot Balls: ";
        missedBallsStr = "\n missed Balls: ";
    }



    void WriteInterruptionLog() {
        float bystanderOpenDoorTime = timerController.AvrEnterTime;
        float bystanderSignalTime = timerController.AvrSignalTime;
        float bystanderAskingTime = timerController.AvrAskingTime;
        float bystanderEndAskingTime = timerController.AvrEndAskingTime;
        float bystanderLeavingTime = timerController.AvrLeavingTime;
        float bystanderCloseDoorTime = timerController.AvrCloseDoorTime;
        float bystanderDuration = bystanderCloseDoorTime - bystanderOpenDoorTime;
        intrInfoStr = intrInfoStr
                    + intrStartTimeStr + bystanderOpenDoorTime
                    + intrSignalTimeStr + bystanderSignalTime
                    + intrAskingTimeStr + bystanderAskingTime
                    + intrEndAskingTimeStr + bystanderEndAskingTime
                    + intrLeavingTimeStr + Mathf.Round(bystanderLeavingTime) + "(" + System.Math.Round(bystanderLeavingTime, 2) + ")"
                    + intrEndTimeStr + Mathf.Round(bystanderCloseDoorTime) + "(" + System.Math.Round(bystanderCloseDoorTime, 2) + ")"
                    + intrDurationStr + Mathf.Round(bystanderDuration) + "(" + System.Math.Round(bystanderDuration, 2) + ")";
    }

    void WritePauseLog() {
        pauseNumber = ballCalculator.GetPauseCounter();
        resumeNumer = ballCalculator.GetResumeCounter();

        if (pauseNumber == 0) {
            playerPauseInfoStr = playerPauseInfoStr + "\n The player did not take a break";
        }
        else {
            for (int i = 0; i < pauseNumber; i++) {
                string pauseIndexStr;
                if (i == 0) {
                    pauseIndexStr = "\n " + (i + 1) + "st pause";
                    float startBreakTime = timerController.GetStartBreakTime();
                    float endBreakTime = timerController.GetEndBreakTime();
                    float breakDuration = endBreakTime - startBreakTime;
                    double percentage = breakDuration / timerController.totalPlayTime;
                    playerPauseInfoStr = playerPauseInfoStr
                                + pauseNumberStr + pauseNumber
                                + pauseIndexStr
                                + pauseStartStr + Mathf.Round(startBreakTime) + "(" + System.Math.Round(startBreakTime, 2) + ")"
                                + pauseEndStr + Mathf.Round(endBreakTime) + "(" + System.Math.Round(endBreakTime, 2) + ")"
                                + pauseDurationStr + Mathf.Round(breakDuration) + "(" + System.Math.Round(breakDuration, 2) + ")"
                                + pauseDurationPercentageStr + string.Format("{0:P2}", percentage)
                                + "\n";
                }
                else if (i == 1) {
                    pauseIndexStr = "\n " + (i + 1) + "nd pause";
                    if (resumeNumer == i + 1) {
                        float startBreakTime = timerController.pStartBreakTimeList[i];
                        float endBreakTime = timerController.pEndBreakTimeList[i];
                        float breakDuration = endBreakTime - startBreakTime;
                        double percentage = breakDuration / timerController.totalPlayTime;
                        playerPauseInfoStr = playerPauseInfoStr
                                    + pauseIndexStr
                                    + pauseStartStr + Mathf.Round(startBreakTime) + "(" + System.Math.Round(startBreakTime, 2) + ")"
                                    + pauseEndStr + Mathf.Round(endBreakTime) + "(" + System.Math.Round(endBreakTime, 2) + ")"
                                    + pauseDurationStr + Mathf.Round(breakDuration) + "(" + System.Math.Round(breakDuration, 2) + ")"
                                    + pauseDurationPercentageStr + string.Format("{0:P2}", percentage)
                                    + "\n";
                    }
                    else {
                        float startBreakTime = timerController.pStartBreakTimeList[i];
                        playerPauseInfoStr = playerPauseInfoStr
                                   + pauseIndexStr
                                   + pauseStartStr + Mathf.Round(startBreakTime) + "(" + System.Math.Round(startBreakTime, 2) + ")"
                                   + "\n";
                    }
                }
                else {
                    pauseIndexStr = "\n " + (i + 1) + "th pause";
                    if (resumeNumer == i + 1) { // timeController.pStartBreakTimeList.Count == timeController.pEndBreakTimeList.Count
                        float startBreakTime = timerController.pStartBreakTimeList[i];
                        float endBreakTime = timerController.pEndBreakTimeList[i];
                        float breakDuration = endBreakTime - startBreakTime;
                        double percentage = breakDuration / timerController.totalPlayTime;
                        playerPauseInfoStr = playerPauseInfoStr
                                    + pauseIndexStr
                                    + pauseStartStr + Mathf.Round(startBreakTime) + "(" + System.Math.Round(startBreakTime, 2) + ")"
                                    + pauseEndStr + Mathf.Round(endBreakTime) + "(" + System.Math.Round(endBreakTime, 2) + ")"
                                    + pauseDurationStr + Mathf.Round(breakDuration) + "(" + System.Math.Round(breakDuration, 2) + ")"
                                    + pauseDurationPercentageStr + string.Format("{0:P2}", percentage)
                                    + "\n";
                    }
                    else {
                        float startBreakTime = timerController.pStartBreakTimeList[i];
                        playerPauseInfoStr = playerPauseInfoStr
                                  + pauseIndexStr
                                  + pauseStartStr + Mathf.Round(startBreakTime) + "(" + System.Math.Round(startBreakTime, 2) + ")"
                                  + "\n";
                    }
                }
            }
        }
    }


    void WriteResponseTimeLog() {
        if (pauseNumber == 0) {
            responseTimeInfoStr = responseTimeInfoStr + "\n The player did not take a break";
        }
        else {
            for (int i = 0; i < pauseNumber; i++) {
                string pauseIndex;
                if (i == 0) {
                    pauseIndex = "\n " + (i + 1) + "st pause";
                    float rt = timerController.GetStartBreakTime();
                    float rtFromAppearance = timerController.PlrRtFromAvrEnter;
                    float rtFromSiganl = timerController.PlrRtFromAvrSignal;
                    float rtFromAsking = timerController.PlrRtFromAvrAsking;

                    responseTimeInfoStr = responseTimeInfoStr
                           + pauseNumberStr + pauseNumber
                           + pauseIndex
                           + responseTimeStr + Mathf.Round(rt) + "(" + System.Math.Round(rt, 2) + ")"
                           + responseTimeFromIntStartStr + Mathf.Round(rtFromAppearance) + "(" + System.Math.Round(rtFromAppearance, 2) + ")" // + " /" + Mathf.Round(startBreakTime - rtFromAppearance) + "(" + (startBreakTime - rtFromAppearance) + ")"
                           + responseTimeFromSignalStr + Mathf.Round(rtFromSiganl) + "(" + System.Math.Round(rtFromSiganl, 2) + ")" //+ " /" + Mathf.Round(startBreakTime - rtFromSiganl) +"(" + (startBreakTime - rtFromSiganl) +")"
                           + responseTimeFromTalkingStr + Mathf.Round(rtFromAsking) + "(" + System.Math.Round(rtFromAsking, 2) + ")"// + " /" + Mathf.Round(startBreakTime - rtFromAsking) + "(" + (startBreakTime - rtFromAsking) + ")"
                           + "\n";
                }
                else if (i == 1) {
                    pauseIndex = "\n " + (i + 1) + "nd pause";
                    float rt = timerController.pStartBreakTimeList[i];
                    float rtFromAppearance = timerController.pStartBreakTimeList[i] - timerController.AvrEnterTime;
                    float rtFromSiganl = timerController.pStartBreakTimeList[i] - timerController.AvrSignalTime;
                    float rtFromAsking = timerController.pStartBreakTimeList[i] - timerController.AvrAskingTime;

                    responseTimeInfoStr = responseTimeInfoStr
                           + pauseIndex
                           + responseTimeStr + Mathf.Round(rt) + "(" + System.Math.Round(rt, 2) + ")"
                           + responseTimeFromIntStartStr + Mathf.Round(rtFromAppearance) + "(" + System.Math.Round(rtFromAppearance, 2) + ")" // + " /" + Mathf.Round(startBreakTime - rtFromAppearance) + "(" + (startBreakTime - rtFromAppearance) + ")"
                           + responseTimeFromSignalStr + Mathf.Round(rtFromSiganl) + "(" + System.Math.Round(rtFromSiganl, 2) + ")" //+ " /" + Mathf.Round(startBreakTime - rtFromSiganl) +"(" + (startBreakTime - rtFromSiganl) +")"
                           + responseTimeFromTalkingStr + Mathf.Round(rtFromAsking) + "(" + System.Math.Round(rtFromAsking, 2) + ")"// + " /" + Mathf.Round(startBreakTime - rtFromAsking) + "(" + (startBreakTime - rtFromAsking) + ")"
                           + "\n";
                }
                else {
                    pauseIndex = "\n " + (i + 1) + "th pause";
                    float rt = timerController.pStartBreakTimeList[i];
                    float rtFromAppearance = timerController.pStartBreakTimeList[i] - timerController.AvrEnterTime;
                    float rtFromSiganl = timerController.pStartBreakTimeList[i] - timerController.AvrSignalTime;
                    float rtFromAsking = timerController.pStartBreakTimeList[i] - timerController.AvrAskingTime;

                    responseTimeInfoStr = responseTimeInfoStr
                           + pauseIndex
                           + responseTimeStr + Mathf.Round(rt) + "(" + System.Math.Round(rt, 2) + ")"
                           + responseTimeFromIntStartStr + Mathf.Round(rtFromAppearance) + "(" + System.Math.Round(rtFromAppearance, 2) + ")" // + " /" + Mathf.Round(startBreakTime - rtFromAppearance) + "(" + (startBreakTime - rtFromAppearance) + ")"
                           + responseTimeFromSignalStr + Mathf.Round(rtFromSiganl) + "(" + System.Math.Round(rtFromSiganl, 2) + ")" //+ " /" + Mathf.Round(startBreakTime - rtFromSiganl) +"(" + (startBreakTime - rtFromSiganl) +")"
                           + responseTimeFromTalkingStr + Mathf.Round(rtFromAsking) + "(" + System.Math.Round(rtFromAsking, 2) + ")"// + " /" + Mathf.Round(startBreakTime - rtFromAsking) + "(" + (startBreakTime - rtFromAsking) + ")"
                           + "\n";
                }
            }
        }
    }

    void WriteShootingResultLog() {
        timerController.CalculatePurePlayTime();
        float purePlayTime = timerController.PlrPurePlayTime;
        double percentagePlayTime = (double)purePlayTime / (double)timerController.totalPlayTime;
        double percentageShotTotalGame = (double)ballCalculator.ShotBallCounter / (double)ballCalculator.ThrBallCounter;
        double percentageShotBeforeBreak = (double)ballCalculator.ShotBallCounterBP / (double)ballCalculator.ThrBallCounterBP;
        double percentageShotDuringInr = (double)ballCalculator.SBallCounterIntr / (double)ballCalculator.ThrBallCounterIntr;
        ballInfoTotalGameStr = ballInfoTotalGameStr
            + "\n total play duration: " + timerController.totalPlayTime
            + "\n play time: " + System.Math.Round(purePlayTime, 2) + "(" + string.Format("Percentage: {0:P2}", percentagePlayTime) + ")"
            + thrownBallsStr + ballCalculator.ThrBallCounter
            + shotBallsStr + ballCalculator.ShotBallCounter
            + missedBallsStr + ballCalculator.MissedBallCounter
            + "\n" + string.Format(" Percentage: {0:P2}", percentageShotTotalGame);    // (score.shotBallCounter / score.ThrownBalls) * 100 + "%";
        ballInfoBeforeBreakStr = ballInfoBeforeBreakStr
            + "\n time: " + System.Math.Round(timerController.GetStartBreakTime(), 2)
            + thrownBallsStr + ballCalculator.ThrBallCounterBP
            + shotBallsStr + ballCalculator.ShotBallCounterBP
            + missedBallsStr + ballCalculator.MissedBallCounterBP
            + "\n" + string.Format(" Percentage: {0:P2}", percentageShotBeforeBreak); // + score.GetShootingPercentBeforeInterruption()+"%";
        if (pauseNumber == 0) {
            ballInfoAfterPauseStr = ballInfoAfterPauseStr + "\n The player did not take a break";
        }
        else {
            int thrBallSum = 0;
            int shotBallSum = 0;
            int missedBallSum = 0;
            double percentageAfterBreak = 0;

            for (int i = 0; i < pauseNumber; i++) {
                if (i < resumeNumer) {
                    thrBallSum = thrBallSum + ballCalculator.thrBallCountListAP[i];
                    shotBallSum = shotBallSum + ballCalculator.sBallCountListAP[i];
                    missedBallSum = missedBallSum + ballCalculator.mBallCountListAP[i];
                    //  Debug.Log("thrBallSum" + thrBallSum);
                    // Debug.Log("shotBallSum" + shotBallSum);
                    // Debug.Log("missedBallSum" + missedBallSum);
                }
            }
            percentageAfterBreak = (double)shotBallSum / (double)thrBallSum;
            // Debug.Log("percentageAfterBreak" + percentageAfterBreak);
            ballInfoAfterPauseStr = ballInfoAfterPauseStr
               + "\n time: " + System.Math.Round(timerController.GetEndBreakTime(), 2)
               + thrownBallsStr + thrBallSum
               + shotBallsStr + shotBallSum
               + missedBallsStr + missedBallSum
               + "\n" + string.Format(" Percentage: {0:P2}", percentageAfterBreak);
        }

        /*
         * During Interuption
         */
        ballInfoDuringIntStr = ballInfoDuringIntStr
            + "\n duration: " + System.Math.Round((timerController.AvrCloseDoorTime - timerController.AvrEnterTime), 2)
            + thrownBallsStr + ballCalculator.ThrBallCounterIntr
            + shotBallsStr + ballCalculator.SBallCounterIntr
            + missedBallsStr + ballCalculator.MBallCounterIntr
        + "\n" + string.Format(" Percentage: {0:P2}", percentageShotDuringInr);

    }


    public void CreateTextFile() {
        // Path of the file
        string studyDateTime = System.DateTime.Now.ToString();
        string fileName = "(Keypoint Log) " + groupName + "_" + studyNumber + ".txt";
        string path = "Assets/LogFiles/" + fileName;         // Application.dataPath + "/Log.txt";
                                                             // Debug.Log("Application.dataPath: " + Application.dataPath);

        // Create File if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "\n\n****************STUDY RESULT LOG******************");
        }
        // Content of the files
        string content = "\n\nStudy Date and Time: " + System.DateTime.Now + "\n"
            + "Group: " + groupName
            + "\n\n"
            + logTextStr;

        // Add some to text to it
        File.AppendAllText(path, content);
    }
}
