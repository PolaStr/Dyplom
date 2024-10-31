using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.HighDefinition;
using System.Collections.Generic;

public class SpawnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool buildMode = false;
    [SerializeField] private CinemachineVirtualCamera cmCam;
    [SerializeField] private SpectatorCamera camMovement;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    [SerializeField] private LayerMask layerMask;

    private GameObject currentPreview;
    private GameObject currentPrefab;

    private float currentRotation = 0f;

    [SerializeField] private List<PropSO> props;
    [SerializeField] private GameObject buildRing;

    [SerializeField] private GridSizeController gridController;

    private Tween previewDT;
    private Tween ringPreviewDT;
    private GameObject currentBuildRing;

    private PropSO selectedProp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && buildMode == false)
        {
            buildMode = true;
            //camMovement.enabled = false;
           // cmCam.enabled = false;
            gridController.FadeGrid();
            gridController.gridVisable = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && buildMode!= false)
        {
            buildMode = false;
            //camMovement.enabled = true;
            //cmCam.enabled = true;

            gridController.FadeGrid();
            gridController.gridVisable = false;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currentBuildRing.SetActive(false);

            DestroyPreviewAndTween();
        }


        if (buildMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleButtonPress(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HandleButtonPress(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                HandleButtonPress(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                HandleButtonPress(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
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

    public void HandleButtonPress(int selectedPropIndex)
    {
        DestroyPreviewAndTween();

        if (currentBuildRing == null)
        {
            // Instantiate the build ring if it doesn't exist
            currentBuildRing = Instantiate(buildRing);
        }
        else
        {
            // Update the existing build ring
            currentBuildRing.SetActive(true);
        }

        if (selectedPropIndex < 0 || selectedPropIndex >= props.Count) return;

        Debug.Log($"Button {selectedPropIndex + 1} pressed");
        PropSO selectedProp = props[selectedPropIndex];

        currentPrefab = selectedProp.model;

        currentPreview = Instantiate(selectedProp.model);
        currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        currentPreview.GetComponent<Collider>().enabled = false;

        currentBuildRing.GetComponent<DecalProjector>().size = selectedProp.decalSize;
    }



    private void UpdatePreviewPosition()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, layerMask) && currentPreview != null)
        {
            if (previewDT != null && previewDT.IsActive())
            {
                previewDT.Kill();
            }

            Vector3 snappedPosition = gridController.SnapToGrid(hit.point);

            previewDT = currentPreview.transform.DOMove(snappedPosition, 0.2f);
            ringPreviewDT = currentBuildRing.transform.DOMove(snappedPosition, 0.2f);
        }
    }

    private void PlacePrefab()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // If it's over UI, stop the raycast from going further
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, layerMask))
        {
            Vector3 snappedPosition = gridController.SnapToGrid(hit.point);

            Instantiate(currentPrefab, snappedPosition, Quaternion.Euler(0, currentRotation, 0));

            impulseSrc.GenerateImpulse();
        }
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation -= 90f; // Rotate counter-clockwise
            //currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            currentPreview.transform.DORotate(new Vector3(0, currentRotation, 0), 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentRotation += 90f; // Rotate clockwise
                                    //currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            currentPreview.transform.DORotate(new Vector3(0, currentRotation, 0), 0.2f);
        }
    }
}
