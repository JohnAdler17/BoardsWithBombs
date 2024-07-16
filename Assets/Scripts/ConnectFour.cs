using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectFour : MonoBehaviour
{
    private int playerTurn = 0; // either 0 or 1 for p1 or p2's turn

    [SerializeField] private GameObject[] turnIcons; // displays who's turn it is
    [SerializeField] private Sprite[] playerPieces; // first sprite is player 1's piece, second is player 2's piece
    [SerializeField] private Button[] columnButtons;

    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private Image[] boardImages;

    private int boardHeight = 6;
    private int boardLength = 7;
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

    public void StartGame() {
      InitializeGameBoard();
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

    public void ColumnSelected(int numCol) {
      TakeTurn(numCol);
    }

    private void TakeTurn(int column) {
      if (UpdateBoardState(column)) {
        //switch player turns, icons
        if (playerTurn == 0) {
          turnIcons[0].SetActive(false);
          turnIcons[1].SetActive(true);

          bombButtons[0].interactable = false;
          if (numberOfBombs[1] != 0) {
            bombButtons[1].interactable = true;
          }

          if (WinnerCheck(1)) {
            //Debug.Log("Player 1 won");
            WinnerDisplay();
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

          if (WinnerCheck(2)) {
            //Debug.Log("Player 2 won");
            WinnerDisplay();
          }
          playerTurn = 0;
        }
      }
      else {
        Debug.Log("Column is full, try again");
      }
    }

    private IEnumerator ActivateBomb(int numCol, int row) {
      audioPlayer.PlaySizzleSoundEffect();
      for (int i = 0; i < columnButtons.Length; i++) {
        columnButtons[i].interactable = false;
      }
      boardImages[numCol * boardHeight + row].sprite = usingBombImage;
      yield return new WaitForSeconds(1f);

      audioPlayer.PlayBoomSoundEffect();

      boardState[numCol, row] = 0;
      boardImages[numCol * boardHeight + row].sprite = null;

      if (numCol == 0) {
        // if column is last on left

        // if row is not on the bottom
        if (row != 0) {
          // removes piece to the bottom right
          boardState[numCol + 1, row - 1] = 0;
          boardImages[(numCol + 1) * boardHeight + row - 1].sprite = null;

          // removes piece below
          boardState[numCol, row - 1] = 0;
          boardImages[numCol * boardHeight + row - 1].sprite = null;
        }

        // if row is not on the top
        if (row < boardHeight - 1) {
          // removes piece above
          boardState[numCol, row + 1] = 0;
          boardImages[numCol * boardHeight + row + 1].sprite = null;

          // removes piece to the top right
          boardState[numCol + 1, row + 1] = 0;
          boardImages[(numCol + 1) * boardHeight + row + 1].sprite = null;
        }

        // removes piece to the right
        boardState[numCol + 1, row] = 0;
        boardImages[(numCol + 1) * boardHeight + row].sprite = null;


        // check to see if any game pieces can fall on the right
        if (boardState[numCol + 1, row + 2] != 0) {

          int numToFallDown = 0;

          // finds the fall height for pieces on right
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol + 1, r] != 0 && r > row) {
              numToFallDown = r - row;
              if (row > 0) { numToFallDown++; }
              break;
            }
          }

          // loops through the column and makes the pieces fall
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol + 1, r] != 0 && numToFallDown != 0 && r > numToFallDown - 1) {
              int movePiece = boardState[numCol + 1, r];
              Sprite pieceSprite =  boardImages[(numCol + 1) * boardHeight + r].sprite;

              boardState[numCol + 1, r - numToFallDown] = movePiece;
              boardImages[(numCol + 1) * boardHeight + r - numToFallDown].sprite = pieceSprite;

              boardState[numCol + 1, r] = 0;
              boardImages[(numCol + 1) * boardHeight + r].sprite = null;
            }
          }
        }

      }

      else if (numCol != 0 && numCol < boardLength - 1) {
        // if column is in the middle

        if (row != 0) {
          // removes piece below
          boardState[numCol, row - 1] = 0;
          boardImages[numCol * boardHeight + row - 1].sprite = null;

          // removes piece to the bottom right
          boardState[numCol + 1, row - 1] = 0;
          boardImages[(numCol + 1) * boardHeight + row - 1].sprite = null;

          // removes piece to the bottom left
          boardState[numCol - 1, row - 1] = 0;
          boardImages[(numCol - 1) * boardHeight + row - 1].sprite = null;
        }

        if (row < boardHeight - 1) {
          // removes piece above
          boardState[numCol, row + 1] = 0;
          boardImages[numCol * boardHeight + row + 1].sprite = null;

          // removes piece to the top right
          boardState[numCol + 1, row + 1] = 0;
          boardImages[(numCol + 1) * boardHeight + row + 1].sprite = null;

          // removes piece to the top left
          boardState[numCol - 1, row + 1] = 0;
          boardImages[(numCol - 1) * boardHeight + row + 1].sprite = null;
        }

        // removes piece to the left
        boardState[numCol - 1, row] = 0;
        boardImages[(numCol - 1) * boardHeight + row].sprite = null;

        // removes piece to the right
        boardState[numCol + 1, row] = 0;
        boardImages[(numCol + 1) * boardHeight + row].sprite = null;



        // check to see if any game pieces can fall on the right
        if (boardState[numCol + 1, row + 2] != 0) {

          int numToFallDown = 0;

          // finds the fall height for pieces on right
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol + 1, r] != 0 && r > row) {
              numToFallDown = r - row;
              if (row > 0) { numToFallDown++; }
              break;
            }
          }

          // loops through the column and makes the pieces fall
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol + 1, r] != 0 && numToFallDown != 0 && r > numToFallDown - 1) {
              int movePiece = boardState[numCol + 1, r];
              Sprite pieceSprite =  boardImages[(numCol + 1) * boardHeight + r].sprite;

              boardState[numCol + 1, r] = 0;
              boardImages[(numCol + 1) * boardHeight + r].sprite = null;

              boardState[numCol + 1, r - numToFallDown] = movePiece;
              boardImages[(numCol + 1) * boardHeight + r - numToFallDown].sprite = pieceSprite;
            }
          }
        }

        // check to see if any game pieces can fall on the left
        if (boardState[numCol - 1, row + 2] != 0) {

          int numToFallDown = 0;

          // finds the fall height for pieces on left
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol - 1, r] != 0 && r > row) {
              numToFallDown = r - row;
              if (row > 0) { numToFallDown++; }
              break;
            }
          }

          // loops through the column and makes the pieces fall
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol - 1, r] != 0 && numToFallDown != 0 && r > numToFallDown - 1) {
              int movePiece = boardState[numCol - 1, r];
              Sprite pieceSprite =  boardImages[(numCol - 1) * boardHeight + r].sprite;

              boardState[numCol - 1, r] = 0;
              boardImages[(numCol - 1) * boardHeight + r].sprite = null;

              boardState[numCol - 1, r - numToFallDown] = movePiece;
              boardImages[(numCol - 1) * boardHeight + r - numToFallDown].sprite = pieceSprite;
            }
          }
        }

      }

      else {
        // if column is last on right

        if (row != 0) {
          // removes piece to the bottom left
          boardState[numCol - 1, row - 1] = 0;
          boardImages[(numCol - 1) * boardHeight + row - 1].sprite = null;

          // removes piece below
          boardState[numCol, row - 1] = 0;
          boardImages[numCol * boardHeight + row - 1].sprite = null;
        }

        if (row < boardHeight - 1) {
          // removes piece to the top left
          boardState[numCol - 1, row + 1] = 0;
          boardImages[(numCol - 1) * boardHeight + row + 1].sprite = null;

          // removes piece above
          boardState[numCol, row + 1] = 0;
          boardImages[numCol * boardHeight + row + 1].sprite = null;
        }

        // removes piece to the left
        boardState[numCol - 1, row] = 0;
        boardImages[(numCol - 1) * boardHeight + row].sprite = null;


        // check to see if any game pieces can fall on the left
        if (boardState[numCol - 1, row + 2] != 0) {

          int numToFallDown = 0;

          // finds the fall height for pieces on left
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol - 1, r] != 0 && r > row) {
              numToFallDown = r - row;
              if (row > 0) { numToFallDown++; }
              break;
            }
          }

          // loops through the column and makes the pieces fall
          for (int r = 0; r < boardHeight; r++) {
            if (boardState[numCol - 1, r] != 0 && numToFallDown != 0 && r > numToFallDown - 1) {
              int movePiece = boardState[numCol - 1, r];
              Sprite pieceSprite =  boardImages[(numCol - 1) * boardHeight + r].sprite;

              boardState[numCol - 1, r] = 0;
              boardImages[(numCol - 1) * boardHeight + r].sprite = null;

              boardState[numCol - 1, r - numToFallDown] = movePiece;
              boardImages[(numCol - 1) * boardHeight + r - numToFallDown].sprite = pieceSprite;
            }
          }
        }
      }

      for (int i = 0; i < columnButtons.Length; i++) {
        columnButtons[i].interactable = true;
      }
    }

    private bool UpdateBoardState(int numCol) {
      for (int row = 0; row < boardHeight; row++) {
        if (boardState[numCol, row] == 0) {

          if (playerUsingBomb) {

            StartCoroutine(ActivateBomb(numCol, row));

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
            if (playerTurn == 0) {
              audioPlayer.PlayP1MoveSoundEffect();

              boardState[numCol, row] = 1;
              boardImages[numCol * boardHeight + row].sprite = playerPieces[0];
            }
            else {
              audioPlayer.PlayP2MoveSoundEffect();

              boardState[numCol, row] = 2;
              boardImages[numCol * boardHeight + row].sprite = playerPieces[1];
            }
          }

          return true;
        }
      }
      return false;
    }

    private bool WinnerCheck(int playerNum) {
      // check for horizontal win
      for (int i = 0; i < boardLength - 3; i++) {
        for (int j = 0; j < boardHeight; j++) {
          if (boardState[i, j] == playerNum && boardState[i + 1, j] == playerNum && boardState[i + 2, j] == playerNum && boardState[i + 3, j] == playerNum) {
            return true;
          }
        }
      }
      // check for vertical win
      for (int i = 0; i < boardLength; i++) {
        for (int j = 0; j < boardHeight - 3; j++) {
          if (boardState[i, j] == playerNum && boardState[i, j + 1] == playerNum && boardState[i, j + 2] == playerNum && boardState[i, j + 3] == playerNum) {
            return true;
          }
        }
      }
      // check for diagonal win (y = x)
      for (int i = 0; i < boardLength - 3; i++) {
        for (int j = 0; j < boardHeight - 3; j++) {
          if (boardState[i, j] == playerNum && boardState[i + 1, j + 1] == playerNum && boardState[i + 2, j + 2] == playerNum && boardState[i + 3, j + 3] == playerNum) {
            return true;
          }
        }
      }
      // check for diagonal win (y = -x)
      for (int i = 0; i < boardLength - 3; i++) {
        for (int j = 0; j < boardHeight - 3; j++) {
          if (boardState[i, j + 3] == playerNum && boardState[i + 1, j + 2] == playerNum && boardState[i + 2, j + 1] == playerNum && boardState[i + 3, j] == playerNum) {
            return true;
          }
        }
      }

      return false;
    }

    private void WinnerDisplay() {
      winnerText.gameObject.SetActive(true);
      //winnerText.text = "Player " + (playerTurn+1).ToString() + " Wins!";
      winnerText.text = playerNames[playerTurn].text + " Wins!";


      for (int i = 0; i < columnButtons.Length; i++) {
        columnButtons[i].interactable = false;
      }
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
