using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTool : MonoBehaviour
{
    [SerializeField] private Camera maincamera;
    [SerializeField] private LayerMask deletableObj;
    [SerializeField] private GameObject destroyObj;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, deletableObj))
            {
                GameObject pain = Instantiate(destroyObj, hit.point, Quaternion.identity);

                Destroy(hit.collider.gameObject);
                Destroy(pain, 0.2f);
            }


        }
    }
}
