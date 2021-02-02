using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

namespace Autohand.Demo{
    public class OVRHandControllerLink : MonoBehaviour{
        public Hand hand;
        public OVRInput.Controller controller;
        public OVRInput.Axis1D grabAxis;
        public OVRInput.Button grabButton;
        public OVRInput.Button squeezeButton;

        public void Update() {
            if(OVRInput.GetDown(grabButton, controller)) {
                hand.Grab();
                hand.gripOffset += 1;
            }
            if(OVRInput.GetUp(grabButton, controller)) {
                hand.Release();
                hand.gripOffset -= 1;
            }
            if(OVRInput.GetDown(squeezeButton, controller)) {
                hand.Squeeze();
            }
            if(OVRInput.GetUp(squeezeButton, controller)) {
                hand.Unsqueeze();
            }
            hand.SetGrip(OVRInput.Get(grabAxis, controller));
        }
    }
}
