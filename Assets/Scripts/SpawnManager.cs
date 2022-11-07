using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    GameObject objectPrefab;

    [SerializeField]
    Transform parent;

    GameObject go;

    //Spawn object AR
    public void Spawn()
    {
        go = Instantiate(objectPrefab, parent);
    }

    //Remove object AR
    public void Remove()
    {
        Destroy(go);
    }
}
