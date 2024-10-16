using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool buildMode = false;
    [SerializeField] private CinemachineBrain cmCam;
    [SerializeField] private SpectatorCamera camMovement;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    [SerializeField] private LayerMask layerMask;

    private GameObject currentPreview;
    private GameObject currentPrefab;

    [SerializeField] private GameObject obj1, obj1Preview, obj2, obj2Preview;

    private Tween previewDT;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = true;
            //camMovement.enabled = false;
            //cmCam.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (buildMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleButtonPress(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HandleButtonPress(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                HandleButtonPress(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                HandleButtonPress(4);
            }

            UpdatePreviewPosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlacePrefab();
            }
        }
    }

    void HandleButtonPress(int buttonNumber)
    {
        if (previewDT != null && previewDT.IsActive())
        {
            previewDT.Kill();
        }
        // Destroy the existing preview before creating a new one
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        switch (buttonNumber)
        {
            case 1:
                Debug.Log("Button 1 pressed");
                currentPrefab = obj1;
                currentPreview = Instantiate(obj1Preview); // Instantiate the preview
                break;
            case 2:
                Debug.Log("Button 2 pressed");
                currentPrefab = obj2;
                currentPreview = Instantiate(obj2Preview); // Instantiate the preview
                break;
            case 3:
                Debug.Log("Button 3 pressed");
                // Add your logic here
                break;
            case 4:
                Debug.Log("Button 4 pressed");
                // Add your logic here
                break;
            default:
                Debug.Log("Unknown button pressed");
                break;
        }
    }


    private void UpdatePreviewPosition()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && currentPreview != null)
        {
            if (previewDT != null && previewDT.IsActive())
            {
                previewDT.Kill();
            }

            previewDT = currentPreview.transform.DOMove(new Vector3(hit.point.x, hit.point.y, hit.point.z), 0.2f);
        }
    }

    private void PlacePrefab()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Instantiate(currentPrefab, hit.point, Quaternion.identity);

            impulseSrc.GenerateImpulse();
        }
    }

    //public void ExitBuildMode()
    //{
    //    buildMode = false;
    //    camMovement.enabled = true;
    //    cmCam.enabled = true;
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    if (currentPreview != null)
    //    {
    //        Destroy(currentPreview);
    //    }
    //}
}
