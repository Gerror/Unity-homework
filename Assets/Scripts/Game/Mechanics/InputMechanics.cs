using UnityEngine;

namespace Game.Mechanics
{
    public class InputMechanics : MonoBehaviour
    {
        private Camera _mainCamera;
        void Start()
        {
            _mainCamera = Camera.main;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                clickPoint.z = 0f;

                var col = Physics2D.OverlapPoint(clickPoint);
                if (col)
                {
                    col.GetComponent<SpawnObjectMechanics>().OnClick();
                }
            }
        }
    }
}