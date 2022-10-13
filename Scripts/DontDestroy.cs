using UnityEngine;
// simple component to keep track of objects that musn't be destroyed during scene changes
public class DontDestroy : MonoBehaviour
{
    [HideInInspector] public string _id { get => name + transform.position.ToString() + transform.localEulerAngles.ToString(); }
    private void Start()
    {
        DontDestroy[] _foundObjects = FindObjectsOfType<DontDestroy>();
        for (int i = 0; i < _foundObjects.Length; i++)
        {
            if(_foundObjects[i]._id == this._id && _foundObjects[i] != this) Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}