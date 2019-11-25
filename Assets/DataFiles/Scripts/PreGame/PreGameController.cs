using UnityEngine;

public class PreGameController : MonoBehaviour {
    public TextMesh instructionText;
    public TextMesh pauseBoardText;
    private RightBullet rightBullet;
    private LeftBullet leftBullet;
    private float timer = -1;
    private bool grabGun;
    private bool shootWithRightGun;
    private bool shotPause;


    // Start is called before the first frame update
    void Start() {
        rightBullet = FindObjectOfType<RightBullet>();
        leftBullet = FindObjectOfType<LeftBullet>();
        instructionText.text = "Welcome!\n If you feel comfortable with your headset, raise your hand.";
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            GrabGun();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ShootWithRightGun();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            ShootPause();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            ShootWithLeftGun();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            EndInstruction();
        }
    }

    public void GrabGun() {
        if (!grabGun) {
            instructionText.text = "Let's try to take a blue gun with the right controller\n and a yellow gun with the left controller!";
            grabGun = true;
        }
        else {
            ShootWithRightGun();
        }     
    }

    public void ShootWithRightGun() {
        if(!shootWithRightGun) {
            instructionText.text = "Now shoot blue balls with your blue gun!";
            shootWithRightGun = true;
        }
        else {
            ShootPause();
        }
    }

    public void ShootWithLeftGun() {
        instructionText.text = "Good job!\n Now shoot yellow balls with your left yellow gun!";
    }

    public void PausePractice() {
        instructionText.text = "You can learn how to pause the game.\nTurn right and look at the board.";
    }

    public void ShootPause() {
        pauseBoardText.text = "Shoot the pause button!\n The game will be paused.\nLet's try it now!";
    }

    public void ShootResume() {
        pauseBoardText.text = "Shoot the resume button!\n The game will be restarted.\nLet's try it now!";
    }

    public void EndInstruction() {
        instructionText.text = "You did a good job.\nYour practice game is finished.";
        pauseBoardText.text = "You did a good job.\nYour practice game is finished.";
    }

}
