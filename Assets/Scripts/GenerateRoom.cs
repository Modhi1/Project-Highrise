using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform roomsTransform;
    [SerializeField] private GameObject noMoneyPopUp;
    public static event Action<bool> objectCreated;
    public static event Action<bool> noEnoughMoney;

    #endregion

    /// <summary>
    /// This method is called when a button in the buttons menu is clicked
    /// it create a room (based on the button clicked)
    /// </summary>
    public void CreateRoom(GameObject prefab)
    {
        //must check first that there's enough money to buy a room 
        if (prefab.GetComponent<Room>().Price > GameManager.instance.Money)
        {
            // display a pop up message 
            noEnoughMoney?.Invoke(true);
            return;
        }


        // instantiate a room and make it under rooms (a child) in the hierarchy
        GameObject obj = Instantiate(prefab, roomsTransform);

        // Invoke objectCreated event (after instanitating) to close canvas & activate menu button
        objectCreated?.Invoke(false);
        // add the instantiated room to the list 
        GameManager.instance.AddRoom(obj);
    }

   
}
