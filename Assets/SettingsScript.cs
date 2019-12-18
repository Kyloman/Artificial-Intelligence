using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    //ballmanager variables
    public GameObject ballManager;

    public Slider numBallsSlider;
    public Text numBallsText;

    public Slider mutationSlider;
    public Text mutationText;

    public Slider obstacleSlider;
    public Text obstacleText;

    public Toggle smartMutation;
    public Text smartMutationText;

    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;

    public GameObject settings;

    public GameObject myButton;

    GameObject myManager;

    bool inMenu = true;

    // Start is called before the first frame update
    void Start()
    {


        numBallsSlider.onValueChanged.AddListener(delegate {
            changeSlider();
        });
        mutationSlider.onValueChanged.AddListener(delegate {
            changeSlider();
        });
        obstacleSlider.onValueChanged.AddListener(delegate {
            changeSlider();
        });
        smartMutation.onValueChanged.AddListener(delegate {
            changeSlider();
        });

    }

    void changeSlider()
    {
        if(mutationSlider.value == 0)
        {
            mutationText.text = "None";
        }
        else if (mutationSlider.value < 3)
        {
            mutationText.text = "Low";
        }
        else if (mutationSlider.value < 6)
        {
            mutationText.text = "Medium";
        }
        else if (mutationSlider.value < 9)
        {
            mutationText.text = "High";
        }
        else if (mutationSlider.value < 12)
        {
            mutationText.text = "Very High";
        }
        else
        {
            mutationText.text = "Extreme";
        }

        if (obstacleSlider.value == 0)
        {
            obstacleText.text = "None";
            obstacle1.SetActive(false);
            obstacle2.SetActive(false);
            obstacle3.SetActive(false);
        }
        else if (obstacleSlider.value == 1)
        {
            obstacleText.text = "Set 1";
            obstacle1.SetActive(true);
            obstacle2.SetActive(false);
            obstacle3.SetActive(false);
        }
        else if (obstacleSlider.value == 2)
        {
            obstacleText.text = "Set 2";
            obstacle1.SetActive(false);
            obstacle2.SetActive(true);
            obstacle3.SetActive(false);
        }
        else
        {
            obstacleText.text = "Set 3";
            obstacle1.SetActive(false);
            obstacle2.SetActive(false);
            obstacle3.SetActive(true);
        }


        numBallsText.text = numBallsSlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonClicked()
    {
        if (inMenu)
        {
            myButton.GetComponent<Text>().text = "RESET";

            myManager = Instantiate(ballManager);
            myManager.GetComponent<BallManagerScript>().assignValues((int)numBallsSlider.value, mutationSlider.value);
            if (smartMutation.isOn)
                myManager.GetComponent<BallManagerScript>().intelligentMutation = true;
            else
                myManager.GetComponent<BallManagerScript>().intelligentMutation = false;

            myManager.GetComponent<BallManagerScript>().realStart();
        }
        else
        {

            myButton.GetComponent<Text>().text = "START";
            myManager.GetComponent<BallManagerScript>().resetAll();
            Destroy(myManager);
        }
        toggleMenu();    
    }

    public void toggleMenu()
    {
        if(inMenu == true)
        {
            settings.gameObject.SetActive(false);


            inMenu = false;
        }
        else
        {
            settings.gameObject.SetActive(true);


            inMenu = true;
        }
    }
}
