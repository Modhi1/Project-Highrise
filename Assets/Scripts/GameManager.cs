using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager instance;

    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject buttonsMenu;
    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject happinessText;
    [SerializeField] private GameObject noMoneyPopUp;


    private List<Room> createdRooms;
    // timer for updating info 
    private float timer = 5;
    // money at the start
    private float money = 1000;
    // happiness at the start
    private float happiness = 10;

    public float Money => money;
    #endregion

    private void Awake()
    {
        // singleton
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }


    void Start()
    {
        // subscribe to event 
        GenerateRoom.objectCreated += MenuStatus;
        GenerateRoom.noEnoughMoney += PopUpDisplayment;
        createdRooms = new List<Room>();


    }

    private void Update()
    {
       // decrease timer 
        timer -= Time.deltaTime;

        // if true -> update info & reset timer
        if (timer <= 0)
        {
            UpdateInfo();
            UpdateTexts();
            timer = 5;
        }
    }

    /// <summary>
    /// This method is resposiable of displayment of the menu button and the buttons menu
    /// This method is subscribed to objectCreated event also it gets called from the menu button in the inspector
    /// </summary>
    public void MenuStatus(bool status)
    {
       
        buttonsMenu.SetActive(status);
        menuButton.SetActive(!status);

    }

    /// <summary>
    /// This method is called once the room is instantiated
    /// </summary>
    public void AddRoom(GameObject room)
    {
        createdRooms.Add(room.GetComponent<Room>());
        // everytime you add a room, you must update the info
        // the price of the room reflects on the money left
        SubtractPrice();
    }

    /// <summary>
    /// This method is called once the room is added to the list
    /// It deletes the price of each room from the money
    /// </summary>
    private void SubtractPrice()
    {
        float startMoney = money;
        // subtract the price of the last added room in the list from the money
        money = money - createdRooms[createdRooms.Count - 1].Price;

        UpdateHappiness(startMoney);

        UpdateTexts();
    }


    // Called based on timer
    /// <summary>
    /// every 5 seconds add the cost and the income 
    /// </summary>
    private void UpdateInfo()
    {
        // loop over all the rooms
        foreach (Room room in createdRooms)
        {
            float startMoney = money;

            // update the money 
            money += room.Income;
            money -= room.Cost;


            UpdateHappiness(startMoney);

        }

        
    }

    /// <summary>
    /// This method is called once the money is updated by eaither (SubtractPrice or UpdateInfo)
    /// It takes the startMoney money as parameter to calculate the growth or decline in the total money
    /// To update the happiness we multiply the happiness by the growth or decline of the money so that the happiness value is dependent on the money 
    /// </summary>
    private void UpdateHappiness(float startMoney)
    {
        // if the money is more by 20% the happiness also will be more by 20%
        // if the subtraction is - the growthOrDecline is - 
        float growthOrDecline = ((money - startMoney) / startMoney);

        happiness += Mathf.RoundToInt(happiness * growthOrDecline);

        // if true -> reset the happiness to be 1 (because 0 is not valid since we calculate happiness by multiplying it by growthOrDecline)
        if (happiness <= 0)
        {
            happiness = 1;
            return;
        }
    }

    /// <summary>
    /// This method is called whenever the money and happiness is updated 
    /// </summary>
    private void UpdateTexts()
    {

        moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString() + "$";
        happinessText.GetComponent<TextMeshProUGUI>().text = happiness.ToString();


    }

    /// <summary>
    /// This method is called when an event occur (money is not enough to buy a room)
    /// It's responsiable of the displayment of the pop up message and the status of the button menu 
    /// </summary>
    public void PopUpDisplayment(bool status)
    {
        noMoneyPopUp.SetActive(status);
        buttonsMenu.SetActive(false);
        menuButton.SetActive(!status);

    }
}
