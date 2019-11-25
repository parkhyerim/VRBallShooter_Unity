using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGun : MonoBehaviour {
    GunController gunController;
    bool isColliding;

    // Start is called before the first frame update
    void Start() {
        gunController = FindObjectOfType<GunController>();
    }

    // Update is called once per frame
    void Update() {
        isColliding = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor")) {
            if (isColliding) return;
            isColliding = true;
            // Debug.Log("Collision with Floor");
            Destroy(gameObject);
            gunController.GetANewGun("LeftGun");
        }
    }
}
