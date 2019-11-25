using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDUserController : MonoBehaviour {
    private TimerController timer;
    private BallCalculator ballCalculator;
    private NonHMDUserController avatar;

    Vector3 hitPoint = new Vector3(0, 1, 0);

    public bool startPause;
    public bool endPause;

    [Header("Break Time Information")]
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private float firstStartBreakTime;
    [SerializeField]
    private float firstEndBreakTime, firstBreakDuration;
    [SerializeField]
    private float numOfBreaks;
    [SerializeField]
    private List<float> startBreakTimeList, endBreakTimeList, breakDurtationList;

    [Header("Response Time Information")]
    [SerializeField]
    private float rtFromAvrEnter;
    [SerializeField]
    private float rtFromAvrSignal, rtFromAvrAsking, rtFromStartOfGame;
    [SerializeField]
    private List<float> rtFromAvrEnterList, rtFromAvrSignalList, rtFromAvrAskingList;
    
    /*
     *  public setter and getter methods to access and update the value of a private variable.
     */
    public float FirstStartBreakTime { get => firstStartBreakTime; set => firstStartBreakTime = value; }
    public float FirstEndBreakTime { get => firstEndBreakTime; set => firstEndBreakTime = value; }
    public float FirstBreakDuration { get => firstBreakDuration; set => firstBreakDuration = value; }
    public float NumOfBreaks { get => numOfBreaks; set => numOfBreaks = value; }
    public float RtFromAvrEnter { get => rtFromAvrEnter; set => rtFromAvrEnter = value; }
    public float RtFromAvrSignal { get => rtFromAvrSignal; set => rtFromAvrSignal = value; }
    public float RtFromAvrAsking { get => rtFromAvrAsking; set => rtFromAvrAsking = value; }
    public float RtFromStartOfGame { get => rtFromStartOfGame; set => rtFromStartOfGame = value; }

    // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private void Start() {
        timer = FindObjectOfType<TimerController>();
        ballCalculator = FindObjectOfType<BallCalculator>();
        avatar = FindObjectOfType<NonHMDUserController>();
    }

    void Update() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
            // public static bool Raycast =>  return bool True if the ray intersects with a Collider, otherwise false.
            // Camera.main: CenterEyeAnchor
            hitPoint = hit.point;   // point: The impact point in world space where the ray hit the collider.
        }
    }

    private void OnDrawGizmos() {
        // Draws a red line from hitPoint transform to the main camera postion
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hitPoint, Camera.main.transform.position);  //  public static void DrawLine(Vector3 from, Vector3 to);
        // Gizmos.DrawSphere(Camera.main.transform.position, 0.2f);
    }

    public void SetStartBreakTimeList(float time) {
        startBreakTimeList.Add(time);
    }

    public List<float> GetStartBreakTimeList() {
        return startBreakTimeList;
    }

    public void SetEndBreakTimeList(float time) {
        endBreakTimeList.Add(time);
    }

    public List<float> GetEndBreakTimeList() {
        return endBreakTimeList;
    }

    public void SetBreakDurationList(float duration) {
        breakDurtationList.Add(duration);
    }

    public List<float> GetBreakDurationList() {
        return breakDurtationList;
    }

    public void SetStartPause() {
        startPause = true;
    }

    public void SetEndPause() {
        endPause = true;
    }
}
