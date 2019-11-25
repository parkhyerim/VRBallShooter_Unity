using UnityEngine;

public class PreBallCounter : MonoBehaviour {
    public GameObject yellowBallsPrefab;
    public int rightBallCounter;
    public int leftBallCounter;
    private float timer = -2.5f;
    private PreGameController gameController;
    bool yellowBallGenerated;
    bool leftBallAllShot;

    void Start() {
        gameController = FindObjectOfType<PreGameController>();
    }

    void Update() {
        if (rightBallCounter >= 5) {
            timer += Time.deltaTime;
           
            if(timer > 0f) {
                Debug.Log(timer);
                if (!yellowBallGenerated) {
                    GenerateYellowBalls();
                    yellowBallGenerated = true;
                    timer = -2.5f;
                }
            }
        }

        if(leftBallCounter >= 5) {
            timer += Time.deltaTime;
            if(timer > 0f) {
                Debug.Log(timer);
                if (!leftBallAllShot) {
                   // gameController.GameReady = true;
                     gameController.PausePractice();
                    leftBallAllShot = true;
                    timer = -2.5f;
                }
            }
        }
    }

    public void RightBallCounter() {
        rightBallCounter = rightBallCounter + 1;
    }

    public void LeftBallCounter() {
        leftBallCounter = leftBallCounter + 1;
    }

    public void GenerateYellowBalls() {
        GameObject yellowBallsObj = Instantiate(yellowBallsPrefab, transform.position, transform.rotation);

        gameController.ShootWithLeftGun();
    }
}
