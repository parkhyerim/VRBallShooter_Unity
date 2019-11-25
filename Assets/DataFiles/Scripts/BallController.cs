using UnityEngine;

public class BallController : MonoBehaviour {
    public GameObject leftBallPrefab;         // It can be shot with the LEFT gun.
    public GameObject rightBallPrefab;        // It can be shot with the RIGHT gun.

    public float ballSpawnInterval = 1.5f;
    public float ballTimer = -0.5f;          // After the start of the game, how many seconds later the ball interval is activated: e.g., 0.5 seconds later
    private float setBallTimerValue;

    private BallCalculator ballCalculator;
    private TimerController timerController;
    private GameController gameController;

    // Ball spawning positions(transform)
    public Transform leftBallPos, leftBallPos2;
    public Transform centerBallPos;
    public Transform rightBallPos, rightBallPos2;

    private int randomNumber, lastRanPositionNum;
    private bool canSpawnBalls;

    /*
     *   public setter and getter methods to access and update the value of a private variable.
     */
    public bool CanSpawnBalls { get => canSpawnBalls; set => canSpawnBalls = value; }

    void Start() {
        ballCalculator = FindObjectOfType<BallCalculator>();
        timerController = FindObjectOfType<TimerController>();
        gameController = FindObjectOfType<GameController>();
        randomNumber = 1;
        setBallTimerValue = ballTimer;
    }

    void Update() {
        if (gameController.GameReady) {     
            CanSpawnBalls = true;
            ballTimer += Time.deltaTime;     // -(ballTimer) seconds later the spawning interval starts 
        } else {
            CanSpawnBalls = false;
        }

        if (ballTimer > ballSpawnInterval) {
            if (!gameController.PauseButtonPressed) {   // pauseButton pressed true -> not spawn balls 
                ballTimer = 0f;
                if (CanSpawnBalls) {
                    SpawnBall();
                }
            } else {
                ballTimer = setBallTimerValue;
            }
        }
    }

    void SpawnBall() {
        GameObject ballObject;
        ballCalculator.ThrownBallsCount();       // The total Number of thrown Balls
        // left ball or right ball
        int ballNum = Random.Range(0, 2);
        if (ballNum == 0) {
            ballObject = Instantiate(leftBallPrefab);    
        } else {
            ballObject = Instantiate(rightBallPrefab);         
        }

        // Which spawner
        int randomPositionNum = GetRandomNum(0, 5);
        lastRanPositionNum = randomPositionNum;

        switch (randomPositionNum) {
            case 0:
                ballObject.transform.position = leftBallPos.position;
                break;

            case 1:
                ballObject.transform.position = rightBallPos.position;
                break;

            case 2:
                ballObject.transform.position = centerBallPos.position;
                break;

            case 3:
                ballObject.transform.position = leftBallPos2.position;
                break;

            case 4:
                ballObject.transform.position = rightBallPos2.position;
                break;

            default:
                ballObject.transform.position = centerBallPos.position;
                Debug.Log("default");
                break;
        }  
    }

    private int GetRandomNum(int min, int max) {
        randomNumber = Random.Range(min, max);
        if (randomNumber == lastRanPositionNum) {
            GetRandomNum(min, max);
        }
        return randomNumber;
    }
}
