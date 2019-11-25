using UnityEngine;

public class RightGun : MonoBehaviour {
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
            // To create only one gun
            if (isColliding) return;
            isColliding = true;        
            Destroy(gameObject);
            gunController.GetANewGun("RightGun");
        }
    }
}
