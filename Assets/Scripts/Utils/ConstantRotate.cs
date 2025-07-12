using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _axis = Vector3.up;

    [SerializeField] private GameObject _around;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_around.transform.position, _axis, _speed);
    }

    void Reset()
    {
        _around = gameObject;
    }
}
