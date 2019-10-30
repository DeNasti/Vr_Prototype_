using Assets.scripts.Utils;
using MyInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpringController : MonoBehaviour
{

    public Material buttonMaterial;
    public GameObject objectToActivate;

    IActivable interfaceActivable;
    int i = 0;
    Color[] ColorArray = { Color.red, Color.green, Color.blue, Color.cyan, Color.white, Color.black };


    private void Start()
    {
        interfaceActivable = objectToActivate.GetComponent<IActivable>();
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == Tags.ButtonTrigger)
        {
            buttonMaterial.color = ColorArray[i];

            i++;

            if (i == ColorArray.Length - 1)
                i = 0;

            interfaceActivable.Activate();
        }
    }
}
