using UnityEngine;

public class ReadyBallsCounter : MonoBehaviour {
    private GameController gameController;
    public int ballCounter;

    void Start() {
        gameController = FindObjectOfType<GameController>();
    }

    void Update() {
        if (ballCounter >= 6) {
            gameController.GameReady = true;
        }
    }

    public void ReadyBallCounter() {
        ballCounter = ballCounter + 1;
    }
}
