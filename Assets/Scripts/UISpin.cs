using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpin : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void Start()
    {
        rectTransform.DOLocalRotate(new Vector3(0, 0, 360), 10f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Incremental);
    }
}
