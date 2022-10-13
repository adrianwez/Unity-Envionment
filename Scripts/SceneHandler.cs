using UnityEngine;
using UnityEngine.SceneManagement;
public enum HandlerType
{
    Trigger,
    Distance
}
public class SceneHandler : MonoBehaviour
{
    private string _sceneToload { get => name; }
    [SerializeField] private HandlerType _type;

    private void OnTriggerEnter(Collider _collider)
    {
        if(_collider.CompareTag("Player"))
            if(!SceneManager.GetSceneByName(_sceneToload).isLoaded) SceneManager.LoadSceneAsync(_sceneToload, LoadSceneMode.Additive);
    }
    private void OnTriggerExit(Collider _collider)
    {
        if(_collider.CompareTag("Player"))
            SceneManager.UnloadSceneAsync(_sceneToload);
    }
}