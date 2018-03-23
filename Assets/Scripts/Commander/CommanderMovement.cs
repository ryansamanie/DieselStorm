using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CommanderMovement : MovementBehaviour {

    Rigidbody m_body;
    

    public float m_turnForce;
    public float m_liftForce;
    public float m_moveForce;

    public float m_maxHeight = 160;
    public float m_minHeight = 35;

	// Use this for initialization
	void Start ()
    {
        m_body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Keep this thing upright no matter what
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }

    private void FixedUpdate()
    {
        Limit();
    }

    public override void Move(float m_hinput, float m_vinput, float m_jinput)
    {
        float aero = Vector3.Dot(transform.forward, m_body.velocity.normalized);

        Vector3 movevec = new Vector3(m_hinput * m_moveForce * m_body.angularVelocity.magnitude * 2, 
            m_jinput * m_liftForce, m_vinput * m_moveForce);
        Vector3 turnvec = new Vector3(0, m_hinput * aero * m_body.velocity.magnitude * m_turnForce, 0);
        m_body.AddRelativeForce(movevec);
        m_body.AddRelativeTorque(turnvec);
    }

    void Limit()
    {
        //Checks if the ship is below or above limits
        if (m_body.position.y < m_minHeight)
            m_body.AddForce(0, 30, 0);
        else if (m_body.position.y > m_maxHeight)
            m_body.AddForce(0, -30, 0);
    }
}
