using UnityEngine;

[CreateAssetMenu(fileName = "PropSO", menuName = "ScriptableObjects/PropSO", order = 1)]
public class PropSO : ScriptableObject
{
    public GameObject model;
    public string modelName = "noname";
    public Vector3 decalSize = new Vector3(1,1,1);
}
