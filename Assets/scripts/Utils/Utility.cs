using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyUtils
{
    public class HandleGameObject
    {

        public static IEnumerator KillParticleAfterSeconds(GameObject obj, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            UnityEngine.Object.Destroy(obj);
        }
    }
}
