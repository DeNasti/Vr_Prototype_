using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private float arrowForce = 20f;
    public Transform tip;
    public GameObject impactParticle;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    internal void Fire(float pullPercentage)
    {

        var actualFireStrength = pullPercentage * arrowForce;

        rb.isKinematic = false;
        rb.useGravity = true;
        this.transform.parent = null;

        rb.AddForce(this.transform.forward * actualFireStrength, ForceMode.Impulse);

    }

    private void OnTriggerEnter(Collider other)
    {
            rb.isKinematic = true;
            rb.useGravity = false;
            Instantiate(impactParticle, tip.transform.position, tip.rotation);
    }

}
