using Assets.scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class HandInteractionController : MonoBehaviour
{
    public enum EquippedState
    {
        unequipped = 0,
        sword = 1,
        bow = 2,
        shield = 3
    }
    public enum EquipZoneType
    {
        none = 0,
        sword = 1,
        bow = 2
    }

    public GameObject shieldPrefab;
    public GameObject swordPrefab;
    public GameObject bowPrefab;
    public HandInteractionController oppositeHand;
    public GameObject thisControllerModel;
    public Material thisHandmaterialIndicator;

    private GameObject currentlyEquipped;

    public EquippedState handEquipState;
    public EquipZoneType currentEquipZone;

    private bool isPullingBow;
    private Sword thisSwordController;

    void Start()
    {
        handEquipState = EquippedState.unequipped;
        swordPrefab.SetActive(false);
        bowPrefab.SetActive(false);
    }



    public void HandActivated(bool gripped)
    {
        //if (gripped)
        //{
        //    handEquipState = EquippedState.shield;

        //    //ritorno perchè lo stato scudo è prioritario
        //    return;
        //}

        if (handEquipState == EquippedState.unequipped)
        {
            if (oppositeHand.handEquipState == EquippedState.bow)
            {
                if (TryPullBow(gripped))
                    return;
            }

            //if in sword-zone equip sword
            if (currentEquipZone == EquipZoneType.sword)
            {
                handEquipState = EquippedState.sword;
                currentlyEquipped = swordPrefab;
                currentlyEquipped.SetActive(true);

                thisSwordController = thisControllerModel.gameObject.GetComponent<Sword>();
                thisSwordController.ActivatePower();
                if (thisControllerModel)
                {
                    thisControllerModel.SetActive(false);
                }
            }

            //in bow zone equib bow
            if (currentEquipZone == EquipZoneType.bow)
            {
                handEquipState = EquippedState.bow;
                currentlyEquipped = bowPrefab;
                currentlyEquipped.SetActive(true);


                if (thisControllerModel)
                {
                    thisControllerModel.SetActive(false);
                }
            }

        }
    }

    private bool TryPullBow(bool extraPower)
    {
        var isPulled = oppositeHand.currentlyEquipped.GetComponent<Bow>().Pull(this.gameObject.transform);

        if (!isPulled)
            return false;

        if (thisControllerModel)
        {
            thisControllerModel.SetActive(false);
        }
        isPullingBow = true;
        return isPullingBow;

    }

    public void HandReleased(bool grip)
    {
        if (isPullingBow)
        {
            ReleaseBow();
        }
        else TryUnequip(grip);

        if (handEquipState == EquippedState.unequipped)
        {
            //disequipaggia tutto
            // Destroy(currentlyEquipped);
            if (currentlyEquipped)
                currentlyEquipped.SetActive(false);

        }

        if (thisControllerModel)
        {
            thisControllerModel.SetActive(true);
        }


    }

    private void TryUnequip(bool grip)
    {

        if (grip)
        {
            handEquipState = EquippedState.unequipped;
        }

        else
        {
            thisSwordController.DeactivatePower();
        }
    }

    private void ReleaseBow()
    {
        oppositeHand.currentlyEquipped.GetComponent<Bow>().Release();
        isPullingBow = false;
    }
}
