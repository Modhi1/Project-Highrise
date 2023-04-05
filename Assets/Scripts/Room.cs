using UnityEngine;

public enum RoomType
{
 Office,
 Bathroom,
 Gym,
 Restaurant
}


public abstract class Room : MonoBehaviour
{
    #region Variables

    [SerializeField] private RoomType type;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightDoorWall;
    [SerializeField] private GameObject leftDoorWall;

    [SerializeField] private int price;
    [SerializeField] private int income;
    [SerializeField] private int cost;

    // getters & setters
    public RoomType Type => type;

    public int Price
    {
        get => price;
        
        set
        {
            price = value;
        }
    }

    public int Income
    {
        get => income;

        set
        {
            income = value;
        }
    }

    public int Cost
    {
        get => cost;

        set
        {
            cost = value;
        }
    }

    #endregion


    private void Start()
    {
        // subscribe to event
        ObjectMovement.objectPlaced += CheckWalls;
    }


    /// <summary>
    /// This method checks the left and right side of the placed room to activate or deactivate certain walls based on the type of the next room 
    /// </summary>
    private void CheckWalls()
    {
        // For left wall
        Ray leftRay = new Ray(leftWall.transform.position, Vector3.left);
        // check if there's a room in the left by doing a raycast
        if (Physics.Raycast(leftRay, out RaycastHit leftHitInfo,1f))
        {
            // deactivate left wall
            leftWall.SetActive(false);
            // if the next wall is of a different type ->  activate the left wall with the door
            if (leftHitInfo.collider.GetComponent<Room>().Type != type)
            {
                leftDoorWall.SetActive(true);
            }
        
        }


        // For right wall
        Ray rightRay = new Ray(rightWall.transform.position, Vector3.right);
        // check if there's a room in the right by doing a raycast
        if (Physics.Raycast(rightRay, out RaycastHit rightHitInfo, 1f))
        {
            // deactivate right wall
            rightWall.SetActive(false);
            // if the next wall is of a different type ->  activate the right wall with the door
            if (rightHitInfo.collider.GetComponent<Room>().Type != type)
            {
                rightDoorWall.SetActive(true);
            }

        }


    }



}
