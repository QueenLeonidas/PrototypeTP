using System;
using UnityEngine;


[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : SubscribedBehaviour {
    [SerializeField] float extraForceForDirectionalJump = 10f;

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    private void Start() {
        // get the transform of the main camera
        m_Cam = GameManager.Instance.player.transform;

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update() {
        if(!m_Jump) {
            m_Jump = Input.GetButtonDown("Jump");
            addForwardForceWhileJumping();
        }
        if(m_Jump) {
            addForwardForceWhileJumping();
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // calculate move direction to pass to character
        if(m_Cam != null) {
            // calculate camera relative direction to move:
            m_Move = v * (transform.position - m_Cam.position) + h * Vector3.Cross(Vector3.up, (transform.position - m_Cam.position)).normalized;
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump);
        m_Jump = false;
    }

    private void addForwardForceWhileJumping() {
        Vector3 buddyLineOfSight = transform.Find("Test_Anim_idle/EyeLight").gameObject.transform.forward;
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
            transform.GetComponent<Rigidbody>().AddForce(buddyLineOfSight * extraForceForDirectionalJump);
        }
    }
}
