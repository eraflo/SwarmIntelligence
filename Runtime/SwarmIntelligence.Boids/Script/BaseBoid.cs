using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseBoid : MonoBehaviour
{
    [Header("Boid Settings")]
    public float MaxSpeed = 10.0f;
    public float Accelerator = 10.0f;
    public float BoundaryStrength = 1.0f;
    public bool UseBoundaries = true;

    [Header("Neighbourhood")]
    public float neighbourRadius = 10.0f;
    public float separationRadius = 5.0f;

    protected Vector3 _position = Vector3.zero;
    protected Vector3 _velocity = Vector3.zero;
    protected Vector3 _massCenter = Vector3.zero;

    protected Vector3[] _boundaries;

    protected Collider _collider;

    [SerializeField]
    protected List<BaseBoid> _neighbours = new List<BaseBoid>();

    protected virtual void Start()
    {
        gameObject.name = "Boid" + GetInstanceID();

        _position = transform.position;
        _velocity = Vector3.zero;

        _collider = GetComponent<Collider>();

        if(_collider == null)
        {
            Debug.LogError("Boid: No collider found!");
            Destroy(this);
            return;
        }

        _neighbours.Clear();

        if(_boundaries.Length == 0)
        {
            _boundaries = new Vector3[2];
            _boundaries[0] = new Vector3(-10, -10, -10);
            _boundaries[1] = new Vector3(10, 10, 10);
        }

        StartCoroutine(Loop());
    }

    // Display the radius of the boid in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, neighbourRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }

    #region Loop

    private IEnumerator Loop()
    {
        while (true)
        {
            yield return LoopProgress();
        }
    }

    /// <summary>
    /// Loop for the boid to calculate the next position
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator LoopProgress()
    {
        CheckNeighboor();
        CalculateCenterOfMass();

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

    #endregion



    #region Steering Behaviours

    /// <summary>
    /// The boids try to keep a small distance away from other objects
    /// </summary>
    /// <returns></returns>
    protected Vector3 Separation()
    {
        Vector3 c = Vector3.zero;

        foreach (BaseBoid boid in _neighbours)
        {
            if(boid != this)
            {
                Vector3 diff = _position - boid.Position;
                float d = Mathf.Abs(diff.magnitude);

                if(d < separationRadius)
                {
                    c = c - (boid.Position - _position);
                }
            }
        }
        return c;
    }

    /// <summary>
    /// Boids try to match velocity with near boids
    /// </summary>
    /// <returns></returns>
    protected Vector3 Alignment()
    {
        Vector3 pv = Vector3.zero;

        foreach (BaseBoid boid in _neighbours)
        {
            if(boid != this)
            {
                pv += boid.Velocity;
            }
        }

        if(_neighbours.Count > 0)
            pv /= _neighbours.Count;
        return (pv - _velocity) / 8;
    }

    /// <summary>
    /// Boids move towards the average position of their neighbours
    /// </summary>
    /// <returns></returns>
    protected Vector3 Cohesion()
    {
        Vector3 pc = Vector3.zero;

        foreach (BaseBoid boid in _neighbours)
        {
            if(boid != this)
            {
                pc += boid.Position;
            }
        }

        if(_neighbours.Count > 0)
            pc /= _neighbours.Count;

        return (pc - _position) / 100;
    }

    /// <summary>
    /// Limit the boid's velocity
    /// </summary>
    protected void LimitVelocity()
    {
        if(Mathf.Abs(_velocity.magnitude) > MaxSpeed * Accelerator)
        {
            _velocity = (_velocity / Mathf.Abs(_velocity.magnitude)) * MaxSpeed * Accelerator;
        }
    }

    /// <summary>
    /// Boundary conditions
    /// </summary>
    protected virtual Vector3 KeepInBoundary()
    {
        Vector3 v = Vector3.zero;

        if(_position.x < _boundaries[0].x)
        {
            v.x = 1;
        }
        else if(_position.x > _boundaries[1].x)
        {
            v.x = -1;
        }

        if(_position.y < _boundaries[0].y)
        {
            v.y = 1;
        }
        else if(_position.y > _boundaries[1].y)
        {
            v.y = -1;
        }

        if(_position.z < _boundaries[0].z)
        {
            v.z = 1;
        }
        else if(_position.z > _boundaries[1].z)
        {
            v.z = -1;
        }

        return v * BoundaryStrength;
    }

    #endregion

    #region Helper Methods

    public void AddNeightboor(BaseBoid boid)
    {
        _neighbours.Add(boid);
    }

    public void RemoveNeightboor(BaseBoid boid)
    {
        _neighbours.Remove(boid);
    }

    public void ClearNeightboors()
    {
        _neighbours.Clear();
    }

    #endregion

    #region Getter and Setter
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    public Vector3[] Boundaries
    {
        get { return _boundaries; }
        set { _boundaries = value; }
    }

    #endregion

    /// <summary>
    /// Calculate the center of mass of the boids neighbourhood
    /// </summary>
    protected void CalculateCenterOfMass()
    {
        _massCenter = transform.position;


        foreach (BaseBoid boid in _neighbours)
        {
            _massCenter += boid.transform.position;
        }

        _massCenter /= _neighbours.Count;
    }

    /// <summary>
    /// Check the neighbourhood of the boid
    /// </summary>
    protected void CheckNeighboor()
    {
        Collider[] colliders = Physics.OverlapSphere(_position, neighbourRadius);

        ClearNeightboors();

        foreach (Collider c in colliders)
        {
            if (c.gameObject != gameObject)
            {
                BaseBoid boid = c.GetComponent<BaseBoid>();

                if (boid != null)
                {
                    _neighbours.Add(boid);
                }
            }
        }
    }
}
