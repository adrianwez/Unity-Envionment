using UnityEngine;
// Basic script to keep the player in a squared zone with basic transform manipulation
public class ZoneLimit : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right,
        Forward,
        Back
    }
    [SerializeField] private Side _side;                        // the side of the squared zone
    [SerializeField] private int _counter = 0;                  // sumbolic int to count how many times the player reached a limit zone
    [SerializeField] private float _ratio = 2;                  // width/lenght ratio
    [SerializeField] private float _hightOffset = .5f;          // an offset to keep player from not reaching enough hight
    [SerializeField] private float _distanceOffset;             // Distance in the opposite diraction that the play will spawn

    private void OnTriggerEnter(Collider _col)
    {
        if(_col.CompareTag("Player"))
        {
            _counter ++;            // sumbolic counter
            switch (_side)
            {
                // move player to the opposite side
                case Side.Left:
                    _col.transform.position = new Vector3(transform.position.x + _distanceOffset * _ratio, _col.transform.position.y + _hightOffset, _col.transform.position.z);
                    break;
                // move player to the opposite side
                case Side.Right:
                    _col.transform.position = new Vector3(transform.position.x - _distanceOffset * _ratio, _col.transform.position.y + _hightOffset, _col.transform.position.z);
                    break;
                // move player to the opposite side
                case Side.Forward:
                    _col.transform.position = new Vector3(_col.transform.position.x, _col.transform.position.y + _hightOffset, _col.transform.position.z - _distanceOffset * _ratio);
                    break;
                case Side.Back:
                // move player to the opposite side
                    _col.transform.position = new Vector3(_col.transform.position.x, _col.transform.position.y + _hightOffset, _col.transform.position.z + _distanceOffset * _ratio);
                    break;
            }
        }
    }
}