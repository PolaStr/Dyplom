using UnityEngine;

[CreateAssetMenu(fileName = "PropSO", menuName = "ScriptableObjects/PropSO", order = 1)]
public class PropSO : ScriptableObject
{
    public GameObject model;
    public string modelName;
    public Vector3 spriteSize;
}
