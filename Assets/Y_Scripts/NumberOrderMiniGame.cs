using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberOrderMiniGame : MonoBehaviour
{
    [SerializeField] int nextButton;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject[] myObjects;

    int randNum;
    //Use this to change the hierarchy of the GameObject siblings
    void Start()
    {
        nextButton = 0;
    }
    private void OnEnable()  //Gets called when the game object this script is attached to becomes active
    {
        nextButton = 0;
        for (int i = 0; i < myObjects.Length; i++)
        {
            myObjects[i].transform.SetSiblingIndex(Random.Range(0, 9));
            //transform.SetSiblingIndex(Random.Range(0, 9)); 
        }
    }

    public void ButtonOrder(int button)
    {
        Debug.Log("Pressed");
        if (button == nextButton)
        {
            nextButton++;
            Debug.Log("Next Button" + nextButton);
        }
        else
        {
            Debug.Log("Failed");
            Debug.Log("Next Button" + nextButton);
            nextButton = 0;
            OnEnable();
        }
        if (button == 9 && nextButton == 10)
        {
            Debug.Log("Pass");
            nextButton = 0;
            ButtonOrderPanelClose();
        }
    }
    public void ButtonOrderPanelClose() //Set this function to the close button On click in the inspector
    {
        GamePanel.SetActive(false);
    }
    public void ButtonOrderPanelOpen() //Set this function to the Play or Open button On click in the inspector
    {
        GamePanel.SetActive(true);
    }
}