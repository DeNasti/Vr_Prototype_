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

    void Start()
    {
        handEquipState = EquippedState.unequipped;
        swordPrefab.SetActive(false);
        bowPrefab.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Tags.BowEquipZone))
        {
            currentEquipZone = EquipZoneType.bow;

            if (thisHandmaterialIndicator)
                thisHandmaterialIndicator.color = Color.cyan;

        }
        else if (other.CompareTag(Tags.SwordEquipZone))
        {
            currentEquipZone = EquipZoneType.sword;

            if (thisHandmaterialIndicator)
                thisHandmaterialIndicator.color = Color.red;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.BowEquipZone) || other.CompareTag(Tags.SwordEquipZone))
        {
            currentEquipZone = EquipZoneType.none;
            if (thisHandmaterialIndicator)
                thisHandmaterialIndicator.color = Color.white;
        }
    }


    public void HandActivated(bool shield)
    {
        if (shield)
        {
            handEquipState = EquippedState.shield;

            //ritorno perchè lo stato scudo è prioritario
            return;
        }

        if (handEquipState == EquippedState.unequipped)
        {
            if (oppositeHand.handEquipState == EquippedState.bow)
            {
                if (TryPullBow())
                    return;
            }

            //if in sword-zone equip sword
            if (currentEquipZone == EquipZoneType.sword)
            {
                handEquipState = EquippedState.sword;
                currentlyEquipped = swordPrefab;
                currentlyEquipped.SetActive(true);

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

    private bool TryPullBow()
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

    public void HandReleased(bool shield)
    {
        if (isPullingBow)
        {
            ReleaseBow();
        }
        else TryUnequip(shield);

        if (handEquipState == EquippedState.unequipped)
        {
            //disequipaggia tutto
            // Destroy(currentlyEquipped);
            if(currentlyEquipped)
                currentlyEquipped.SetActive(false);

        }

        if (thisControllerModel)
        {
            thisControllerModel.SetActive(true);
        }
    }

    private void TryUnequip(bool shield)
    {
        //passo allo stato "unequipped" solo in due casi: 
        //1) mollo lo scudo quando lo ho in mano
        //2) mollo la spada/arco quando li ho in mano
        if (handEquipState == EquippedState.shield && shield)
        {
            handEquipState = EquippedState.unequipped;
        }
        else if (handEquipState != EquippedState.shield && !shield)
        {
            handEquipState = EquippedState.unequipped;
        }
    }

    private void ReleaseBow()
    {
        oppositeHand.currentlyEquipped.GetComponent<Bow>().Release();
        isPullingBow = false;
    }
}
