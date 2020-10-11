using UnityEngine;
using UserInput;

public class MovementScript : MonoBehaviour
{
    public UserInputSettings userInput;
    [Space(10f)]
    public float speed;
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        if (Input.GetKey(userInput.up))
        {
            transform.position += Vector3.up.normalized * speed  * Time.fixedDeltaTime;
        }
        if (Input.GetKey(userInput.down))
        {
            transform.position += Vector3.down.normalized * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(userInput.right))
        {
            transform.position += Vector3.right.normalized * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(userInput.left))
        {
            transform.position += Vector3.left.normalized * speed * Time.fixedDeltaTime;
        }    
    }
}
