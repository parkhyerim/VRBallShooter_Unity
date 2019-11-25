using UnityEngine;

public class LeftBullet : MonoBehaviour {
    private BallCalculator ballCalculator;
    private LogController logController;
    private ReadyBallsCounter readyBallCounter;
    private PreBallCounter preBallCounter;

    public float lifetime = 2f;

    // Prefab, das die Explosion realisiert.
    public GameObject explosionEffetPrefab;
    public GameObject shotSoundPrefab;

    void Start() {
        ballCalculator = FindObjectOfType<BallCalculator>();
        logController = FindObjectOfType<LogController>();
        readyBallCounter = FindObjectOfType<ReadyBallsCounter>();
        preBallCounter = FindObjectOfType<PreBallCounter>();
    }

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("LeftBall")) {
            LeftBall ball = collision.gameObject.GetComponent<LeftBall>();          // LeftBall Class of leftballPrefab
            ball.LifeState = true;                                                  // life state: true -> shot
            ballCalculator.ShotBallsCount();

            logController.AddBallInfoToList(ball.SpawnTime, true);
            logController.CreateBallLog(ball.SpawnTime, true);
           
            GameObject explosionEffectObj = Instantiate(explosionEffetPrefab, transform.position, transform.rotation);
            GameObject shotSoundObj = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);       // The bullet that removed the ball will disappear.
            Destroy(explosionEffectObj, 1);
            Destroy(shotSoundObj, 1);
        }

        if (collision.gameObject.CompareTag("ReadyBallLeft")) {
            GameObject explosionEffectObj = Instantiate(explosionEffetPrefab, transform.position, transform.rotation);
            GameObject shotSoundObj = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            readyBallCounter.ReadyBallCounter();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(explosionEffectObj, 1);
            Destroy(shotSoundObj, 1);          
        }

        if (collision.gameObject.CompareTag("PrBallLeft")) {
            GameObject explosionEffectObj = Instantiate(explosionEffetPrefab, transform.position, transform.rotation);
            GameObject shotSoundObj = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            preBallCounter.LeftBallCounter();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(explosionEffectObj, 1);
            Destroy(shotSoundObj, 1);
        }

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
