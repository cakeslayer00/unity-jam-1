using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slide : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private float _direction = 1f;

    private void Start()
    {

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _direction *= -1f;
        }
        transform.Translate(_direction * Vector2.up * (_speed * Time.deltaTime));
    }
}
