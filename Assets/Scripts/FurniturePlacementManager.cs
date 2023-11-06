using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FurniturePlacementManager : MonoBehaviour {
    
    public GameObject SpawnableFurniture;

    public ARSessionOrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    // Update()
    private void Update() {
        if (Input.touchCount > 0) { // if there is a touch

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                bool collision = raycastManager.Raycast(touch.position, raycastHits, TrackableType.PlaneWithinPolygon);
                
                if (collision && !isButtonPressed()) {                          // if there is a collision and the button is not pressed
                    GameObject _object = Instantiate(SpawnableFurniture);
                    _object.transform.position = raycastHits[0].pose.position;  // move the object to the plane
                    _object.transform.rotation = raycastHits[0].pose.rotation;  // rotate the object to match the plane
                }

                foreach (var plane in planeManager.trackables) {
                    plane.gameObject.SetActive(false);                          // hide the plane
                }

                planeManager.enabled = false;                                   // disable the plane manager
            }
        }
    }

    // isButtonPressed()
    public bool isButtonPressed() {
        if (EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() == null) { // if the current selected object is a button
            return false;
        } else {
            return true;
        }
    }

    // SwitchFurniture(-furniture)
    // switch the furniture to be spawned
    public void SwitchFurniture(GameObject furniture) { 
        SpawnableFurniture = furniture;
    }

}
