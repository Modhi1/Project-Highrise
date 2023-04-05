using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Variables
    private int horizontal;
    private int vertical;
    #endregion

    void Update()
    {

        MoveCamera();

    }


    /// <summary>
    /// This method is called by the update method (once per frame) to ensure smooth movement of the camera 
    /// </summary>
    private void MoveCamera()
    {
        // Left arrow -> -1
        // Right arrow -> 1
        horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        // Down arrow -> -1
        // Up arrow -> 1
        vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        // 20 & -20 are the edges of the floor in x axis
        // 1 is the edge of the floor in the y axis 
        if (transform.position.x <= 20 && transform.position.x >= -20 && transform.position.y >= 1)
        {
            // based on the value of horizontal var the direction of the camera movement will change
            // Vector3.right * 1 -> move right
            // Vector3.right * -1 -> move left 
            transform.Translate(Vector3.right * horizontal * 10f * Time.deltaTime);

            // based on the value of vertical var the direction of the camera movement will change
            // Vector3.up * 1 -> move up
            // Vector3.up * -1 -> move down 
            transform.Translate(Vector3.up * vertical * 10f * Time.deltaTime);


           
        }

        // using transform.Translate to move the camera makes the camera position exceeds the max valid position
        // that's why we need to call this method (To ensure valid position)
        CheckMaxPosition();


    }

    /// <summary>
    /// This method checks the cameras valid position
    /// Camera position must be between 20 & -20 in the x axis    and   1 to infinity in the y axis
    /// </summary>
    private void CheckMaxPosition()
    {
        if (transform.position.y < 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }

        if (transform.position.x > 20)
        {
            transform.position = new Vector3(20, transform.position.y, transform.position.z);

        }

        if (transform.position.x < -20)
        {
            transform.position = new Vector3(-20, transform.position.y, transform.position.z);

        }
    }
}
