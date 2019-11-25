using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePauseButton : MonoBehaviour {
    private Vector3 buttonStartPosition;
    private Color buttonColor;
    public TextMesh pauseText;
    private PreResumButton resumeButton;
    private PrePauseController pauseController;
    public bool pauseBtnClicked;
    private PreGameController gameController;

    private int resumeShootCounter;

    // Start is called before the first frame update
    void Start() {
        resumeButton = FindObjectOfType<PreResumButton>();
        pauseController = FindObjectOfType<PrePauseController>();
        gameController = FindObjectOfType<PreGameController>();
        buttonStartPosition = transform.localPosition;
        buttonColor = GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider other) {
        if ((other.gameObject.CompareTag("RightBullet") || other.gameObject.CompareTag("LeftBullet") || other.gameObject.CompareTag("Bullet"))) {

            Destroy(other.gameObject);      // Destroy Bullet immediately

            if (!pauseBtnClicked) {
                ActivatePauseButton();
                resumeButton.DeactivateResumeButton();
            }
        }
    }

    private void ActivatePauseButton() {
        pauseController.SetPauseMode();
        GetComponent<MeshRenderer>().material.color = Color.gray;
        
        pauseText.color = new Color32(170, 170, 170, 255);
        //transform.position = new Vector3(-0.2f, transform.position.y, transform.position.z);
        transform.localPosition = new Vector3(transform.localPosition.x - 0.2f, transform.localPosition.y, transform.localPosition.z);
        if(resumeShootCounter < 2) {
            gameController.ShootResume();
            resumeShootCounter = resumeShootCounter + 1;
        }
        pauseBtnClicked = true;
    }

    public void DeactivatePauseButton() {
        if (pauseBtnClicked) {
            transform.localPosition = buttonStartPosition;
            GetComponent<MeshRenderer>().material.color = buttonColor;
            pauseText.color = Color.white;
            pauseBtnClicked = false;
        }
    }
}
