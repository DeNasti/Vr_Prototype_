using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamInputVR : MonoBehaviour
{
    public SteamVR_Action_Boolean pullAction;
    public SteamVR_Action_Boolean gripAction;

    public GameObject handR;
    public GameObject handL;

    HandInteractionController controllerR;
    HandInteractionController controllerL;

    SteamVR_Behaviour_Pose poseR;
    SteamVR_Behaviour_Pose poseL;

    private void Start()
    {
        controllerR = handR.GetComponent<HandInteractionController>();
        poseR = handR.GetComponent<SteamVR_Behaviour_Pose>();

        controllerL = handL.GetComponent<HandInteractionController>();
        poseL = handL.GetComponent<SteamVR_Behaviour_Pose>();

    }


    // Update is called once per frame
    void Update()
    {
        TryInput(controllerR, poseR);
        TryInput(controllerL, poseL);
    }

    private void TryInput(HandInteractionController thisController, SteamVR_Behaviour_Pose thisPose)
    {

        //premo e mollo il grip
        if (gripAction.GetStateDown(thisPose.inputSource))
        {
            thisController.HandActivated(true);
            return;
        }

        if (gripAction.GetStateUp(thisPose.inputSource))
        {
            thisController.HandReleased(true);
            return;
        }

        //premo e mollo il trigger
        if (pullAction.GetStateDown(thisPose.inputSource))
        {
            thisController.HandActivated(false);
            return;
        }

        if (pullAction.GetStateUp(thisPose.inputSource))
        {
            thisController.HandReleased(false);
            return;
        }
    }

    private void HandleHead()
    {
    }

    private void HandleHeight()
    {
    }


    private void CalculateMovement()
    {
    }
}
