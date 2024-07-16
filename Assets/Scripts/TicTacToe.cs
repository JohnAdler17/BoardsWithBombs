using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TicTacToe : MonoBehaviour
{
    public int playerTurn = 0; // 0 = player 1 turn, 1 = player 2 turn

    public int turnCount = 0;

    [SerializeField] private GameObject[] turnIcons; // displays who's turn it is
    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private Button[] tictactoeSquares;

    [SerializeField] private int[] markedSquares; //IDs which space was marked by which player

    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private GameObject[] winningLines;

    [SerializeField] private Button[] bombButtons;
    [SerializeField] private TextMeshProUGUI[] bombText;
    [SerializeField] private Sprite usingBombImage;
    [SerializeField] private Sprite notUsingBombImage;
    [SerializeField] private TextMeshProUGUI[] numBombsText;
    private bool playerUsingBomb = false;
    [SerializeField] private int[] numberOfBombs; // initialize in GameSetup()
    private int numBombs = 3;
    [SerializeField] private TMP_Dropdown bombsDropdown;

    [SerializeField] private TMP_InputField[] playerInputFields;
    [SerializeField] private TextMeshProUGUI[] playerNames;
    private int playerNameIndex;

    [SerializeField] private Image coverButtonsImage;

    AudioPlayer audioPlayer;

    void Start() {
      audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioPlayer>();
    }

    public void StartGame() {
      GameSetup();
    }

    public void ResetScene() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSceneAt(int index) {
      SceneManager.LoadScene(index);
    }

    public void SetPlayerNameIndex(int index) {
      playerNameIndex = index;
    }

    public void GrabNameInput() {
      playerNames[playerNameIndex].text = playerInputFields[playerNameIndex].text;
    }

    public void ChangeNumBombs() {
      if (bombsDropdown.value == 0) {
        numBombs = 3;
      }
      else if (bombsDropdown.value == 1) {
        numBombs = 2;
      }
      else if (bombsDropdown.value == 2) {
        numBombs = 1;
      }
      else if (bombsDropdown.value == 3) {
        numBombs = 0;
      }
    }

    public void TogglePlayerUsingBomb(int playerNum) {
      if (!playerUsingBomb) {
        playerUsingBomb = true;
        // change player using bomb text and button image
        bombButtons[playerNum].image.sprite = usingBombImage;
        bombText[playerNum].text = "Using Bomb";
      }
      else {
        playerUsingBomb = false;
        // change player using bomb text and button image
        bombButtons[playerNum].image.sprite = notUsingBombImage;
        bombText[playerNum].text = "Not Using Bomb";
      }
    }

    private void DeactivateBombButton(int playerNum) {
      playerUsingBomb = false;
      bombButtons[playerNum].image.sprite = notUsingBombImage;
      bombText[playerNum].text = "Not Using Bomb";
    }

    private void GameSetup() {
      turnIcons[0].SetActive(true);
      turnIcons[1].SetActive(false);
      bombButtons[0].interactable = true;
      bombButtons[1].interactable = false;

      // change static 3 to be how many bombs are selected
      numberOfBombs[0] = numBombs;
      numberOfBombs[1] = numBombs;
      if (numBombs == 0) {
        numBombsText[0].text = "No Bombs Left";
        numBombsText[1].text = "No Bombs Left";
        bombButtons[0].interactable = false;
      }
      else {
        numBombsText[0].text = "Bombs Left: " + numberOfBombs[0];
        numBombsText[1].text = "Bombs Left: " + numberOfBombs[1];
      }

      for (int i = 0; i < tictactoeSquares.Length; i++) {
        tictactoeSquares[i].interactable = true;
        tictactoeSquares[i].GetComponent<Image>().sprite = null;
      }

      for (int i = 0; i < markedSquares.Length; i++) {
        markedSquares[i] = -100;
      }
    }

    private IEnumerator ActivateBomb(int number) {
      audioPlayer.PlaySizzleSoundEffect();

      // enable coverButtonsImage
      coverButtonsImage.gameObject.SetActive(true);

      tictactoeSquares[number].image.sprite = usingBombImage;

      yield return new WaitForSeconds(1f);

      audioPlayer.PlayBoomSoundEffect();

      // disable coverButtonsImage
      coverButtonsImage.gameObject.SetActive(false);

      tictactoeSquares[number].image.sprite = null;

      if (number == 0) {
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 1) {
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number - 1] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 2) {
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        markedSquares[number - 1] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 3) {
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number - 3] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 4) {
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number - 1] = -100;
        markedSquares[number - 3] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 5) {
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        tictactoeSquares[number + 3].image.sprite = null;
        tictactoeSquares[number + 3].interactable = true;
        markedSquares[number - 1] = -100;
        markedSquares[number - 3] = -100;
        markedSquares[number + 3] = -100;
      }
      else if (number == 6) {
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number - 3] = -100;
      }
      else if (number == 7) {
        tictactoeSquares[number + 1].image.sprite = null;
        tictactoeSquares[number + 1].interactable = true;
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        markedSquares[number + 1] = -100;
        markedSquares[number - 1] = -100;
        markedSquares[number - 3] = -100;
      }
      else if (number == 8) {
        tictactoeSquares[number - 3].image.sprite = null;
        tictactoeSquares[number - 3].interactable = true;
        tictactoeSquares[number - 1].image.sprite = null;
        tictactoeSquares[number - 1].interactable = true;
        markedSquares[number - 1] = -100;
        markedSquares[number - 3] = -100;
      }
    }

    public void TicTacToeButton(int number) {
      if (playerUsingBomb) {

        StartCoroutine(ActivateBomb(number));

        numberOfBombs[playerTurn] --;
        if (numberOfBombs[playerTurn] == 0) {
          numBombsText[playerTurn].text = "No Bombs Left";
        }
        else {
          numBombsText[playerTurn].text = "Bombs Left: " + numberOfBombs[playerTurn];
        }

        DeactivateBombButton(playerTurn);
      }
      else {
        tictactoeSquares[number].image.sprite = playerIcons[playerTurn];
        tictactoeSquares[number].interactable = false;

        markedSquares[number] = playerTurn + 1;
        turnCount++;

        if (turnCount > 4) {
          WinnerCheck();
        }
      }


      if (playerTurn == 0) {
        audioPlayer.PlayP1MoveSoundEffect();
        playerTurn = 1;
        turnIcons[0].SetActive(false);
        turnIcons[1].SetActive(true);
        bombButtons[0].interactable = false;
        if (numberOfBombs[1] != 0) {
          bombButtons[1].interactable = true;
        }
      }
      else {
        audioPlayer.PlayP2MoveSoundEffect();
        playerTurn = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);
        if (numberOfBombs[0] != 0) {
          bombButtons[0].interactable = true;
        }
        bombButtons[1].interactable = false;
      }

    }

    private void WinnerCheck() {
      int sq1 = markedSquares[0] + markedSquares[1] + markedSquares[2];
      int sq2 = markedSquares[3] + markedSquares[4] + markedSquares[5];
      int sq3 = markedSquares[6] + markedSquares[7] + markedSquares[8];
      int sq4 = markedSquares[0] + markedSquares[3] + markedSquares[6];
      int sq5 = markedSquares[1] + markedSquares[4] + markedSquares[7];
      int sq6 = markedSquares[2] + markedSquares[5] + markedSquares[8];
      int sq7 = markedSquares[0] + markedSquares[4] + markedSquares[8];
      int sq8 = markedSquares[2] + markedSquares[4] + markedSquares[6];

      int[] solutions = new int[] {sq1, sq2, sq3, sq4, sq5, sq6, sq7, sq8};

      for (int i = 0; i < solutions.Length; i++) {
        if (solutions[i] == 3 * (playerTurn + 1)) {
          WinnerDisplay(i);
          return;
        }
      }
    }

    private void WinnerDisplay(int indexIn) {
      winnerText.gameObject.SetActive(true);

      if (playerTurn == 0) {
        winnerText.text = playerNames[0].text + " Wins!";
      }
      else if (playerTurn == 1) {
        winnerText.text = playerNames[1].text + " Wins!";
      }

      winningLines[indexIn].SetActive(true);

      for (int i = 0; i < tictactoeSquares.Length; i++) {
        tictactoeSquares[i].interactable = false;
      }
    }
}
