using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.HighDefinition;

public class SpawnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool buildMode = false;
    [SerializeField] private CinemachineVirtualCamera cmCam;
    [SerializeField] private SpectatorCamera camMovement;
    [SerializeField] private CinemachineImpulseSource impulseSrc;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Material grid;

    public float gridSize = 2.0f;

    private GameObject currentPreview;
    private GameObject currentPrefab;

    private float currentRotation = 0f;

    //[SerializeField] private GameObject obj1, obj1Preview, obj2, obj2Preview, obj3, obj3Preview;

    [SerializeField] private PropSO prop1, prop2, prop3;
    [SerializeField] private GameObject buildRing;

    private Tween previewDT;
    private Tween ringPreviewDT;
    private GameObject currentBuildRing;
    private bool gridVisable = false;

    private void Start()
    {
        grid.color = Color.clear;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && buildMode == false)
        {
            buildMode = true;
            camMovement.enabled = false;
            cmCam.enabled = false;
            FadeGrid();
            gridVisable = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && buildMode!= false)
        {
            buildMode = false;
            camMovement.enabled = true;
            cmCam.enabled = true;

            FadeGrid();
            gridVisable = false;


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

            if (!buildMode && currentBuildRing != null)
            {
                Destroy(currentBuildRing);
                currentBuildRing = null;
            }


            if (Input.GetMouseButtonDown(0) && currentPrefab != null)
            {
                PlacePrefab();
            }
        }

    }

    public void HandleButtonPress(int buttonNumber)
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

        switch (buttonNumber)
        {
            case 1:
                Debug.Log("Button 1 pressed");
                currentPrefab = prop1.model;

                currentPreview = Instantiate(prop1.model);
                currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
                currentPreview.GetComponent<Collider>().enabled = false;

                buildRing.GetComponent<DecalProjector>().size = prop1.decalSize;
                break;
            case 2:
                Debug.Log("Button 2 pressed");
                currentPrefab = prop2.model;

                currentPreview = Instantiate(prop2.model);
                currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

                buildRing.GetComponent<DecalProjector>().size = prop2.decalSize;
                break;
            case 3:
                Debug.Log("Button 3 pressed");
                currentPrefab = prop3.model;

                currentPreview = Instantiate(prop3.model);
                currentPreview.transform.rotation = Quaternion.Euler(0, currentRotation, 0);

                buildRing.GetComponent<DecalProjector>().size = prop3.decalSize;
                break;
            case 4:
                Debug.Log("Button 4 pressed");
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
            ringPreviewDT = buildRing.transform.DOMove(snappedPosition, 0.2f);
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

    private void FadeGrid()
    {
        if (gridVisable == true)
        {
            grid.DOColor(new Vector4(1, 1, 1, 0f), 0.5f)
                .SetEase(Ease.InOutQuad);
        }
        else if (gridVisable == false)
        {
            grid.DOColor(new Vector4(1, 1, 1, 0.25f), 0.5f)
                .SetEase(Ease.InOutQuad);
        }
    }
}
