using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PerchingBoid : BaseBoid
{
    [Header("Perching Settings")]
    public LayerMask perchLayer;
    public float perchingTime = 5.0f;
    public float perchCooldown = 10.0f;

    private bool _perching = false;
    private bool _activateCooldown = false;
    


    protected override IEnumerator LoopProgress()
    {
        CheckNeighboor();
        CalculateCenterOfMass();

        if (_perching)
        {
            yield return Perch();
        }

        Vector3 separation = Separation();
        Vector3 alignment = Alignment();
        Vector3 cohesion = Cohesion();
        Vector3 boundary = UseBoundaries ? KeepInBoundary() : Vector3.zero;

        Vector3 acceleration = separation + alignment + cohesion + boundary;

        _velocity += acceleration * Time.deltaTime * Accelerator;

        LimitVelocity();

        _position += _velocity * Time.deltaTime;

        transform.position = _position;

        yield return null;
    }

    protected override Vector3 KeepInBoundary()
    {
        Vector3 v = base.KeepInBoundary();

        CheckGrounded();

        return v;
    }

    private IEnumerator Perch()
    {
        Position = transform.position + Vector3.up;

        yield return new WaitForSeconds(perchingTime);
        _perching = false;
        _activateCooldown = true;

        StartCoroutine(CooldownPerch());
    }

    private IEnumerator CooldownPerch()
    {
        yield return new WaitForSeconds(perchCooldown);
        _activateCooldown = false;
    }

    private void CheckGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighbourRadius);

        foreach (Collider c in colliders)
        {
            if (perchLayer == (perchLayer | (1 << c.gameObject.layer)) && !_activateCooldown)
            {
                _perching = true;
            }
        }
    } 

    
}
