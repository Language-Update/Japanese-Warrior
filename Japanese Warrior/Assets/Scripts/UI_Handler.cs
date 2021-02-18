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

    // Learning State Stuff

    [SerializeField] TextMeshProUGUI[] slots = null;
    [SerializeField] Button[] learningButtons = null;

    Content[] selectedContents = new Content[5];

    bool laydown, firstAnswer, secondAnswer, newAnswer;
    int numberOfTrueSelection, firstSelection, secondSelection, newSelection,
        firstID, secondID;
    string learningButton = "";
    Color normalColor;

    public bool _learningState, _playState;

    void Start()    {
        if (_playState) {
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
            return;
        }
        if (_learningState) {
            // Learning State
            laydown = firstAnswer = secondAnswer = newAnswer = false;
            numberOfTrueSelection = 0;
            gameHandler = FindObjectOfType<GameHandler>();

            // if it's first time to start, create data storage for button actions
            for (int i = 0; i < 10; i++) {
                learningButton = "learningButton" + i.ToString();
                if (!PlayerPrefs.HasKey(learningButton))
                    PlayerPrefs.SetInt(learningButton, 0);
            }
            // Get a normal color at the beginning
            normalColor = learningButtons[0].image.color;
        }
        
    }


    void Update()    {
        PlayState();
        LearningState();
    }

    private void PlayState() {
        if (!_playState) { return; }

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

    private void LearningState() {
        if (!_learningState) { return; }

        // if we did 5 correct answer, then get new contents and reset counter
        if (numberOfTrueSelection >= 5) {
            laydown = true;
            numberOfTrueSelection = 0;
        } else if (numberOfTrueSelection == 0) {
            foreach (Button button in learningButtons) {
                button.gameObject.SetActive(true);
            }
        }

        if (laydown)
            LaydownQuestions();

        // if we get new Answer
        if (newAnswer) {

            // Place the selection
            if (!firstAnswer) {
                firstSelection = newSelection;
                //Debug.Log("First Selection: " + firstSelection);
                learningButtons[firstSelection].image.color = Color.yellow;
                firstAnswer = true;
            }
            else {
                if (newSelection == firstSelection) { return; }
                secondSelection = newSelection;
                //Debug.Log("Second Selection: " + secondSelection);
                secondAnswer = true;
            }

            // if we get 2 answer in total
            if (secondAnswer) {
                // Get what they are
                learningButton = "learningButton" + firstSelection.ToString();
                firstID = selectedContents[PlayerPrefs.GetInt(learningButton)].contentID;
                //Debug.Log("Learning Button: " + learningButton + "   ID: " + firstID);

                learningButton = "learningButton" + secondSelection.ToString();
                secondID = selectedContents[PlayerPrefs.GetInt(learningButton)].contentID;
                //Debug.Log("Learning Button: " + learningButton + "   ID: " + secondID);

                // compare them
                if (firstID == secondID) {
                    FindObjectOfType<AudioManager>().Play("TrueAnswer");
                    StartCoroutine(LearningButtonColor(true));
                }
                else {
                    FindObjectOfType<AudioManager>().Play("WrongAnswer");
                    StartCoroutine(LearningButtonColor(false));
                }

                // Reset all selections
                firstAnswer = secondAnswer = false;
            }
            newAnswer = false; // wait for a new selection
        }
    }

    private IEnumerator LearningButtonColor(bool correctAnswer) {
        if (correctAnswer) {
            learningButtons[firstSelection].image.color = Color.green;
            learningButtons[secondSelection].image.color = Color.green;

            foreach (Button button in learningButtons) {
                button.interactable = false;
            }

            numberOfTrueSelection++;

            yield return new WaitForSeconds(1f);            

            learningButtons[firstSelection].gameObject.SetActive(false);
            learningButtons[secondSelection].gameObject.SetActive(false);

            foreach (Button button in learningButtons) {
                button.interactable = true;
            }
        }
        else {
            learningButtons[firstSelection].image.color = Color.red;
            learningButtons[secondSelection].image.color = Color.red;

            foreach (Button button in learningButtons) {
                button.interactable = false;
            }

            yield return new WaitForSeconds(2f);

            learningButtons[firstSelection].image.color = normalColor;
            learningButtons[secondSelection].image.color = normalColor;

            foreach (Button button in learningButtons) {
                button.interactable = true;
            }
        }
    }

    public void LaydownQuestions() {
        // Get content pool
        contentPool = gameHandler.GetQuestionContent();

        // Laydown 5 of them with their counterparts
        int contentOrder = 0;
        bool selectedSame;
        for (int j = 0; j < 5; j++) {
            do {
                selectedSame = false; // Reset same condition
                // Select random content from pool
                selectedContents[contentOrder] = contentPool[Random.Range(0, contentPool.Length)];

                // Check if it is different one or not
                for (int i = 0; i < contentOrder; i++) {    // check all of them except the last one
                                                            // Compare with the last one
                    if (selectedContents[i].contentID == selectedContents[contentOrder].contentID)
                        selectedSame = true;
                }
            }
            while (selectedSame);
            // if we got different, then go for the next one
            contentOrder++;
        }

        // Laydown them in the buttons but randomly
        // --- Upper part randomly
        for (int i = 0; i < selectedContents.Length; i++) {
            slots[i].SetText(selectedContents[i].japaneseContent);
            slots[i + 5].SetText(selectedContents[i].englishContent);
        }

        // --- Randomly laydown for the left side
        // Reset the ones that helps to pick random content
        int randomContentIndex; int[] selectedOnes = new int[] { 9, 9, 9, 9, 9 };
        contentOrder = 0; selectedSame = false;
        for (int i = 0; i < selectedContents.Length; i++) { // Do for 5 buttons
            do { // Pick random number (0-5) but should be different from others
                selectedSame = false;

                randomContentIndex = Random.Range(0, selectedContents.Length);

                for (int j = 0; j < selectedOnes.Length; j++) {    // check others
                    if (randomContentIndex == selectedOnes[j])
                        selectedSame = true;
                }
            }
            while (selectedSame);
            // If we get different order, then place it to the next button and save this random content number
            selectedOnes[i] = randomContentIndex;
            learningButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(selectedContents[randomContentIndex].japaneseContent);
            //Debug.Log("***learningButton " + i.ToString() + "  --Got: " + randomContentIndex +
            //    "  --ID: " + selectedContents[randomContentIndex].contentID);
            learningButton = "learningButton" + i.ToString();   // Write which content we wrote in which button
            PlayerPrefs.SetInt(learningButton, randomContentIndex);

            contentOrder++;
        }
        // --- Randomly laydown for the right side
        // Reset the ones that helps to pick random content
        randomContentIndex = 0; selectedOnes = new int[] { 9, 9, 9, 9, 9 };
        contentOrder = 0; selectedSame = false;
        for (int i = 5; i < selectedContents.Length + 5; i++) { // Do for 5 buttons
            do { // Pick random number (0-5) but should be different from others
                selectedSame = false;

                randomContentIndex = Random.Range(0, selectedContents.Length);

                for (int j = 0; j < selectedOnes.Length; j++) {    // check others
                    if (randomContentIndex == selectedOnes[j])
                        selectedSame = true;
                }
            }
            while (selectedSame);
            // If we get different order, then place it to the next button and save this random content number
            selectedOnes[i - 5] = randomContentIndex; // this is 0-4 array. 5 in length
            learningButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(selectedContents[randomContentIndex].englishContent);
            //Debug.Log("***learningButton " + i.ToString() + "  --Got: " + randomContentIndex +
            //    "  --ID: " + selectedContents[randomContentIndex].contentID);
            learningButton = "learningButton" + i.ToString();   // Write which content we wrote in which button
            PlayerPrefs.SetInt(learningButton, randomContentIndex);

            contentOrder++;
        }

        foreach(Button button in learningButtons) {
            button.image.color = normalColor;
            button.gameObject.SetActive(true);
        }
        laydown = false;
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
    
    public void SetLearningAnswer(int answer) {
        newAnswer = true;
        newSelection = answer;
    }

}
