using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UILoginScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject signInScreen;
    [SerializeField]
    private GameObject nameScreen;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private GameObject playBtn;
    [SerializeField]
    private GameObject changeUserName;

    [Header("State Info")]
    [SerializeField]
    private float minSigninTime = 1.0f;
    [SerializeField]
    private float maxSigninTime = 5.0f;
    [SerializeField]
    private float curSigninTime = 0.0f;
    [SerializeField]
    private string username;
    [SerializeField]
    private bool receivedSignal = false;
    [SerializeField]
    private bool finishedWaiting = false;

    private void OnEnable()
    {
        LootLockerManager.OnSignIn += HandleSignIn;
        EvaluateState();
    }

    private void OnDisable()
    {
        LootLockerManager.OnSignIn -= HandleSignIn;
    }

    private void Start()
    {
        receivedSignal = false;
        finishedWaiting = false;
    }

    private void Update()
    {
        // Don't use timers when waiting is finished
        if (finishedWaiting)
            return;

        // While waiting for a signal, start the timer
        if (curSigninTime < maxSigninTime)
            curSigninTime += Time.deltaTime;

        // If a signal was received, wait for min time and then show the correct screen
        if(curSigninTime >= minSigninTime && receivedSignal)
        {
            // If the user has no name, enable the name screen
            if (username == "")
                EnableNameScreen();
            // If the user has a name, complete the sign in process
            else
                CompleteSignIn();
            finishedWaiting = true;
        }

        // If no signal is received by max time, finish waiting and force name screen
        if (curSigninTime >= maxSigninTime)
        {
            EnableNameScreen();
            finishedWaiting = true;
        }
    }

    // Evaluates the state of this component
    private void EvaluateState()
    {
        // If the user isn't logged in, enable sign in screen
        if (!LootLockerManager.Instance.GetIsLoggedIn())
        {
            EnableSignInScreen();
            return;
        }

        // If the player is logged in, allow them to change their username
        EnableNameScreen();
    }

    // Handles signing in.
    private void HandleSignIn(string _username)
    {
        username = _username;
        inputField.text = _username;
        receivedSignal = true;
    }

    // Enables the loading icon while LootLocker signs in
    public void EnableSignInScreen()
    {
        signInScreen.SetActive(true);
        nameScreen.SetActive(false);
    }

    // Enables the name screen for the user to enter their name
    public void EnableNameScreen()
    {
        signInScreen.SetActive(false);
        nameScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
    }

    // Completes the sign in process
    public void CompleteSignIn()
    {
        signInScreen.SetActive(false);
        nameScreen.SetActive(false);

        changeUserName.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playBtn);
        this.gameObject.SetActive(false);
    }
}
