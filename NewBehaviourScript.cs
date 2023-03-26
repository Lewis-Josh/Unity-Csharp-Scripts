using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    //Public Variables/Definitions
    public float movespeed = 000.0f;
    public float jumpForce;
    public float delayTime = .5f;
    private Rigidbody rb;
    public Camera Cameramain;
    public float sphereRadius = .65f;
    public LayerMask whatIsGround;

    public TimerController timer;

    bool isGrounded = true;

    //Input Bools
    bool forward;
    bool back;
    bool right;
    bool left;

    void Start()
    {
        //Rigidbody GetComponent
        rb = GetComponent<Rigidbody>();

        //Cursor LockState
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Player Input
        if (Input.GetButton("Forward"))
        {
            forward = true;
        }

        else if (Input.GetButton("Back"))
        {
            back = true;
        }

        if (Input.GetButton("Right"))
        {
            right = true;
        }

        if (Input.GetButton("Left"))
        {
            left = true;
        }
    }

    void FixedUpdate()
    {
        //Camera Relative Movement
        if (forward == true)
        {
            forward = false;
            var camDir = Cameramain.transform.TransformDirection(Vector3.forward);
            camDir.y = 0.0f;
            rb.AddForce(camDir.normalized * movespeed * Time.fixedDeltaTime);
        }

        else if (back == true)
        {
            back = false;
            var camDir = Cameramain.transform.TransformDirection(-Vector3.forward);
            camDir.y = 0.0f;
            rb.AddForce(camDir.normalized * movespeed * 1.25f * Time.fixedDeltaTime);
        }

        if (right == true)
        {
            right = false;
            var camDir = Cameramain.transform.TransformDirection(Vector3.right);
            camDir.y = 0.0f;
            rb.AddForce(camDir.normalized * movespeed * Time.fixedDeltaTime);
        }

        else if (left == true)
        {
            left = false;
            var camDir = Cameramain.transform.TransformDirection(Vector3.left);
            camDir.y = 0.0f;
            rb.AddForce(camDir.normalized * movespeed * Time.fixedDeltaTime);
        }

        //Spherecast for isGrounded
        RaycastHit hit;
        if (Physics.SphereCast(rb.transform.position + rb.GetComponent<SphereCollider>().center, sphereRadius, Vector3.down, out hit, .05f, whatIsGround, QueryTriggerInteraction.UseGlobal))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector3.up.normalized * jumpForce, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //OnTrigger Win/Lose Condition
        if (collider.gameObject.CompareTag("Lose"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        else if (collider.gameObject.CompareTag("Finish"))
        {
            timer.EndTimer();
            Invoke("DelayedAction", delayTime);
        }
    }

    void DelayedAction()
    {
        //On Win Scene Load
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnDrawGizmosSelected()
    {
        //Debug Shit
        rb = GetComponent<Rigidbody>();
        Gizmos.color = Color.red;
        Debug.DrawLine(rb.transform.position + rb.GetComponent<SphereCollider>().center, rb.transform.position + rb.GetComponent<SphereCollider>().center + Vector3.down * 1.0f);
        Gizmos.DrawWireSphere(rb.transform.position + rb.GetComponent<SphereCollider>().center + Vector3.down * 1.0f, sphereRadius);
    }
}