using UnityEngine;

public class LeftBall : MonoBehaviour {
    private BallCalculator ballCalculator;
    private LogController logController;
    private TimerController timerController;

    public float speed = 3.5f;
    public Vector3 direction;

    private float spawnTime;
    private bool lifeState;

   /*
   *  public setter and getter methods to access and update the value of a private variable.
   */
    public bool LifeState { get => lifeState; set => lifeState = value; }
    public float SpawnTime { get => spawnTime; set => spawnTime = value; }

    void Start() {
        ballCalculator = FindObjectOfType<BallCalculator>();
        logController = FindObjectOfType<LogController>();
        timerController = FindObjectOfType<TimerController>();

        // The time when the ball is generated
        SpawnTime = timerController.RemainingTime;  // From 0 second

        // decide ball spawn position (left1, left2, center, right1, right2)
        float xPos = transform.localPosition.x;          
        float xAbs = Mathf.Abs(xPos);                     // Absolute x-value

        if (xPos > 0) {
            if (xAbs > 8) {
                direction = new Vector3(direction.x * Random.Range(-2f, -1.6f), direction.y * Random.Range(1f, 1.2f), direction.z);
            }
            else {
                direction = new Vector3(direction.x * -1f, direction.y * Random.Range(1f, 1.2f), direction.z);
            }
        }
        else if (xPos < 0) {
            if (xAbs > 8) {
                direction = new Vector3(direction.x * Random.Range(1.6f, 2f), direction.y * Random.Range(1f, 1.2f), direction.z);
            }
            else {
                direction = new Vector3(direction.x * 1f, direction.y * Random.Range(1f, 1.2f), direction.z);
            }
        }
        else {
            direction = new Vector3(direction.x * 0f, direction.y, direction.z);
        }
    }

    void Update() {
        transform.position += speed * direction * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) {
            ballCalculator.MissedBallsCount(); 
            logController.AddBallInfoToList(SpawnTime, false);  // spawn time, life state: false -> missed
            logController.CreateBallLog(SpawnTime, false);
            Destroy(gameObject);
        }
    }
}
