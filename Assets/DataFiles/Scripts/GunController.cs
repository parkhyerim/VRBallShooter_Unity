using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public int gunCounter;
    public bool getANewGun;
    public GameObject gunPrefab;
    public GameObject rightGunPrefab;
    public GameObject leftGunPrefab;

    // Start is called before the first frame update
    void Start() {
        gunCounter = 0;
        getANewGun = false;
    }

    // If the gun falls to the ground, it counts one. 
    public void GunCounter() {
        gunCounter = gunCounter + 1;
        Debug.Log("gun to fall: " + gunCounter);
        getANewGun = true;
    }

    // When the gun falls to the ground, a new gun is automatically generated.
    public void GetANewGun(string name) {
        string gunName = name;
        GameObject gunObject;
        switch (gunName) {
            case "Gun":
                gunObject = Instantiate(gunPrefab);
                gunObject.transform.position = transform.position;
                break;
            case "LeftGun":
                gunObject = Instantiate(leftGunPrefab);
                gunObject.transform.position = leftGunPrefab.transform.position;
                break;
            case "RightGun":
                gunObject = Instantiate(rightGunPrefab);
                gunObject.transform.position = rightGunPrefab.transform.position;
                break;
            default:
                Debug.Log("default");
                break;
        }
    }
}
