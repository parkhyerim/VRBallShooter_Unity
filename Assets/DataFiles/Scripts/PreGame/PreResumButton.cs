using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreResumButton : MonoBehaviour {
    private Vector3 buttonStartPosition;
    private Color buttonColor;
    public TextMesh resumeText;
    private PrePauseButton pauseButton;
    private PreGameController gameController;
    private PrePauseController pauseController;
    public bool resumeBtnClicked;
    private int pauseShootCounter;

    // Start is called before the first frame update
    void Start() {
        pauseButton = FindObjectOfType<PrePauseButton>();
        pauseController = FindObjectOfType<PrePauseController>();
        gameController = FindObjectOfType<PreGameController>();
        buttonStartPosition = transform.localPosition;
        // buttonColor = GetComponent<MeshRenderer>().material.color;
        buttonColor = pauseButton.GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider other) {
        if ((other.gameObject.CompareTag("RightBullet") || other.gameObject.CompareTag("LeftBullet") || other.gameObject.CompareTag("Bullet"))) {
            Destroy(other.gameObject); // Destroy Bullet immediately

            if (!resumeBtnClicked && pauseButton.pauseBtnClicked) {
                // gameController.EndPause();
                // Debug.Log("RESUME BUTTON");
                ActivateResumeButton();
                pauseButton.DeactivatePauseButton();
                ;
            }
        }
    }

    public void ActivateResumeButton() {
        pauseController.SetResumeGameMode();
        GetComponent<MeshRenderer>().material.color = Color.gray;
        // resumeText.text = "RESUME";
        resumeText.color = new Color32(170, 170, 170, 255);
        transform.localPosition = new Vector3(transform.localPosition.x - 0.2f, transform.localPosition.y, transform.localPosition.z);
        if (pauseShootCounter < 1) {
            gameController.ShootPause();
            pauseShootCounter = pauseShootCounter + 1;
        }
        else if(pauseShootCounter == 1) {
            gameController.EndInstruction();
        }  
        resumeBtnClicked = true;
    }

    public void DeactivateResumeButton() {
        if (resumeBtnClicked) {
            transform.localPosition = buttonStartPosition;
            GetComponent<MeshRenderer>().material.color = buttonColor;
            resumeText.color = Color.white;
            resumeBtnClicked = false;
        }
        else {
            GetComponent<MeshRenderer>().material.color = buttonColor;
        }

    }
}
