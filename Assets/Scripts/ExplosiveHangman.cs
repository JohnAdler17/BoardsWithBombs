using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExplosiveHangman : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown bombDropdownMenu;

    [SerializeField] private Button bombButton;
    [SerializeField] private TextMeshProUGUI numBombsText;
    [SerializeField] private Sprite usingBombImage;
    [SerializeField] private Sprite notUsingBombImage;

    [SerializeField] private Image bombImage;
    [SerializeField] private Image coverButtonsImage;

    private int numBombs = 3;

    [SerializeField] private TMP_InputField[] playerInputFields;
    [SerializeField] private TextMeshProUGUI[] playerNames;

    private int playerNameIndex;

    [SerializeField] private TMP_InputField[] wordInputFields;
    [SerializeField] private TextMeshProUGUI[] wordInputText;
    private int numWords = 4;
    private int wordNameIndex;

    private string[] wordStrings = new string[4];

    private string pickedWord;

    private char[] pickedWordChars = new char[12];

    [SerializeField] private Image[] stickFigureImages;

    private int guesserHealth = 0;

    [SerializeField] private Button[] guessLetterButtons;

    [SerializeField] private TextMeshProUGUI[] blankLetters;
    [SerializeField] private Image[] blankLetterImages;

    private int revealedLetters = 0;

    private int pickedWordIndex;

    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI cantUseBombText;

    AudioPlayer audioPlayer;

    void Start()
    {
      // fixes an error where if the hanger hasn't input any words, wordStrings array is empty -> this makes the default words "word"
      wordStrings[0] = "Bunt";
      wordStrings[1] = "Nagger";
      wordStrings[2] = "Pitch";
      wordStrings[3] = "Default";
      audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioPlayer>();
    }

    public void StartGame() {
      InitializeGameBoard();
    }

    private void InitializeGameBoard() {
      // deactivate stick figure images
      for (int i = 0; i < stickFigureImages.Length; i++) {
        stickFigureImages[i].gameObject.SetActive(false);
      }

      // if numBombs = 0, disable bomb button
      if (numBombs == 0) {
        bombButton.interactable = false;
        numBombsText.text = "No Bombs Left";
      }

      // choose random word from word strings, make word all lowercase, parse word, and set each char to the text component of each image in blankLetters
      ChooseWord();
    }

    public void GuessLetter(string letter) {
      audioPlayer.PlayP1MoveSoundEffect();

      // if letter is in the pickedWordChars array, loop through array reveal the letter(s) in the respective position in blankLetters array
      char[] guessedLetter = letter.ToCharArray();
      bool isInWord = false;

      for (int i = 0; i < pickedWordChars.Length; i++) {
        if (guessedLetter[0] == pickedWordChars[i]) {
          isInWord = true;
          break;
        }
      }

      // if letter is not in pickedWordChars array, reveal a piece of the stick man and determine whether the guesser lost
      if (isInWord) {
        for (int i = 0; i < pickedWordChars.Length; i++) {
          if (guessedLetter[0] == pickedWordChars[i]) {
            blankLetters[i].gameObject.SetActive(true);
            revealedLetters++;
          }
        }
      }
      else {
        stickFigureImages[guesserHealth].gameObject.SetActive(true);
        guesserHealth++;
      }

      // Winner/Loser check - if guesser health is 6, guesser loses, if revealedLetters = pickedWordChars.Length, guesser wins
      WinnerCheck();
    }

    public void DisableLetterButton(int index) {
      guessLetterButtons[index].interactable = false;
    }

    public void SetPlayerNameIndex(int index) {
      playerNameIndex = index;
    }

    public void GrabNameInput() {
      playerNames[playerNameIndex].text = playerInputFields[playerNameIndex].text;
    }

    public void SetWordNameIndex(int index) {
      wordNameIndex = index;
    }

    public void GrabWordNameInput() {
      wordStrings[wordNameIndex] = wordInputFields[wordNameIndex].text;
      //Debug.Log(wordStrings[wordNameIndex]);
    }

    private void ChooseWord() {
      // reset all found letters from previous word
      for (int i = 0; i < blankLetters.Length; i++) {
        blankLetters[i].gameObject.SetActive(false);
      }
      revealedLetters = 0;

      // enable all guessLetterButtons
      for (int i = 0; i < guessLetterButtons.Length; i++) {
        guessLetterButtons[i].interactable = true;
      }

      int randomIndex = Random.Range(0, numWords);

      if (randomIndex == pickedWordIndex) {
        randomIndex += 1;

        if (randomIndex > numWords - 1) {
          randomIndex = 0;
        }
      }
      pickedWord = wordStrings[randomIndex];

      pickedWordIndex = randomIndex;

      // convert pickedWord string into all lowercase
      pickedWord = pickedWord.ToUpper();

      //Debug.Log(pickedWord);

      //parse pickedWord into pickedWordChars array
      pickedWordChars = pickedWord.ToCharArray();

      //set chars in pickedWordChars array to be the text of each space in blankLetters
      for (int i = 0; i < blankLetterImages.Length; i++) {
        if (i < pickedWordChars.Length) {
          blankLetterImages[i].gameObject.SetActive(true);
          blankLetters[i].text = pickedWordChars[i].ToString();
          if (blankLetters[i].text == " ") {
            blankLetterImages[i].gameObject.SetActive(false);
            revealedLetters++;
          }
        }
        else {
          blankLetterImages[i].gameObject.SetActive(false);
        }

      }
    }

    private void WinnerCheck() {
      if (guesserHealth == 6) {
        // disable all letter buttons
        for (int i = 0; i < guessLetterButtons.Length; i++) {
          guessLetterButtons[i].interactable = false;
        }

        // reveal word
        for (int i = 0; i < pickedWordChars.Length; i++) {
          blankLetters[i].gameObject.SetActive(true);
        }

        //guesser loses
        //Debug.Log("Hanger wins");
        winnerText.gameObject.SetActive(true);
        winnerText.text = playerNames[0].text + " Wins!";
      }

      if (revealedLetters == pickedWordChars.Length) {
        //disable all letter buttons
        for (int i = 0; i < guessLetterButtons.Length; i++) {
          guessLetterButtons[i].interactable = false;
        }

        //guesser wins
        //Debug.Log("Guesser wins");
        winnerText.gameObject.SetActive(true);
        winnerText.text = playerNames[1].text + " Wins!";
      }
    }

    private void DeactivateWordFields() {
      for (int i = 0; i < 4; i++) {
        wordInputFields[i].gameObject.SetActive(false);
        wordInputText[i].gameObject.SetActive(false);
      }
    }

    private void ActivateWordFields(int num) {
      for (int i = 0; i < num; i++) {
        wordInputFields[i].gameObject.SetActive(true);
        wordInputText[i].gameObject.SetActive(true);
      }
    }

    public void ChangeNumBombs() {
      if (bombDropdownMenu.value == 0) {
        numBombs = 3;
        numWords = 4;
        ActivateWordFields(4);
      }
      else if (bombDropdownMenu.value == 1) {
        numBombs = 2;
        numWords = 3;
        DeactivateWordFields();
        ActivateWordFields(3);
      }
      else if (bombDropdownMenu.value == 2) {
        numBombs = 1;
        numWords = 2;
        DeactivateWordFields();
        ActivateWordFields(2);
      }
      else if (bombDropdownMenu.value == 3) {
        numBombs = 0;
        numWords = 1;
        DeactivateWordFields();
        ActivateWordFields(1);
      }
    }

    private IEnumerator FlashBombText() {
      cantUseBombText.gameObject.SetActive(true);
      yield return new WaitForSeconds(2f);
      cantUseBombText.gameObject.SetActive(false);
    }

    public void UseBomb() {
      if (guesserHealth == 0) {
        //Debug.Log("Error: No body parts to remove");
        StartCoroutine(FlashBombText());
      }
      else {
        StartCoroutine(ActivateBomb());
      }
    }

    private IEnumerator ActivateBomb() {
      audioPlayer.PlaySizzleSoundEffect();
      bombImage.gameObject.SetActive(true);
      coverButtonsImage.gameObject.SetActive(true);
      yield return new WaitForSeconds(1f);
      audioPlayer.PlayBoomSoundEffect();
      bombImage.gameObject.SetActive(false);
      coverButtonsImage.gameObject.SetActive(false);

      // first, remove one body part from stick figure and decrement guesser health by 1
      guesserHealth--;
      stickFigureImages[guesserHealth].gameObject.SetActive(false);

      int randomNumber = Random.Range(0, 2);

      if (randomNumber == 0) {
        ChooseWord();
      }

      numBombs--;
      if (numBombs == 0) {
        bombButton.interactable = false;
        numBombsText.text = "No Bombs Left";
      }
      else {
        numBombsText.text = "Bombs Left: " + numBombs;
      }
    }

    public void ResetScene() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSceneAt(int index) {
      SceneManager.LoadScene(index);
    }

}
