using UnityEngine;

namespace Game
{
    public class ZombieComponent : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private GameObject _aliveView;
        [SerializeField] private GameObject _diedView;
        [SerializeField] private PlayerInput _zombieBotInput;
        
        private Rigidbody _rigidbody;
        private float _attackTimer;
        
        public bool CanAttack => _attackTimer <= 0f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _attackTimer = _gameSettings.GetZombieAttackTime();
        }

        private void OnEnable()
        {
            SetState(true);
        }

        private void Update()
        {
            if (!IsAlive)
                return;
            
            var (moveDirection, viewDirection, attack) = _zombieBotInput.CurrentInput();
            ProcessAttack(attack);
            _rigidbody.velocity = moveDirection.normalized * _gameSettings.GetZombieSpeed();
            transform.rotation = viewDirection;
        }

        private void ProcessAttack(bool isAttack)
        {
            _attackTimer -= Time.deltaTime;

            if (isAttack && CanAttack)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _player.Hitpoints -= _gameSettings.GetZombieDamage();
            _attackTimer = _gameSettings.GetZombieAttackTime();
        }
        
        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);
        }
        

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}