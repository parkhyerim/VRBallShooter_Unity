using System;
using UnityEngine;
using UnityEngine.AI;

public class NonHMDUserController : MonoBehaviour {
    private NavMeshAgent avatarAI;
    private Animator avatarAnim;
   
    public GameObject player;
    public GameObject questionSoundPrefab;
    public GameObject signalSoundPrefab;
    public GameObject okaySoundPrefab;
    private TimerController timerController;
    private PauseController pauseController;
    private GameController gameController;
    private LogController logController;

    [Header("Avatar Position Information")]
    [SerializeField]
    private Vector3 originalPosition;           // Where the avatar originally stands (in front of the door)
    public GameObject firstStopPosition;        // Where the avatar stands to talk to the player
    
    [Header("<Setting> Group")]
    public bool isAvatarInMixedReality;
    public bool isAvatarOnly;

    [Header("<Setting> Timer: Countdown from the beginning of the game")]
    public float enterTimeSetting;          // The time the bystander/avatar enters the room
    public float walkingTimeToPlayerSetting;        // Time to go from door to player 
    public float startSpeakingTimeSetting;  // Time to start speaking;
    public float speakDurationSetting = 5;
    public float walkingTimeToDoorSetting;
    public float maxStayingTimeSetting;     // The time between Entering the room and Starting to leave
    public float minStayingTimeSetting;     // The time the bystander start to leave (in case the player clicked on the Pause and Resume button too soon)

    [Header("Time Info(Countdown)")]
    [SerializeField]
    private float totalPlayTime;
    [SerializeField]
    private float remainingTime;
    [SerializeField]
    private float enterRoomTimeCD;           // enter time: the avatar/bystander departs to interrupt the player
    [SerializeField]
    private float signalTimeCD, startSpeakingTimeCD, stopAskingTimeCD;
    [SerializeField]
    private float maxEndTimeCD, minEndTimeCD, endTimeCD;  // end Time: the avatar/bystander leaves after interrupting, maxEndTime: if the player does not click a button
    [SerializeField]
    private float closeDoorTimeCD;
    [SerializeField]
    private float giveSignalTimeCD, startAskingTimeCD, endAskingTimeCD, endAnsweringTimeCD;
    // public bool playerPaused, playerEndedPause;
    private bool interruptionStarted, signalGiven, questionAsked, stopAsked, playerAnswered, endIntrCalled, questionVoicePlayed, signalVoicePlayed;

    /*
     *  public setter and getter methods to access and update the value of a private variable.
     */
    public float EnterTimeCD { get => enterRoomTimeCD; set => enterRoomTimeCD = value; }
    public float EndTimeCD { get => endTimeCD; set => endTimeCD = value; }
    public float SignalTimeCD { get => signalTimeCD; set => signalTimeCD = value; }
    public float StartAskingTimeCD { get => startSpeakingTimeCD; set => startSpeakingTimeCD = value; }
    public float CloseDoorTimeCD { get => closeDoorTimeCD; set => closeDoorTimeCD = value; }

    void Awake() {
        avatarAI = GetComponent<NavMeshAgent>();
        avatarAnim = GetComponent<Animator>();
    }

    void Start() {
        timerController = FindObjectOfType<TimerController>();
        pauseController = FindObjectOfType<PauseController>();
        gameController = FindObjectOfType<GameController>();
        logController = FindObjectOfType<LogController>();
        
        originalPosition = transform.position;             // The position where the bystander originally stands (in front of the door)

        totalPlayTime = timerController.totalPlayTime;
        EnterTimeCD = timerController.totalPlayTime - enterTimeSetting;
        SignalTimeCD = EnterTimeCD - walkingTimeToPlayerSetting;
        StartAskingTimeCD = SignalTimeCD - startSpeakingTimeSetting;
        stopAskingTimeCD = StartAskingTimeCD - speakDurationSetting;
        maxEndTimeCD = EnterTimeCD - maxStayingTimeSetting;
        minEndTimeCD = EnterTimeCD - minStayingTimeSetting;
        EndTimeCD = maxEndTimeCD;                                   // (temporal) The time to end interruption will be changed later when the player has finished his break.
        CloseDoorTimeCD = EndTimeCD - walkingTimeToDoorSetting;          // (temporal) The time to close the door will be changed later when the player has finished his break.

        // Set Time info for Time Controller
        timerController.AvrEnterTime = totalPlayTime - EnterTimeCD;
        timerController.AvrSignalTime = totalPlayTime - SignalTimeCD;
        timerController.AvrAskingTime = totalPlayTime - StartAskingTimeCD;
        timerController.AvrEndAskingTime = totalPlayTime - stopAskingTimeCD;
        timerController.AvrLeavingTimeALAP = totalPlayTime - maxEndTimeCD;
        timerController.AvrLeavingTime = totalPlayTime - EndTimeCD;             
        timerController.AvrCloseDoorTime = totalPlayTime - CloseDoorTimeCD;   // because the endtime would be changed 
    }

    void Update() {
        remainingTime = timerController.RemainingTime;

        // The time between the Non-HMD user ENTERING the room and STANDING at the marked position.
        if ((timerController.RemainingTime < EnterTimeCD) && (timerController.RemainingTime >= SignalTimeCD)) {
            if (!interruptionStarted) {
                gameController.StartInterruption();             // the interruption starts
                logController.CreateInterruptionLog(timerController.RemainingTime, "Start: Open the door");
                interruptionStarted = true;
            }
          
            if (isAvatarInMixedReality || isAvatarOnly) {
                // if an avatar stands at the first start position AND stand near to the player(the standPosition)
                if (avatarAI.remainingDistance != Mathf.Infinity && avatarAI.remainingDistance < avatarAI.stoppingDistance) {
                    avatarAI.destination = firstStopPosition.transform.position;
                    FacePlayer(player.transform.position);
                    avatarAnim.SetBool("IsWalking", false);
                }
                else {
                    avatarAnim.SetBool("IsWalking", true);
                }
            }
            // the interrupter is a real person
            else {
                // Debug.Log("The bystander enters the room and walks to the player." + timeController.RemainingTime);
            }
        }

        // The time between STANDING at the marked position and the BEGINNING of the CONVERSATION with the player
        if ((timerController.RemainingTime < SignalTimeCD) && (timerController.RemainingTime >= StartAskingTimeCD)) {
            if (!signalGiven) {
                logController.CreateInterruptionLog(timerController.RemainingTime, "Signal");
                signalGiven = true;
            }
            if (isAvatarInMixedReality || isAvatarOnly) {
                WaveHand();
            }
            else {
                // Debug.Log("The bystander touches the shoulder of the player" + timeController.RemainingTime);
            }  
        }

        // The time in which the bystander asks the player a question
        if ((timerController.RemainingTime < StartAskingTimeCD) && (timerController.RemainingTime >= stopAskingTimeCD)) {
            if (!questionAsked) {
                logController.CreateInterruptionLog(timerController.RemainingTime, "Asking");
                questionAsked = true;
            }
            if (isAvatarInMixedReality || isAvatarOnly) {
                StopWavingHand();
                StartAsking();
            }
            else {
                // Debug.Log("The bystander asks the player a question" + timeController.RemainingTime);
            }
        }

        if((timerController.RemainingTime < stopAskingTimeCD) && (timerController.RemainingTime >= EndTimeCD)) {
            if (!stopAsked) {
                logController.CreateInterruptionLog(timerController.RemainingTime, "Stop Asking");
                stopAsked = true;
            }
            if (isAvatarInMixedReality || isAvatarOnly) {
                StopTalking();
                NodHead();
            }
            else {
                // Debug.Log("The bystander listens to the player" + timeController.RemainingTime);
            }
        }

        if(timerController.RemainingTime < EndTimeCD) {
            StopNoddingHead();
            StopTalking();
            StopWavingHand();
            if (!endIntrCalled){
                SetEndInterruptionTime();
                logController.CreateInterruptionLog(timerController.RemainingTime, "Start leaving");
                if (isAvatarOnly) {
                    GameObject okaySoundObject = Instantiate(okaySoundPrefab, transform.position, transform.rotation);
                    Destroy(okaySoundObject, 2);
                }
                endIntrCalled = true;
            }
            if (isAvatarInMixedReality || isAvatarOnly) {
                if (avatarAI.remainingDistance != Mathf.Infinity && avatarAI.remainingDistance < avatarAI.stoppingDistance) {
                    avatarAI.destination = originalPosition;
                    avatarAnim.SetBool("IsWalking", false);
                }
                else {
                    avatarAnim.SetBool("IsWalking", true);
                }
            }
            else {
                // Debug.Log("The bystander leaves the room" + timeController.RemainingTime);
            }       
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            SetEndInterruptionTime();
            playerAnswered = true;
        }

        if (Input.GetMouseButtonDown(1)) {
            questionVoicePlayed = false;
            StartAsking();
        }
    }

    // The avatar faces the player
    private void FacePlayer(Vector3 destination) {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, maxStayingTimeSetting * UnityEngine.Time.deltaTime); //???????????????? stayingTimeForSetting
    }


    public void SetEndInterruptionTime() {
      //  Debug.Log("SetendInterruptionTime clciked");
        float timeToGoCDChecker = timerController.RemainingTime;
        if((timeToGoCDChecker < stopAskingTimeCD) && (timeToGoCDChecker < minEndTimeCD)) {   // If the bystander does not ask a question, he can not go
            EndTimeCD = timeToGoCDChecker;
        }
        else {
            EndTimeCD = minEndTimeCD;
        }
        CloseDoorTimeCD = EndTimeCD - walkingTimeToDoorSetting;
        timerController.AvrLeavingTime = totalPlayTime - EndTimeCD;
        timerController.AvrCloseDoorTime = totalPlayTime - CloseDoorTimeCD;
        // Debug.Log("EndTime is set: " + EndTimeCD);
    }

    // The avatar waves his Hand == the bystander touches the shoulder of the player
    private void WaveHand() {
        avatarAnim.ResetTrigger("StopWaving");
        avatarAnim.SetTrigger("WavingHand");
        FacePlayer(player.transform.position);
        if (!signalVoicePlayed && isAvatarOnly) {
            GameObject heySoundObject = Instantiate(signalSoundPrefab, transform.position, transform.rotation);
            Destroy(heySoundObject, 2);
            signalVoicePlayed = true;
        }

        if (giveSignalTimeCD == 0f) {
            giveSignalTimeCD = timerController.RemainingTime;
           // Debug.Log("Waving Hand Time: " + Mathf.Round(wavingHandTime) + "(" + wavingHandTime + ")");
        }
    }

    private void StopWavingHand() {
        avatarAnim.ResetTrigger("WavingHand");
        avatarAnim.SetTrigger("StopWaving");
    }

    private void StartAsking() {
        avatarAnim.ResetTrigger("StopTalking");
        avatarAnim.SetTrigger("Talking");
        FacePlayer(player.transform.position);
        if (!questionVoicePlayed && isAvatarOnly) {
            GameObject questionObject = Instantiate(questionSoundPrefab, transform.position, transform.rotation);
            Destroy(questionObject, 3);
            questionVoicePlayed = true;
        }
        
        if (startAskingTimeCD == 0f) {
            startAskingTimeCD = timerController.RemainingTime;
           // Debug.Log("Start Talking Time: " + Mathf.Round(startAskingTime) + "(" + startAskingTime + ")");
        }
    }

    private void StopTalking() {
        avatarAnim.ResetTrigger("Talking");
        avatarAnim.SetTrigger("StopTalking");
        if (endAskingTimeCD == 0f) {
            endAskingTimeCD = timerController.RemainingTime;
            // Debug.Log("Stop Talking Time: " + Mathf.Round(endAskingTime) + "(" + endAskingTime + ")");
        }
    }

    private void NodHead() {
        avatarAnim.ResetTrigger("StopNodding");
        avatarAnim.SetTrigger("NoddingHead"); 
    }

    private void StopNoddingHead() {
        avatarAnim.ResetTrigger("NoddingHead");
        avatarAnim.SetTrigger("StopNodding");
        if (endAnsweringTimeCD == 0f && playerAnswered) {
            endAnsweringTimeCD = timerController.RemainingTime;
           // Debug.Log("End Answering Time: " + Mathf.Round(endAnsweringTime) + "(" + endAnsweringTime + ")");
        }
    }
}
