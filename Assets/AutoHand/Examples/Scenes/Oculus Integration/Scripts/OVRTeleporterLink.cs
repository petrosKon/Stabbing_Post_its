using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand.Demo{
    public class OVRTeleporterLink : MonoBehaviour{
        public Teleporter teleport;
        public OVRInput.Controller controller;
        public OVRInput.Button teleportButton;

        public void Update() {
            if(OVRInput.GetDown(teleportButton, controller)) {
                teleport.StartTeleport();
            }
            if(OVRInput.GetUp(teleportButton, controller)) {
                teleport.Teleport();
            }
        }
    }
}
