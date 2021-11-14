using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ZombieComponent))]
public class ZombieBotInput : PlayerInput
{
    [SerializeField] private GameObject _eye;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private ZombieComponent _zombieComponent;
    [SerializeField] private LayerMask _firemanLayerMask;
    [SerializeField] private LayerMask _zombieLayerMask;
    [SerializeField] private Vector3[] _deltaPath;
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _screamRadius;
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
        var sphereCastAll = Physics.SphereCastAll(_eye.transform.position, 
            _viewRadius, _eye.transform.forward, _viewRadius, _firemanLayerMask);
        if (sphereCastAll.Length > _stepLength)
        {
            Target = sphereCastAll[0].collider.gameObject;
            Scream();
        }
        else
            Target = null;
    }

    private void Scream()
    {
        var sphereCastAll = Physics.SphereCastAll(_eye.transform.position, 
            _screamRadius, _eye.transform.forward, _screamRadius, _zombieLayerMask);
        foreach (var zombie in sphereCastAll)
        {
            ZombieBotInput zombieBotInput = zombie.collider.gameObject.GetComponentInParent<ZombieBotInput>();
            if (zombieBotInput)
                zombieBotInput.Target = Target;
        }
    }

    public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
    {
        Vector3 direction = new Vector3();
        Vector3 targetPosition = new Vector3();
        bool needAttack = false;
        
        if (Target != null)
        {
            targetPosition = Target.transform.position;
            needAttack = (Target.transform.position - transform.position).magnitude <= _attackDistance;
        }
        else
        {
            if (_deltaPath == null || _deltaPath.Length < 2)
                return (Vector3.zero, Quaternion.identity, false);
            
            targetPosition = _initPosition + _deltaPath[_currentPoint];
            
            if ((targetPosition - transform.position).magnitude <= _stepLength)
            {
                _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
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

                return (direction, Quaternion.LookRotation(direction), needAttack);
            }
        }
        
        return (Vector3.zero, Quaternion.identity, false);
    }
}
