using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtils;
using Assets.scripts.Utils;

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
        if (other.tag.Equals(Tags.BowEquipZone) || other.tag.Equals(Tags.SwordEquipZone))
        {
            return;
        }

        ArrowImpact(other);

    }

    private void ArrowImpact(Collider other)
    {
        var velocityPreImpact = rb.velocity;
        rb.isKinematic = true;
        rb.useGravity = false;
        var particle = Instantiate(impactParticle, tip.transform.position, tip.rotation);

        StartCoroutine(HandleGameObject.KillParticleAfterSeconds(particle, 3f));

        other.gameObject.GetComponent<Rigidbody>()?.AddForceAtPosition(velocityPreImpact, tip.position, ForceMode.Impulse);
        transform.parent = other.gameObject.transform;
    }


}
