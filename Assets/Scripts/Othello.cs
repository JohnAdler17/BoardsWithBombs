using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Othello : MonoBehaviour
{
  private int playerTurn = 0; // either 0 or 1 for p1 or p2's turn

  [SerializeField] private GameObject[] turnIcons; // displays who's turn it is
  [SerializeField] private Sprite[] playerPieces; // first sprite is player 1's piece, second is player 2's piece
  [SerializeField] private Button[] columnButtons;

  [SerializeField] private TextMeshProUGUI winnerText;

  [SerializeField] private Image[] boardImages;

  private int boardHeight = 8;
  private int boardLength = 8;
  private int[,] boardState;

  [SerializeField] private TMP_InputField[] playerInputFields;
  [SerializeField] private TextMeshProUGUI[] playerNames;
  private int playerNameIndex;

  [SerializeField] private Button[] bombButtons;
  [SerializeField] private TextMeshProUGUI[] bombText;
  [SerializeField] private Sprite usingBombImage;
  [SerializeField] private Sprite notUsingBombImage;
  [SerializeField] private TextMeshProUGUI[] numBombsText;
  [SerializeField] private int[] numberOfBombs; // initialize in GameSetup()
  [SerializeField] private TMP_Dropdown bombsDropdown;
  private bool playerUsingBomb = false;
  private int numBombs = 3;

  AudioPlayer audioPlayer;

  void Start() {
    audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioPlayer>();
  }

  private void InitializeGameBoard() {
    boardState = new int[boardLength, boardHeight];
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
  }

  private int columnPicked;
  private int rowPicked;

  public void SetColumn(int numCol) {
    columnPicked = numCol;
  }

  public void SetRow(int numRow) {
    rowPicked = numRow;
  }

  public void PickSquare() {
    TakeTurn(columnPicked, rowPicked);
  }

  private void TakeTurn(int column, int row) {
    //if (CheckLegalMove(column, row)) {
      //switch player turns, icons
      if (playerTurn == 0) {
        turnIcons[0].SetActive(false);
        turnIcons[1].SetActive(true);

        bombButtons[0].interactable = false;
        if (numberOfBombs[1] != 0) {
          bombButtons[1].interactable = true;
        }


        playerTurn = 1;
      }
      else {
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);

        if (numberOfBombs[0] != 0) {
          bombButtons[0].interactable = true;
        }
        bombButtons[1].interactable = false;


        playerTurn = 0;
      }
    //}
    //else {
      //Debug.Log("Column is full, try again");
    //}
  }

  // checks to see if the move is legal then plays the piece
  private bool CheckLegalMove(int col, int row) {
    // loop through vertical column
    for (int i = 0; i < boardHeight; i++) {
      // access square through boardState[col, i]
    }

    // loop through horizontal row
    for (int i = 0; i < boardLength; i++) {
      // access square through boardState[i, row]
    }

    int distanceFromRightEdge = boardLength - col;
    int distanceFromLeftEdge = boardLength - distanceFromRightEdge - 1;
    if (distanceFromRightEdge == boardLength) {
      distanceFromLeftEdge = 0;
    }

    int distanceFromTopEdge = boardHeight - row;
    int distanceFromBottomEdge = boardHeight - distanceFromTopEdge - 1;
    if (distanceFromTopEdge == boardHeight) {
      distanceFromBottomEdge = 0;
    }


    // loop through diagonal column up-right
    if (distanceFromTopEdge > distanceFromRightEdge) {
      for (int i = 0; i < distanceFromRightEdge; i++) {
        // access square through boardState[col + i, row + i]
      }
    }
    else {
      for (int i = 0; i < distanceFromTopEdge; i++) {
        // access square through boardState[col + i, row + i]
      }
    }

    // loop through diagonal column down-left
    if (distanceFromBottomEdge > distanceFromLeftEdge) {
      for (int i = 0; i < distanceFromLeftEdge; i++) {
        // access square through boardState[col - i, row - i]
      }
    }
    else {
      for (int i = 0; i < distanceFromBottomEdge; i++) {
        // access square through boardState[col - i, row - i]
      }
    }

    // loop through diagonal column up-left
    if (distanceFromTopEdge > distanceFromLeftEdge) {
      for (int i = 0; i < distanceFromLeftEdge; i++) {
        // access square through boardState[col - i, row + i]
      }
    }
    else {
      for (int i = 0; i < distanceFromTopEdge; i++) {
        // access square through boardState[col - i, row + i]
      }
    }

    // loop through diagonal column down-right
    if (distanceFromBottomEdge > distanceFromRightEdge) {
      for (int i = 0; i < distanceFromRightEdge; i++) {
        // access square through boardState[col + i, row - i]
      }
    }
    else {
      for (int i = 0; i < distanceFromBottomEdge; i++) {
        // access square through boardState[col + i, row - i]
      }
    }


    return false;
  }

  private void WinnerCheck() {
    // whoever has most pieces their own color, wins
  }

  private void WinnerDisplay() {
    // displays which player one the game once the board is filled or there are no more legal moves (and the players have no more bombs)
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
}
