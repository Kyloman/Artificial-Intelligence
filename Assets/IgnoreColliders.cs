//Attach this script to a GameObject and make sure it has a Rigidbody component
//Make a second GameObject with a Collider to test collisions on. Make sure both GameObjects are the same on the y and z axes

//This script stops collisions between two layers (in this case layers 0 and 8). Set up a new layer in the Inspector window by clicking the Layer option.
//Next click “Add Layer”. Then, assign this layer to the second GameObject.

//In Play Mode, press the left and right keys to move the Rigidbody to the left and right. If your first GameObject is in layer 0 and your second GameObject is in layer 8, the collision is ignored.


using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{
    //Set the speed number in the Inspector window
    public float m_Speed;
    Rigidbody m_Rigidbody;

    void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);
        //Fetch the Rigidbody component from the GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
        //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
        
    }

    void Update()
    {
        //Press right to move the GameObject to the right. Make sure you set the speed high in the Inspector window.
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Rigidbody.AddForce(Vector3.right * m_Speed);
        }

        //Press the left arrow key to move the GameObject to the left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Rigidbody.AddForce(Vector3.left * m_Speed);
        }
    }

    /*    //Detect when there is a collision
    void OnCollisionStay(Collision collide)
    {
    }
    */
}