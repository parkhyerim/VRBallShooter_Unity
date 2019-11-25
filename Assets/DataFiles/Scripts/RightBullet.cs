using UnityEngine;

public class RightBullet : MonoBehaviour {
    private BallCalculator ballCalculator;
    private LogController logController;
    private ReadyBallsCounter readyBallCounter;
    private PreBallCounter preBallCounter;

    public float lifetime = 2f;

    // Prefab, das die Explosion realisiert.
    public GameObject explosionEffectPrefab;
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

        if (collision.gameObject.CompareTag("RightBall")) {
            RightBall ball = collision.gameObject.GetComponent<RightBall>();  // RightBall Class of rightballPrefab
            ball.LifeState = true;      // life state: true -> shot
            ballCalculator.ShotBallsCount();

            logController.AddBallInfoToList(ball.SpawnTime, true);
            logController.CreateBallLog(ball.SpawnTime, true);
                   
            GameObject explosionEffectObj = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            GameObject shotSoundObj = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);  // the ball disappear
            Destroy(gameObject);          // The bullet that removed the ball will disappear.
            Destroy(explosionEffectObj, 1);   
            Destroy(shotSoundObj, 1);
        }

        if (collision.gameObject.CompareTag("ReadyBallRight")) {
            GameObject explosionEffectObj = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            GameObject shotSoundObj = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            readyBallCounter.ReadyBallCounter();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(explosionEffectObj, 1);
            Destroy(shotSoundObj, 1);          
        }

        if (collision.gameObject.CompareTag("PrBallRight")) {
            GameObject explosition = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            GameObject shotSound = Instantiate(shotSoundPrefab, transform.position, transform.rotation);
            preBallCounter.RightBallCounter();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(explosition, 1);
            Destroy(shotSound, 1);           
        }

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
