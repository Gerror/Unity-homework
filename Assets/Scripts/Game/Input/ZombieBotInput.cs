using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.AI;

namespace Input
{
    [RequireComponent(typeof(ZombieComponent))]
    public class ZombieBotInput : PlayerInput
    {
        [SerializeField] private GameObject _eye;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ZombieComponent _zombieComponent;
        [SerializeField] private LayerMask _firemanLayerMask;
        [SerializeField] private LayerMask _wallLayerMask;
        [SerializeField] private Vector3[] _deltaPath;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _stepLength;

        private Vector3 _initPosition;
        private int _currentPoint = 0;

        public GameObject Target = null;

        private void Awake()
        {
            _initPosition = transform.position;
            _gameSettings = FindObjectOfType<GameSettings>();
            _zombieComponent = GetComponent<ZombieComponent>();
        }

        private void Start()
        {
            StartCoroutine(ProcessSensors());
        }

        private IEnumerator ProcessSensors()
        {
            while (_zombieComponent.IsAlive)
            {
                yield return new WaitForSeconds(_gameSettings.GetTimeToThinkAI());

                VisionSensor();
            }
        }

        private void VisionSensor()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _viewRadius, _firemanLayerMask);
            if (hitColliders.Length > 0)
            {
                foreach (var collider in hitColliders)
                {
                    RaycastHit hit;
                    Vector3 direction = collider.gameObject.transform.position - _eye.transform.position;
                    direction.y = 0;
                    if (!Physics.Raycast(_eye.transform.position, direction, out hit, _viewRadius, _wallLayerMask))
                    {
                        Target = collider.gameObject;
                        return;
                    }
                }
            }
            else
                Target = null;
        }

        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            Vector3 direction = new Vector3();
            Vector3 targetPosition = new Vector3();

            if (Target != null)
            {
                targetPosition = Target.transform.position;

                if ((Target.transform.position - transform.position).magnitude <= _attackDistance)
                    return (Vector3.zero, transform.rotation, true);
            }
            else
            {
                if (_deltaPath == null || _deltaPath.Length < 2)
                {
                    targetPosition = _initPosition;
                }
                else
                {
                    targetPosition = _initPosition + _deltaPath[_currentPoint];

                    if ((targetPosition - transform.position).magnitude <= _stepLength)
                    {
                        _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
                    }
                }
            }

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            if (path.corners.Length > 0)
            {
                foreach (var corner in path.corners)
                {
                    direction = corner - transform.position;
                    if (direction.magnitude <= _stepLength)
                        continue;
                    direction.y = 0;
                    return (direction, Quaternion.LookRotation(direction), false);
                }
            }

            return (Vector3.zero, Quaternion.identity, false);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.3f, 0.3f, 0.25f);
            Gizmos.DrawSphere(transform.position, _viewRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(_eye.transform.position, _eye.transform.forward * _viewRadius);
        }
    }
}