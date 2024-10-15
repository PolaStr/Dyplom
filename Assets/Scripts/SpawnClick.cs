using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnClick : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject previewPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool buildMode = false;
    [SerializeField] private CinemachineBrain cmCam;
    [SerializeField] private SpectatorCamera camMovement;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    [SerializeField] private LayerMask layerMask;

    private GameObject previewInstance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = true;
            //camMovement.enabled = false;
            //cmCam.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (previewInstance == null)
            {
                previewInstance = Instantiate(previewPrefab);
            }
        }

        if (buildMode)
        {
            UpdatePreviewPosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlacePrefab();
            }
        }
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            previewInstance.transform.DOMove(new Vector3(hit.point.x, hit.point.y, hit.point.z), 0.2f);
        }
    }

    private void PlacePrefab()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Instantiate(prefabToSpawn, hit.point, Quaternion.identity);

            impulseSrc.GenerateImpulse();
        }
    }

    public void ExitBuildMode()
    {
        buildMode = false;
        camMovement.enabled = true;
        cmCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }
}
