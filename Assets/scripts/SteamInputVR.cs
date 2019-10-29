using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Valve.VR;

public class SteamInputVR : MonoBehaviour
{
    public SteamVR_Action_Boolean pullAction;
    public SteamVR_Action_Boolean gripAction;
    public GameObject charController;

    public GameObject handR;
    public GameObject handL;

    private HandInteractionController controllerR;
    private HandInteractionController controllerL;

    private SteamVR_Behaviour_Pose poseR;
    private SteamVR_Behaviour_Pose poseL;
    private Transform cameraRig;
    private Transform head;

    private CharacterControllerMover characterControllerMover;
    private void Start()
    {
        controllerR = handR.GetComponent<HandInteractionController>();
        poseR = handR.GetComponent<SteamVR_Behaviour_Pose>();

        controllerL = handL.GetComponent<HandInteractionController>();
        poseL = handL.GetComponent<SteamVR_Behaviour_Pose>();

        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;

        characterControllerMover = new CharacterControllerMover(charController, cameraRig, head, 0.1f, 0.2f);
    }


    // Update is called once per frame
    void Update()
    {
        TryInput(controllerR, poseR);
        TryInput(controllerL, poseL);

        characterControllerMover.Move();
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

 
}
