using System;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    #region Variables

    public static event Action objectPlaced;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform[] raypoints;
    private Camera camera;
    private bool isPlaced;
    private bool isOverLapping;
    private Vector3 mousePosition;
    private Vector3 targetPosition;

    #endregion

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
     
        if (isPlaced)
            return;


        MouseMovement();

        PlaceRoom();

    }

    /// <summary>
    /// This method will be called when the room overlaps with another room
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // the if condition -> so the already placed room is not affected by the collision 
        if (isPlaced)
            return;
        indicator.SetActive(true);
        isOverLapping = true;
    }

    /// <summary>
    /// This method will be called when the overlapping of the room with another room is not there (exit overlapping)
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        isOverLapping = false;
    }

    /// <summary>
    /// This method is called in the Update method (once per frame) to continuously track the mouse movement (which is reflected on the room movement)
    // This method tracks the mouse position and assign it to the object position
    /// </summary>
    private void MouseMovement()
    {

        mousePosition = Input.mousePosition;

        // since the camera is 10f point far
        mousePosition.z = 10f;

        // change from screen point to world point
        targetPosition = camera.ScreenToWorldPoint(mousePosition);


        targetPosition.x = Mathf.RoundToInt(targetPosition.x);
        targetPosition.y = Mathf.RoundToInt(Mathf.Clamp(targetPosition.y, 0, int.MaxValue));

        transform.position = targetPosition;
    }

    /// <summary>
    /// This method is called in the Update method (once per frame) to continuously check if the user placed a room or not 
    // This method assign the room to a static position (if it's valid & the mouse is clicked)
    /// </summary>
    private void PlaceRoom()
    {
        // if it's a valid position and the user clicked the mouse button -> place the room
        if (CheckValid() && Input.GetMouseButtonDown(0))
        {
            isPlaced = true;
            objectPlaced?.Invoke();
        }
    }

    /// <summary>
    /// This method checks if the current placement of the room is valid or not (based on whether there's a room or floor under it or not)
    /// </summary>
    private bool CheckValid()
    {

        // if the room is overlapping with another room -> not a valid position
        if (isOverLapping)
            return false;


        // loop on rayPoints to check if there's an object (room or floor) under them (hence a valid position)
        for (int i = 0; i < raypoints.Length; i++)
        {
            Ray ray = new Ray(raypoints[i].position, Vector3.down);
            // if no hit -> placement is not valid 
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, 1.5f))
            {
                // not valid position
                indicator.SetActive(true);
                return false;

            }
        }

        // valid  position 
        indicator.SetActive(false);
        return true;

    }
}
