using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Handler : MonoBehaviour{

    [SerializeField] TextMeshProUGUI energyText = null;
    [SerializeField] TextMeshProUGUI questionText = null;
    [SerializeField] UnityEngine.UI.Button[] buttons = null;

    GameHandler gameHandler;
    Content[] contentPool;
    UnityEngine.UI.ColorBlock theColor;
    Color originalColor;

    public bool canLoadQuestion, interactableButtons;
    bool answered, answeredTrue;
    int answer, givenAnswer, energyValue;

    void Start()    {
        // Reset all paramaters
        energyText.SetText("Blade: 0"); // Start with zero energy   
        answered = false;
        answeredTrue = true; // Get new question
        StartCoroutine(SetNewQuestion(answeredTrue));

        givenAnswer = 5;
        energyValue = 0;
        answer = Random.Range(0, 3);

        originalColor = buttons[0].GetComponent<Image>().color;

        gameHandler = FindObjectOfType<GameHandler>();
        contentPool = gameHandler.GetQuestionContent();
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
                energyValue++;  // Increase Energy
                energyText.SetText("Blade: " + energyValue.ToString());  // Update GUI
                gameHandler.SetBladeNumber(energyValue);   // Update energy value on GM 

                buttons[answer].GetComponent<Image>().color = Color.green;

                answeredTrue = true;
                givenAnswer = 5;    // Reset givenAnswer
            }
            else {
                buttons[answer].GetComponent<Image>().color = Color.green;
                buttons[givenAnswer].GetComponent<Image>().color = Color.red;
                answeredTrue = false;
            }
            
            StartCoroutine(SetNewQuestion(answeredTrue));   //Set new answer
            answered = false;   // Reset answered
        }

        energyText.SetText("Blade: " + energyValue);   // Update Energy Value
    }

    private IEnumerator SetNewQuestion(bool answeredTrue) {
        canLoadQuestion = false; // Stop taking new questions (used this against buttons)

        if (answeredTrue)
            yield return new WaitForSeconds(0.2f);
        else
            yield return new WaitForSeconds(1.5f);

        foreach (UnityEngine.UI.Button button in buttons) {  // Set all colors back
            button.GetComponent<Image>().color = originalColor;
        }

        answer = Random.Range(0, 3);        // Gettin new position for the true answer
        int randomQuestionIndex;            // Used for avoid multiple true answer

        Content questionToAsk = contentPool[Random.Range(0, contentPool.Length)];   //  Select new question
        questionText.SetText(questionToAsk.japaneseContent);                        //  Add it's Hiragana to the question Text
        buttons[answer].GetComponentInChildren<TextMeshProUGUI>().SetText(questionToAsk.englishContent);                  //  Add it's answer to the right place
        for (int i = 0; i < buttons.Length; i++) {                              //  Add random other 3 answers
            if (i != answer) {  // skip the true button cuz we already put the true answer in it.
                do {        // I'll pick a number from the pool to get random answer
                    randomQuestionIndex = Random.Range(0, contentPool.Length);
                }
                while (randomQuestionIndex == questionToAsk.contentID);  // But if that number is equal my true number, then do it again

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

    public void SetBladeNumber(int energyValue) {
        this.energyValue = energyValue;
    }
    public void SetLoadQuestion(bool canLoadQuestion) {
        this.canLoadQuestion = canLoadQuestion;
    }

}
