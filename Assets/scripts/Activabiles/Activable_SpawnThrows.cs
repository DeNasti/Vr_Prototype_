using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.Activabiles
{
    class Activable_SpawnThrows : MonoBehaviour, IActivable
    {
        public float power = 10f;
        public GameObject obj;

        public void Activate()
        {
            var instantiatedObj = Instantiate(obj);
            instantiatedObj.GetComponent<Rigidbody>()?.AddForce(Vector3.up* power, ForceMode.Impulse);
        }

        public void Deactivate()
        {
            return;
        }
    }
}
