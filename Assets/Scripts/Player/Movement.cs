using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float speed = 7;

        private float _inputX;
        private float _inputY;
        private Rigidbody2D _rb;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        private void Update()
        {
            _inputX = Input.GetAxis("Horizontal");
            _inputY = Input.GetAxis("Vertical");

            speed = Input.GetKeyDown(KeyCode.Joystick1Button0) ? 12 : speed;
        }

        private void FixedUpdate() => _rb.velocity = new Vector2(_inputX * speed, _inputY * speed);
    }
}