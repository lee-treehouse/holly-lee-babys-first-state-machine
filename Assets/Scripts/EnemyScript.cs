using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform _player;
    public float _moveSpeed = 4;
    public float _aggroRadius = 5;
    public float _returnToOriginRadius = 15;

    public Material _aggroMat;
    public Material _neutralMat;
    public Material _returningMat;

    private float _distanceToPlayer;
    private Renderer _renderer;
    private Vector3 _originPos;//Position where the enemy started

    private enum EnemyState
    {
        Idle,
        Aggro,
        ReturnToOrigin,
        Melee
    }

    [SerializeField]
    private EnemyState _curState;


    void Update()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if(_curState == EnemyState.Idle)
            UpdateIdle();
        else if(_curState == EnemyState.Aggro)
            UpdateAggro();
        else if(_curState == EnemyState.ReturnToOrigin)
            UpdateReturnToOrigin();
    }

    private void UpdateIdle()
    {
        _renderer.material = _neutralMat;

        if (_distanceToPlayer <= _aggroRadius)
            _curState = EnemyState.Aggro;
    }

    private void UpdateAggro()
    {
        _renderer.material = _aggroMat;
        MoveToPlayer();


        if (_distanceToPlayer >= _returnToOriginRadius)
            _curState = EnemyState.ReturnToOrigin;
    }

    private void UpdateReturnToOrigin()
    {
        _renderer.material = _returningMat;
        MoveToOriginPos();

        float distanceToOrigin = Vector3.Distance(_originPos, transform.position);
        if (distanceToOrigin <= 0.05f)
            _curState = EnemyState.Idle;
    }

    void MoveToOriginPos()
    {
        transform.position += (_originPos - transform.position).normalized * Time.deltaTime * _moveSpeed;
    }

    void MoveToPlayer()
    {
        transform.position += (_player.position - transform.position).normalized * Time.deltaTime * _moveSpeed;
    }

    #region Offtopic & Init Stuff

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originPos = transform.position;
        _curState = EnemyState.Idle;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, _aggroRadius);
        Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.1f);
        Gizmos.DrawSphere(transform.position, _returnToOriginRadius);

    }

    #endregion
}
