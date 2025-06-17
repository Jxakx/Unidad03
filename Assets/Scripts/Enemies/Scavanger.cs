using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavanger : MonoBehaviour, IDamagiable
{
    [Header("Enemigo")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damage;

    [Header("Movimiento")]
    [SerializeField] private float _speed;
    [SerializeField] private float _distAttack;
    [SerializeField] private List<Transform> _movPoints;
    private int _indexMovPoints = 0;
    private Vector3 _dir;
    private Action _currentState;
    private bool _canShase = true;

    [Header("Rotación")]
    [SerializeField] private float _speedRotation = 5f;
    [Tooltip("Ángulo en grados para corregir la orientación del modelo")]
    [SerializeField] private float offsetY = 90f;

    [Header("Referencias")]
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundDamage;
    [SerializeField] private AudioClip _soundAttack;

    [Header("Externos")]
    [SerializeField] private PlayerMovement _playerScript;
    [SerializeField] private Transform _playerTransform;
    private Vector3 _dirPlayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _origen;

    [Header("Target")]
    public Transform targetPoint;
    public float distBorrar;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _distAttackDamage;


    private void Start()
    {
        _currentHealth = _maxHealth;
        _anim.SetBool("isWalking", true);
        _dir = (_movPoints[_indexMovPoints].transform.position - transform.position).normalized;
        _currentState = WalkingArround;
    }

    private void Update()
    {
        distBorrar = Vector3.Distance(transform.position, _playerScript.transform.position);
        _currentState();

        _dirPlayer = (_playerTransform.position - _origen.position).normalized;

        Debug.DrawLine(_origen.position, _origen.position + _dirPlayer * _distAttack);

        if (Physics.Raycast(transform.position, _dirPlayer, out RaycastHit hit, _distAttack, _playerLayer))
        {
            if (hit.transform.gameObject.layer == _playerTransform.gameObject.layer)
            {
                
                //El player esta en el rango de ataque
                if (Vector3.Distance(transform.position, _playerScript.transform.position) <= _distAttackDamage)
                {
                    //Lo alcance, debo atacar
                    if (_canAttack)
                    {
                        ResetAnimatorParameters();
                        _anim.SetBool("isAttacking", true);
                        _currentState = LookToAttack;
                        _canShase = true;
                        _canAttack = false;
                    }
                }
                else
                {
                    //El player esta en el rango de vision del enemigo y lo estoy persiguiendo
                    if (_canShase && !_playerScript.IsInvisible)
                    {
                        ResetAnimatorParameters();
                        _anim.SetBool("isRunning", true);
                        _currentState = ShasePlayer;
                        _canShase = false;
                        _canAttack = true;
                    }
                    else if (_playerScript.IsInvisible)
                    {
                        ResetAnimatorParameters();
                        _anim.SetBool("isWalking", true);
                        _dir = (_movPoints[_indexMovPoints].transform.position - transform.position).normalized;
                        _currentState = WalkingArround;
                        _canShase = true;
                        _canAttack = true;
                    }
                }
            }
        } 
        else
        {
            //Salio del rango de vision
            if (!_canShase)
            {
                ResetAnimatorParameters();
                _anim.SetBool("isWalking", true);
                _dir = (_movPoints[_indexMovPoints].transform.position - transform.position).normalized;
                _currentState = WalkingArround;
                _canShase = true;
                _canAttack = true;
            }
        }
    }

    
    private void LookToAttack()
    {
        GirarHacia(_playerTransform.position, 1.5f);
    }

    public void Attack()
    {
        print("Hice daño");
        _audioSource.PlayOneShot(_soundAttack);
        if (Vector3.Distance(transform.position, _playerScript.transform.position) <= _distAttackDamage)
        {
            _playerScript.Damage(_damage);
        }
    }

    private void Idle()
    {
        //Lo que sea que haga en idle
    }

    private void WalkingArround()
    {
        //Aqui hace la ronda entre los puntos
        transform.position += _dir * _speed * Time.deltaTime;
        GirarHacia(_movPoints[_indexMovPoints].transform.position);

        Debug.DrawLine(_origen.position, _origen.position + _dir * 6);

        if (Vector3.Distance(transform.position, _movPoints[_indexMovPoints].transform.position) < 1f)
        {
            //Llego al punto
            if (_indexMovPoints == _movPoints.Count - 1)
            {
                _indexMovPoints = 0;
            }
            else
            {
                _indexMovPoints++;
            }

            _dir = (_movPoints[_indexMovPoints].transform.position - transform.position).normalized;


            ResetAnimatorParameters();
            _anim.SetBool("isIdle", true);
            StartCoroutine(TimerIdle());
            _currentState = Idle;
        }
    }

    private void ShasePlayer()
    {
        transform.position += _dirPlayer * _speed * 1.5f * Time.deltaTime;
        GirarHacia(_playerTransform.position, 1.5f);

        if (Vector3.Distance(transform.position, _playerTransform.position) < 1f)
        {
            //Llego al punto, ahora debe atacar

            //ResetAnimatorParameters();
            //_anim.SetBool("isIdle", true);
            //_currentState = Attack;
        }
    }

    
    private void GirarHacia(Vector3 target)
    {
        Vector3 direccion = (target - transform.position);
        direccion.y = 0;
        if (direccion.sqrMagnitude < 0.001f) return;

        Quaternion rotDeseada = Quaternion.LookRotation(direccion.normalized, Vector3.up);
        Quaternion rotCorregida = rotDeseada * Quaternion.Euler(0, offsetY, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotCorregida,
                                              _speedRotation * Time.deltaTime);
    }

    private void GirarHacia(Vector3 target, float moreSpeed)
    {
        Vector3 direccion = (target - transform.position);
        direccion.y = 0;
        if (direccion.sqrMagnitude < 0.001f) return;

        Quaternion rotDeseada = Quaternion.LookRotation(direccion.normalized, Vector3.up);
        Quaternion rotCorregida = rotDeseada * Quaternion.Euler(0, offsetY, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotCorregida,
                                              _speedRotation * moreSpeed * Time.deltaTime);
    }

    private void ResetAnimatorParameters()
    {
        _anim.SetBool("isAttacking", false);
        _anim.SetBool("isRunning", false);
        _anim.SetBool("isWalking", false);
        _anim.SetBool("isIdle", false);
    }

    private IEnumerator TimerIdle()
    {
        yield return new WaitForSeconds(2);
        ResetAnimatorParameters();
        _currentState = WalkingArround;
        _anim.SetBool("isWalking", true);
    }

    public void Health(float health)
    {
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;
        _audioSource.PlayOneShot(_soundDamage);

        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
