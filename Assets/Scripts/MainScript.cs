using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class MainScript : MonoBehaviour
{

    public GameObject xFigure;
    public GameObject oFigure;
    public GameObject winLine;
    public GameObject[] arrayGameObject = new GameObject[9];

    private List<GameObject> GameObjectsOnPlaneList = new List<GameObject>();
    private int?[,] matrix = new int?[3, 3];
    private GameObject figure;
    private GameObject placementPlan;
    private bool gamePause;
    private bool gameFinish;
    private bool AIModeOn;
    private int difficult;
    private int playerIndex = 0;
    private int numberMove = 0;

    private int switchIntForDifficultyButtonsPositions = -305;
    private int switchIntForButtonsPositions = -205;
    private int switchIntForBox = 165;
    private bool switchStateInfo;
    private bool switchDifficultyStateInfo;
 //   private string switchButtonIcon = "Open Menu";
    private int menuShedow = 0;
    private int boxHeight = 205;
    private string winerName;
    private int xWins;
    private int oWins;
    private int draws;

    void Update()
    {
        if (!gameFinish) //If the game is not over
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            /*
            if (!AIModeOn) //If selected 2 players mod
            {
                if (!gamePause && Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100)) //Create a ray, if menu is closed
                {
                    PlayerMove(hit); //Call playerMove
                    WhoWiner(); //select a winner
                }
            }
            if (AIModeOn)
            {*/
                if (playerIndex == 0)
                {
                    if (!gamePause && Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100)) //Create a ray, if menu is closed
                    {
                        PlayerMove(hit); //Call playerMove
                        WhoWiner(); //select a winner
                    }
                }
                else
                {
                    int randomSleepNumber = Random.Range(250, 500);
                    System.Threading.Thread.Sleep(randomSleepNumber); //Delay before the AI ​​move
                    if (difficult != 0) //If selected Hard or Middle difficult level
                    {
                        HighLogic();
                    }
                    else
                    {
                        LowLogic();
                    }
                    WhoWiner(); //select a winner
                }
            //}
        }
    }

    private void PlayerMove(RaycastHit hit)
    {
        var mouseTargetAll = hit.collider.gameObject;
        if (mouseTargetAll.tag == "Trigger_Plane_Open") //Check for free place
        {
            placementPlan = mouseTargetAll;
            PutFigure(placementPlan.transform.position, placementPlan.transform.rotation); //Call method, which create figure on free place.
            placementPlan.tag = "Trigger_Plane_Close"; //Close free place
            UpdateLogicMatrix(); //Call method, which update matrix for AI and WhoWiner() method.
            placementPlan = null; //after action
        }
    }

    private void HighLogic()
    {

        if (CheckAiWinLose(0)) return; //Check possible to win. If successful - break this method
        if (CheckAiWinLose(1)) return; //Check possible to lose. If successful - break this method
        LowLogic(); //Make any move
    }

    private bool CheckAiWinLose(int i)
    {
        for (int x = 0; x < 3; x++) //Check all lines to possible to win or lose on the next move
        {
            if (matrix[x, 0] == i && matrix[x, 1] == i && matrix[x, 2] == null)
            {
                LoadFigureToThePlacementPlan(x, 2);//Call method for AI, which call method to create figure on free place.
                return true;
            }
            if (matrix[x, 0] == i && matrix[x, 1] == null && matrix[x, 2] == i)
            {
                LoadFigureToThePlacementPlan(x, 1);
                return true;
            }
            if (matrix[x, 0] == null && matrix[x, 1] == i && matrix[x, 2] == i)
            {
                LoadFigureToThePlacementPlan(x, 0);
                return true;
            }
            if (matrix[0, x] == i && matrix[1, x] == i && matrix[2, x] == null)
            {
                LoadFigureToThePlacementPlan(2, x);
                return true;
            }
            if (matrix[0, x] == i && matrix[1, x] == null && matrix[2, x] == i)
            {
                LoadFigureToThePlacementPlan(1, x);
                return true;
            }
            if (matrix[0, x] == null && matrix[1, x] == i && matrix[2, x] == i)
            {
                LoadFigureToThePlacementPlan(0, x);
                return true;
            }
        }

        if (matrix[0, 0] == i && matrix[1, 1] == i && matrix[2, 2] == null)
        {
            LoadFigureToThePlacementPlan(2, 2);
            return true;
        }
        if (matrix[0, 0] == i && matrix[1, 1] == null && matrix[2, 2] == i)
        {
            LoadFigureToThePlacementPlan(1, 1);
            return true;
        }
        if (matrix[0, 0] == null && matrix[1, 1] == i && matrix[2, 2] == i)
        {
            LoadFigureToThePlacementPlan(0, 0);
            return true;
        }
        if (matrix[0, 2] == i && matrix[1, 1] == i && matrix[2, 0] == null)
        {
            LoadFigureToThePlacementPlan(2, 0);
            return true;
        }
        if (matrix[0, 2] == i && matrix[1, 1] == null && matrix[2, 0] == i)
        {
            LoadFigureToThePlacementPlan(1, 1);
            return true;
        }
        if (matrix[0, 2] == null && matrix[1, 1] == i && matrix[2, 0] == i)
        {
            LoadFigureToThePlacementPlan(0, 2);
            return true;
        }
        return false;
    }


    private void LowLogic()
    {
        if (matrix[1, 1] == null || matrix[0, 0] == null || matrix[0, 2] == null || matrix[2, 2] == null || matrix[2, 0] == null) //Check free priority place
        {
            int aiRandom;
            if (difficult == 2) //if difficult level is Hard
            {
                aiRandom = Random.Range(0, 6); //do not make the mistake
            }
            else aiRandom = Random.Range(0, 8);//make a mistake with the chance 5 to 2
            switch (aiRandom)
            {
                case 0:
                    if (matrix[0, 0] == null)
                    {
                        LoadFigureToThePlacementPlan(0, 0);
                    }
                    break;
                case 1:
                    if (matrix[0, 2] == null)
                    {
                        LoadFigureToThePlacementPlan(0, 2);
                    }
                    break;
                case 2:
                    if (matrix[2, 2] == null)
                    {
                        LoadFigureToThePlacementPlan(2, 2);
                    }
                    break;
                case 3:
                    if (matrix[2, 0] == null)
                    {
                        LoadFigureToThePlacementPlan(2, 0);
                    }
                    break;
                case 4:
                    if (matrix[1, 1] == null)
                    {
                        LoadFigureToThePlacementPlan(1, 1);
                    }
                    break;
                default:
                    FreePlaceSelection(); //our mistake (select any free place)
                    break;
            }
        }
        else
        {
            FreePlaceSelection();//select any free place
        }

    }

    private void FreePlaceSelection()//select any free place
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (matrix[i, j] == null)
                {
                    LoadFigureToThePlacementPlan(i, j);
                    return; 
                }
            }
        }
    }

    private void LoadFigureToThePlacementPlan(int x, int y) //find a place for matrix position 
    {
        int k = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (x == i && y == j) //if found
                {
                    matrix[x, y] = 0; //add move to matrix for AI and WhoWiner() method.
                    PutFigure(arrayGameObject[k].transform.position, arrayGameObject[k].transform.rotation);//Call method, which create figure on free place.
                    arrayGameObject[k].tag = "Trigger_Plane_Close"; //Close free place

                }
                k++;
            }
        }
    }

    private void PutFigure(Vector3 placementPlanPosition, Quaternion placementPlanRotation)
    {
        if (playerIndex == 0)
        {
            figure = xFigure;
            playerIndex = 1;
        }
        else
        {
            figure = oFigure;
            playerIndex = 0;
        }
        GameObjectsOnPlaneList.Add((GameObject)Instantiate(figure, placementPlanPosition, placementPlanRotation));//Create Player/AI figure on selected place.
        numberMove++;
    }

    private void UpdateLogicMatrix()//Update matrix for AI after Player move
    {
        for (int i = 0; i <= 8; i++) 
        {
            if (arrayGameObject[i] == placementPlan)
            {
                int a, b;
                switch (i)
                {
                    case 0:
                        a = 0;
                        b = 0;
                        matrix[a, b] = playerIndex;
                        break;
                    case 1:
                        a = 0;
                        b = 1;
                        matrix[a, b] = playerIndex;
                        break;
                    case 2:
                        a = 0;
                        b = 2;
                        matrix[a, b] = playerIndex;
                        break;
                    case 3:
                        a = 1;
                        b = 0;
                        matrix[a, b] = playerIndex;
                        break;
                    case 4:
                        a = 1;
                        b = 1;
                        matrix[a, b] = playerIndex;
                        break;
                    case 5:
                        a = 1;
                        b = 2;
                        matrix[a, b] = playerIndex;
                        break;
                    case 6:
                        a = 2;
                        b = 0;
                        matrix[a, b] = playerIndex;
                        break;
                    case 7:
                        a = 2;
                        b = 1;
                        matrix[a, b] = playerIndex;
                        break;
                    case 8:
                        a = 2;
                        b = 2;
                        matrix[a, b] = playerIndex;
                        break;
                }
            }
        }
    }

    private void WhoWiner() //Select the winer. Check all line.
    {
        for (int i = 0; i <= 1; i++)
        {
            if (matrix[0, 0] == i && matrix[0, 1] == i && matrix[0, 2] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, 6), new Quaternion(0, 0.7f, 0, 0.7f)));
                EndGame(i);
                return;
            }
            if (matrix[1, 0] == i && matrix[1, 1] == i && matrix[1, 2] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, 0), new Quaternion(0, 0.7f, 0, 0.7f)));
                EndGame(i);
                return;
            }
            if (matrix[2, 0] == i && matrix[2, 1] == i && matrix[2, 2] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, -6), new Quaternion(0, 0.7f, 0, 0.7f)));
                EndGame(i);
                return;
            }

            if (matrix[0, 0] == i && matrix[1, 0] == i && matrix[2, 0] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(-6, 0, 0), new Quaternion(0, 0, 0, 1)));
                EndGame(i);
                return;
            }
            if (matrix[0, 1] == i && matrix[1, 1] == i && matrix[2, 1] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1)));
                EndGame(i);
                return;
            }
            if (matrix[0, 2] == i && matrix[1, 2] == i && matrix[2, 2] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(6, 0, 0), new Quaternion(0, 0, 0, 1)));
                EndGame(i);
                return;
            }

            if (matrix[0, 0] == i && matrix[1, 1] == i && matrix[2, 2] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, 0), new Quaternion(0, 0.9f, 0, 0.375f)));
                EndGame(i);
                return;
            }
            if (matrix[0, 2] == i && matrix[1, 1] == i && matrix[2, 0] == i)
            {
                GameObjectsOnPlaneList.Add((GameObject)Instantiate(winLine, new Vector3(0, 0, 0), new Quaternion(0, 0.375f, 0, 0.9f)));
                EndGame(i);
                return;
            }
        }
        if (numberMove == 9) //It's a Draw
        {
            EndGame(2);
        }
    }

    private void EndGame(int winerIndex) //Add winers to variables and finish game
    {
        if (winerIndex == 1)
        {
            winerName = "X Won!";
            xWins++;
            Application.LoadLevel(1);
        }
        else if (winerIndex == 0)
        {
            winerName = "0 Won!";
            oWins++;
            Application.LoadLevel(4);
        }
        else
        {
            winerName = "It's a draw!";
            draws++;
            Application.LoadLevel(4);
        }
        gameFinish = true;
    }

    private void NewGame() //Start new Game (delete all figure from scene, clean matrix for AI, reset variables, open all place) and random select Player
    {
        foreach (GameObject killingGameObject in GameObjectsOnPlaneList)
        {
            Destroy(killingGameObject.gameObject);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix[i, j] = null;
            }
        }
        numberMove = 0;
        gameFinish = false;
        winerName = String.Empty;
        foreach (var plane in arrayGameObject)
        {
            plane.tag = "Trigger_Plane_Open";
        }
       // playerIndex = Random.Range(0, 2);
        playerIndex = 2;
        
    }







    void OnGUI()
    {
        GuiGroup();
    }

    private void GuiGroup()
    {
        float boxPosWeidth = Screen.width / 2 - 105;
        float boxPosHeight = 5f;
        GUI.Box(new Rect(-10, -10, menuShedow, menuShedow), "");
        GUI.Box(new Rect(boxPosWeidth, boxPosHeight, 210, boxHeight - switchIntForBox), "");
        if (GUI.Button(new Rect(boxPosWeidth + 10f, boxPosHeight + 35f + switchIntForButtonsPositions, 190, 50), "New Game"))
        {
            NewGame();
            SwitchButtonGroup();
        }
       /* if (GUI.Button(new Rect(boxPosWeidth + 10f, boxPosHeight + 90f + switchIntForButtonsPositions, 190, 50), "Two Player Mode"))
        {
            difficult = 0;
            AIModeOn = false;
            NewGame();
            SwitchButtonGroup();
        }*/
        if (GUI.Button(new Rect(boxPosWeidth + 10f, boxPosHeight + 145f + switchIntForButtonsPositions, 190, 50), "One Player Mode"))
        {
            SwitchDifficultyButton();
        }
      /*  if (GUI.Button(new Rect(boxPosWeidth + 10f, boxPosHeight + 10f, 190, 20), switchButtonIcon))
        {
            SwitchButtonGroup();
        }*/
        if (GUI.Button(new Rect(boxPosWeidth + 10f, boxPosHeight + 220f + switchIntForDifficultyButtonsPositions, 60, 50), "Low"))
        {
            difficult = 0;
            AIModeOn = true;
            NewGame();
            SwitchButtonGroup();
        }
        if (GUI.Button(new Rect(boxPosWeidth + 75f, boxPosHeight + 220f + switchIntForDifficultyButtonsPositions, 60, 50), "Medium"))
        {
            difficult = 1;
            AIModeOn = true;
            NewGame();
            SwitchButtonGroup();
        }
        if (GUI.Button(new Rect(boxPosWeidth + 140f, boxPosHeight + 220f + switchIntForDifficultyButtonsPositions, 60, 50), "High"))
        {
            difficult = 2;
            AIModeOn = true;
            NewGame();
            SwitchButtonGroup();
        }
        GUI.Label(new Rect(boxPosWeidth + 65f, boxPosHeight + 195f + switchIntForDifficultyButtonsPositions, 190, 20), "Difficulty");
        if (gameFinish)
        {
            GUI.Window(0, new Rect(Screen.width / 2f - 100f, Screen.height / 2f - 35f, 200, 70), GameFinishWindow, "Result");
        }
     /*   if (GUI.Button(new Rect(Screen.width - 60f, Screen.height - 60f, 50, 50), "Quit"))
        {
            Application.Quit();
        }*/

        GUI.Box(new Rect(Screen.width / 2f - 50, Screen.height - 50f, 100, 150), "Score");
        GUI.Label(new Rect(Screen.width / 2f - 90, Screen.height - 30f, 180, 40), String.Format("{0} {3}; {1} {4}; {2} {5}", "X Wins:", "O Wins", "Draws:", xWins, oWins, draws));
    }

    void GameFinishWindow(int windowId)
    {
        GUI.Label(new Rect(30, 20, 200, 80), "<size=30>" + winerName + "</size>");
    }

    private void SwitchButtonGroup() //Show/Hide the menu
    {
        if (!switchStateInfo)
        {
            switchIntForButtonsPositions = 0;
            switchIntForBox = 0;
            switchStateInfo = true;
            gamePause = true;
            menuShedow = Screen.width + 100;
            //switchButtonIcon = "Close Menu";
        }
        else
        {
            switchIntForButtonsPositions = -205;
            switchIntForDifficultyButtonsPositions = -305;
            switchIntForBox = 165;
            menuShedow = 0;
            boxHeight = 205;
          //  switchButtonIcon = "Open Menu";
            switchStateInfo = false;
            switchDifficultyStateInfo = false;
            gamePause = false;
        }
    }

    private void SwitchDifficultyButton()//Show/Hide the Difficult level
    {
        if (!switchDifficultyStateInfo)
        {
            switchDifficultyStateInfo = true;
            switchIntForDifficultyButtonsPositions = 0;
            boxHeight += 75;
        }
        else
        {
            switchDifficultyStateInfo = false;
            switchIntForDifficultyButtonsPositions = -305;
            boxHeight -= 75;
        }
    }
}
