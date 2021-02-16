using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Handler : MonoBehaviour{

    [SerializeField] TextMeshProUGUI bladeText = null;
    [SerializeField] TextMeshProUGUI questionText = null;
    [SerializeField] UnityEngine.UI.Button[] buttons = null;

    GameHandler gameHandler;
    Content[] contentPool;
    UnityEngine.UI.ColorBlock theColor;
    Color originalColor;

    public bool canLoadQuestion, interactableButtons;
    bool answered, answeredTrue;
    int answer, givenAnswer, bladeNumber;

    void Start()    {
        // Reset all paramaters
        bladeText.SetText("Blade: 0"); // Start with zero blade   
        answered = false;
        answeredTrue = true; // Get new question
        StartCoroutine(SetNewQuestion(answeredTrue));

        givenAnswer = 5;
        bladeNumber = 0;
        answer = Random.Range(0, 3);

        originalColor = buttons[0].GetComponent<Image>().color;

        gameHandler = FindObjectOfType<GameHandler>();
    }


    void Update()    {
        if (interactableButtons) {
            foreach (Button button in buttons) {
                button.interactable = true;
            }
        }
        else {
            foreach (Button button in buttons) {
                button.interactable = false;
            }
        }


        if (answered && canLoadQuestion) {
            //  if player know the question, give them reward and color the buttons
            if (givenAnswer == answer) {    
                bladeNumber++;  // Increase blade number
                bladeText.SetText("Blade: " + bladeNumber.ToString());  // Update GUI
                gameHandler.SetBladeNumber(bladeNumber);   // Update blade number on GM 

                buttons[answer].GetComponent<Image>().color = Color.green;
                FindObjectOfType<AudioManager>().Play("TrueAnswer");

                answeredTrue = true;
                givenAnswer = 5;    // Reset givenAnswer
            }
            else {
                FindObjectOfType<AudioManager>().Play("WrongAnswer");
                buttons[answer].GetComponent<Image>().color = Color.green;
                buttons[givenAnswer].GetComponent<Image>().color = Color.red;
                answeredTrue = false;
            }
            
            StartCoroutine(SetNewQuestion(answeredTrue));   //Set new answer
            answered = false;   // Reset answered
        }

        bladeText.SetText("Blade: " + bladeNumber);   // Update blade number
    }

    private IEnumerator SetNewQuestion(bool answeredTrue) {
        canLoadQuestion = false; // Stop taking new questions (used this against buttons)

        if (answeredTrue)
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(2f);


        contentPool = gameHandler.GetQuestionContent(); // Get updated content pool from Game Handler

        foreach (UnityEngine.UI.Button button in buttons) {  // Set all colors back
            button.GetComponent<Image>().color = originalColor;
        }

        answer = Random.Range(0, 3);        // Gettin new position for the true answer
        int randomQuestionIndex;            // Used for avoid multiple true answer

        Content questionToAsk = contentPool[Random.Range(0, contentPool.Length)];   //  Select new question
        questionText.SetText(questionToAsk.japaneseContent);                        //  Add it's Hiragana to the question Text
        buttons[answer].GetComponentInChildren<TextMeshProUGUI>().SetText(questionToAsk.englishContent);                  //  Add it's answer to the right place
        for (int i = 0; i < buttons.Length; i++) {                              //  Add random other 3 answers
            string typeOfAnswer = "";
            if (i != answer) {  // skip the true button cuz we already put the true answer in it.
                do {        // I'll pick a number from the pool to get random answer
                    randomQuestionIndex = Random.Range(0, contentPool.Length);
                    typeOfAnswer = contentPool[randomQuestionIndex].contentType;
                }
                while (randomQuestionIndex == questionToAsk.contentID || typeOfAnswer != questionToAsk.contentType);  // But if that number is equal my true number, then do it again

                buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(contentPool[randomQuestionIndex].englishContent); // if not put it into button
            }
                
        }

        canLoadQuestion = true;
    }

    private UnityEngine.UI.ColorBlock GetColor(bool positiveColor) {
        if (positiveColor) {
            theColor.normalColor = theColor.pressedColor = 
                theColor.selectedColor = theColor.highlightedColor = Color.green;
        }
        else {
            theColor.normalColor = theColor.pressedColor =
                theColor.selectedColor = theColor.highlightedColor = Color.red;
        }

        return theColor;
    }

    // Getting answer
    public void SetAnswer(int givenAnswer) {
        if (!canLoadQuestion) { return; } // if waiting for color, then don't get answer
        answered = true;
        this.givenAnswer = givenAnswer;
    }

    public void SetBladeNumber(int bladeNumber) {
        this.bladeNumber = bladeNumber;
    }
    public void SetLoadQuestion(bool canLoadQuestion) {
        this.canLoadQuestion = canLoadQuestion;
    }

}
