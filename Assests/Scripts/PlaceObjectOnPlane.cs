using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class NewBehaviourScript : MonoBehaviour
{
    

    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    [SerializeField]
    private GameObject placeObjectPrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(placeObjectPrefab, hitPose.position, hitPose.rotation);
                
                Vector3 cameraPosition = Camera.main.transform.position;
                cameraPosition.y = spawnedObject.transform.position.y;
                spawnedObject.transform.LookAt(cameraPosition);

                spawnedObject.transform.rotation *= Quaternion.Euler(0, 90, 0); 
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;

                // Calculate the rotation needed to face the camera
                Vector3 cameraPosition = Camera.main.transform.position;
                cameraPosition.y = spawnedObject.transform.position.y;
                spawnedObject.transform.LookAt(cameraPosition);

                spawnedObject.transform.rotation *= Quaternion.Euler(0, 90, 0); // Rotate 180 degrees to face the camera
            }
        }
    }


}
