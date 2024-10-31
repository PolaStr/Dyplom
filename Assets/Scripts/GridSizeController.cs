using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridSizeController : MonoBehaviour
{
    public float gridScale = 2.0f;
    public float verticalGrid = 2.0f;
    public bool gridVisable = false;

    [SerializeField] private GameObject gridObj;
    [SerializeField] private Material grid;
    [SerializeField] private TextMeshProUGUI scaleText;
    [SerializeField] private float height = 0;

    private void Start()
    {
        grid.color = Color.clear;
    }

    private void Update()
    {
        ChangeGridHeight();

        if (Input.GetKeyDown(KeyCode.K))
        {
            gridScale *= 2;
            //ChangMatScale();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            gridScale /= 2;
            //ChangMatScale();
        }

        scaleText.text = gridScale.ToString();
    }
    public Vector3 SnapToGrid(Vector3 originalPosition)
    {
        float x = Mathf.Round(originalPosition.x / gridScale) * gridScale;
        //float y = Mathf.Round(originalPosition.y / gridSize) * gridSize;
        float y = height;
        float z = Mathf.Round(originalPosition.z / gridScale) * gridScale;

        return new Vector3(x, y, z);
    }

    private void ChangeGridHeight()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket) && height >= verticalGrid)
        {
            height -= verticalGrid;
            gridObj.transform.position = new Vector3(0, 0.1f + height, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            height += verticalGrid;
            gridObj.transform.position = new Vector3(0, 0.1f + height, 0);
        }
    }

    private void ChangMatScale()
    {
        switch (gridScale)
        {
            case 0.125f:
                grid.DOTiling(new Vector2(80, 80), 0.1f);
                break;
            case 0.25f:
                grid.DOTiling(new Vector2(40, 40), 0.1f);
                break;
            case 0.5f:
                grid.DOTiling(new Vector2(20, 20), 0.1f);
                break;
            case 1:
                grid.DOTiling(new Vector2(10, 10), 0.1f);
                break;
            case 2:
                grid.DOTiling(new Vector2(5, 5), 0.1f);
                break;
        }
    }

    public void FadeGrid()
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
