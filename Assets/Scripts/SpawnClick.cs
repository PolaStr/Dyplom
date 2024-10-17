using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations;
using Unity.VisualScripting;

public class SpawnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool buildMode = false;
    [SerializeField] private CinemachineVirtualCamera cmCam;
    [SerializeField] private SpectatorCamera camMovement;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    [SerializeField] private LayerMask layerMask;

    public float gridSize = 2.0f;

    private GameObject currentPreview;
    private GameObject currentPrefab;

    private float currentRotation = 0f;

    [SerializeField] private GameObject obj1, obj1Preview, obj2, obj2Preview;

    private Tween previewDT;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && buildMode == false)
        {
            buildMode = true;
            camMovement.enabled = false;
            cmCam.enabled = false;
            

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && buildMode!= false)
        {
            buildMode = false;
            camMovement.enabled = true;
            cmCam.enabled = true;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            DestroyPreviewAndTween();
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
            RotateObject();

            if (Input.GetMouseButtonDown(0) && currentPrefab != null)
            {
                PlacePrefab();
            }
        }

    }

    void HandleButtonPress(int buttonNumber)
    {
        DestroyPreviewAndTween();

        switch (buttonNumber)
        {
            case 1:
                Debug.Log("Button 1 pressed");
                currentPrefab = obj1;
                currentPreview = Instantiate(obj1Preview); // Instantiate the preview
                currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
                break;
            case 2:
                Debug.Log("Button 2 pressed");
                currentPrefab = obj2;
                currentPreview = Instantiate(obj2Preview); // Instantiate the preview
                currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
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

            Vector3 snappedPosition = SnapToGrid(hit.point);

            previewDT = currentPreview.transform.DOMove(snappedPosition, 0.2f);
        }
    }

    private void PlacePrefab()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);

            Instantiate(currentPrefab, snappedPosition, Quaternion.Euler(0, currentRotation, 0));

            impulseSrc.GenerateImpulse();
        }
    }

    private Vector3 SnapToGrid(Vector3 originalPosition)
    {
        float x = Mathf.Round(originalPosition.x / gridSize) * gridSize;
        float y = Mathf.Round(originalPosition.y / gridSize) * gridSize;
        float z = Mathf.Round(originalPosition.z / gridSize) * gridSize;

        return new Vector3(x, y, z);
    }

    private void DestroyPreviewAndTween()
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
    }

    private void RotateObject()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotation -= 90f; // Rotate counter-clockwise
            //currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            currentPreview.transform.DORotate(new Vector3(0, currentRotation, 0), 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRotation += 90f; // Rotate clockwise
                                    //currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            currentPreview.transform.DORotate(new Vector3(0, currentRotation, 0), 0.2f);
        }
    }
}
