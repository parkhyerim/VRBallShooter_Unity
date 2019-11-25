using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandColor : MonoBehaviour {
    public GameObject collidingGun;
    public GameObject objectInHand;

    public GameObject bulletPrefab;
    public float bulletSpeed = 2000f;
    public GameObject gunsoundPrefab;

    public bool attachedToHand;
    public bool gameFinished;

    public float shootingCooldown = 0.1f;
    private float shootingTimer = 0f;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>() && other.gameObject.CompareTag("RightGun"))      // Check whether rigidBody is set and the colliding objet is gun
        {
            collidingGun = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("RightGun")) {
            collidingGun = null;
        }
    }


    void Update() {
        shootingTimer -= Time.deltaTime;

        // detecting input
        //released:0 <-> pushed full:1
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.1 && collidingGun && collidingGun.gameObject.CompareTag("RightGun")) {
            GrabObject();
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) <= 0.1 && objectInHand) {
            if (!attachedToHand) {              // When the main game begins, the gun is attached to the hand 
                ReleaseObject();
            }
        }

        // When the trigger is pulled, the bullet is fired.
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && objectInHand && objectInHand.gameObject.CompareTag("RightGun") && shootingTimer <= 0f) 
        {
            Shoot();
        }
    }

    // grab function and releasing object
    public void GrabObject() {
        objectInHand = collidingGun;
        objectInHand.transform.SetParent(this.transform);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;  //If isKinematic is enabled, Forces, collisions or joints will not affect the rigidbody anymore.
    }

    public void ReleaseObject() {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand = null;
    }

    public void Shoot() {
        if (!gameFinished) {
            shootingTimer = shootingCooldown;

            GameObject gunSoundObject = Instantiate(gunsoundPrefab, 
                objectInHand.transform.position + objectInHand.transform.forward * 0.25f, 
                objectInHand.transform.rotation);
            Destroy(gunSoundObject, 1);

            GameObject bullet = Instantiate(bulletPrefab,
                objectInHand.transform.position + objectInHand.transform.forward * 0.25f,
                objectInHand.transform.rotation);  // 25cm in front of gun
            bullet.GetComponent<Rigidbody>().AddForce(objectInHand.transform.forward * bulletSpeed);
        }
    }
}

