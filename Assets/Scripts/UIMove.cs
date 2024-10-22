using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMove : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private bool windowOpen = false;

    //rectTransform.DOLocalRotate(new Vector3(0, 0, -180), 5f);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            switch(windowOpen)
            {
                case true:
                    rectTransform.DOAnchorPos(new Vector2(0, -1200), 1f);
                    windowOpen= false;
                    break;

                case false:
                    rectTransform.DOAnchorPos(new Vector2(0, 0), 1f);
                    windowOpen = true;
                    break;
            }
        }
    }
}
