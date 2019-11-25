using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandColor : MonoBehaviour
{
    public GameObject collidingGun;
    public GameObject objectInHand;

    public GameObject bulletPrefab;
    public float bulletSpeed = 2000f;

    public GameObject gunsoundPrefab;

    public bool attachedToHand;
    public bool gameFinished;

    public float shootingCooldown = 0.1f;
    private float shootingTimer = 0f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && other.gameObject.CompareTag("LeftGun"))      // check whether rigidBody is set
        {
            //Debug.Log("Trigger Enter for the left hand: " + other.gameObject.name);
            collidingGun = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LeftGun"))
        {
            collidingGun = null;
        }
    }

    //Update is called once per frame
    void Update()
    {
        shootingTimer -= Time.deltaTime;

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.1 && collidingGun && collidingGun.gameObject.CompareTag("LeftGun"))  //released:0 - push:1
        {
           // Debug.Log("Left collding: " + collidingGun);
            GrabObject();
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) <= 0.1 && objectInHand)
        {
            // Debug.Log("Left objectInhand: " + objectInHand);
            if (!attachedToHand)
            {
                RleaseObject();
            }
          
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && objectInHand && objectInHand.gameObject.CompareTag("LeftGun") && shootingTimer <= 0f)
        {
            Shoot();
        }
    }

    public void GrabObject()
    {
        objectInHand = collidingGun;
        objectInHand.transform.SetParent(transform);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;

    }

    public void RleaseObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand = null;
    }

    public void Shoot()
    {
        if (!gameFinished) {
            shootingTimer = shootingCooldown;

            GameObject gunSoundObject = Instantiate(gunsoundPrefab, objectInHand.transform.position + objectInHand.transform.forward * 0.25f,
             objectInHand.transform.rotation);
            Destroy(gunSoundObject, 1);

            GameObject bullet = Instantiate(bulletPrefab,
                objectInHand.transform.position + objectInHand.transform.forward * 0.25f,
                objectInHand.transform.rotation);  // 35cm in front of gun
            bullet.GetComponent<Rigidbody>().AddForce(objectInHand.transform.forward * bulletSpeed);
        }
        
    }
}
