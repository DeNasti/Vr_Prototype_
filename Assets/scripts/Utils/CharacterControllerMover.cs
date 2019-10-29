using UnityEngine;

namespace Utils
{
    class CharacterControllerMover
    {
        private GameObject charControllerObj;
        private CharacterController charControllerCC;
        private Transform cameraRig;
        private Transform head;
        private float sensitivity;
        private float maxSpeed;
        private float curSpeed;

        private Transform colliderMover;

        public CharacterControllerMover(GameObject charController, Transform cameraRig, Transform head, float sensitivity, float maxSpeed)
        {
            this.charControllerObj = charController;
            this.cameraRig = cameraRig;
            this.head = head;
            this.sensitivity = sensitivity;
            this.maxSpeed = maxSpeed;

            charControllerCC = charControllerObj.GetComponent<CharacterController>();
            colliderMover = charControllerObj.transform.GetChild(1);
        }

        public void Move()
        {
            HandleHead();
            HandleHeight();
            CalculateMovement();

            colliderMover.position = new Vector3(head.position.x, colliderMover.position.y, head.position.z );
        }

        private void HandleHead()
        { 
            //store current
            Vector3 oldPos = cameraRig.position;
            Quaternion oldRotation = cameraRig.rotation;

            //rotation
            charControllerObj.transform.eulerAngles = new Vector3(0f, head.rotation.eulerAngles.y, 0f);

            ////restore
            cameraRig.position = oldPos;
            cameraRig.rotation = oldRotation;
        }

    

        private void CalculateMovement()
        {
            //Find direction
            var orientetionEuler = new Vector3(0, charControllerObj.transform.eulerAngles.y, 0f);
            var orientetion = Quaternion.Euler(orientetionEuler);
            var movement = Vector3.zero;

            //if not moving
            if (false) //controller released
            {
                curSpeed = 0f;
            }

            //if button pressed
            if (false)
            {
                //add, clamp
                curSpeed += 1 * sensitivity; //getAxis
                curSpeed = Mathf.Clamp(curSpeed, -maxSpeed, maxSpeed);

                //orientation
                movement += orientetion * (curSpeed * Vector3.forward) * Time.deltaTime;
            }

            //apply
            charControllerCC.Move(movement);
        }

        private void HandleHeight()
        {
            //set head height
            float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
            charControllerCC.height = headHeight;

            //set Center
            var newCenter = Vector3.zero;
            newCenter.y = headHeight / 2;
            newCenter.y += charControllerCC.skinWidth;

            //move Capsule in local space 
            newCenter.x = head.localPosition.x;
            newCenter.z = head.localPosition.z;

            //rotate
            newCenter = Quaternion.Euler(0, -charControllerObj.transform.eulerAngles.y, 0) * newCenter;

            charControllerCC.center = newCenter;

        }

    }
}
