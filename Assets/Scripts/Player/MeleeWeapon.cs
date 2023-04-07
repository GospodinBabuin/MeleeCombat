using System.Collections.Generic;
using System;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private int _weaponDamage = 1;
    [SerializeField] private AudioSource _attackAudio;


    [System.Serializable]
    private class AttackPoint
    {
        public float radius;
        public Vector3 offset;
        public Transform attackRoot;

#if UNITY_EDITOR
        [NonSerialized] public List<Vector3> previousPositions = new List<Vector3>();
#endif
    }

    [SerializeField] private ParticleSystem _hitParticlePrefab;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private AttackPoint[] _attackPoints = new AttackPoint[0];

    [SerializeField] private GameObject _owner;
    [SerializeField] private Vector3[] _previousPosition = null;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _inAttack;
    [SerializeField] private const int _particleCount = 10;
    [SerializeField] private ParticleSystem[] _particlesPool = new ParticleSystem[_particleCount];
    [SerializeField] private int _currentParticle = 0;
    [SerializeField] private static RaycastHit[] _raycastHitCash = new RaycastHit[32];
    [SerializeField] private static Collider[] _colliderCash = new Collider[32];


    private void Awake()
    {
        if (_hitParticlePrefab != null)
        {
            for (int i = 0; i < _particleCount; i++)
            {
                _particlesPool[i] = Instantiate(_hitParticlePrefab);
                _particlesPool[i].Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_inAttack)
        {
            for (int i = 0; i < _attackPoints.Length; i++)
            {
                AttackPoint pts = _attackPoints[i];

                Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                Vector3 attackVector = worldPos - _previousPosition[i];

                if (attackVector.magnitude < 0.001f)
                {
                    attackVector = Vector3.forward * 0.0001f;
                }

                Ray ray = new Ray(worldPos, attackVector.normalized);
                int contacts = Physics.SphereCastNonAlloc(ray, pts.radius, _raycastHitCash, attackVector.magnitude,
                    ~0, QueryTriggerInteraction.Ignore);

                for (int k = 0; k < contacts; k++)
                {
                    Collider collider = _raycastHitCash[k].collider;

                    if (collider != null)
                    {
                        CheckDamage(collider);
                    }

                    _previousPosition[i] = worldPos;

#if UNITY_EDITOR
                    pts.previousPositions.Add(_previousPosition[i]);
#endif
                }
            }
        }
    }

    private void CheckDamage(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();

        if (damageable == null)
        {
            return;
        }

        if (damageable.gameObject == _owner)
        {
            return;
        }

        /*if (hitAudio != null)
        {
            var renderer = other.GetComponent<Renderer>();
            if (!renderer)
                renderer = other.GetComponentInChildren<Renderer>();
            if (renderer)
                hitAudio.PlayRandomClip(renderer.sharedMaterial);
            else
                hitAudio.PlayRandomClip();
        }*/

        damageable.ApplyDamage(_weaponDamage);

        //hitParticles

    }

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    public void BeginAttack()
    {
        if (_attackAudio != null)
        {
            _attackAudio.Play();
        }

        _inAttack = true;

        _previousPosition = new Vector3[_attackPoints.Length];

        for (int i = 0; i < _attackPoints.Length; i++)
        {
            Vector3 worldPos = _attackPoints[i].attackRoot.position +
                _attackPoints[i].attackRoot.TransformVector(_attackPoints[i].offset);
            _previousPosition[i] = worldPos;

#if UNITY_EDITOR
            _attackPoints[i].previousPositions.Clear();
            _attackPoints[i].previousPositions.Add(_previousPosition[i]);
#endif
        }
    }

    public void EndAttack()
    {
        _inAttack = false;

#if UNITY_EDITOR
        for (int i = 0; i < _attackPoints.Length; ++i)
        {
            _attackPoints[i].previousPositions.Clear();
        }
#endif
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _attackPoints.Length; ++i)
        {
            AttackPoint pts = _attackPoints[i];

            if (pts.attackRoot != null)
            {
                Vector3 worldPos = pts.attackRoot.TransformVector(pts.offset);
                Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                Gizmos.DrawSphere(pts.attackRoot.position + worldPos, pts.radius);
            }

            if (pts.previousPositions.Count > 1)
            {
                UnityEditor.Handles.DrawAAPolyLine(10, pts.previousPositions.ToArray());
            }
        }
    }

#endif
}
