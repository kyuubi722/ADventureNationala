using UnityEngine;
using System.Linq;
using TMPro;
using System;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class RunningGameDB : MonoBehaviour
{
    int Questionid;
    string QuestionTXT = "2";
    string QuestionAns1;
    string QuestionAns2;
    string QuestionAns3;    
    string QuestionAns4;
    string solution;
    int QuestionRightAns;
    public static short lives=3;
    string query;
    public static int numberOfTables=0;
    private short presumedLives=3;
    public static bool Canclick;
    public static double score;
    public static int maxScore;
    public static float pos1Xcoord, pos2Xcoord, pos3Xcoord;
    private int questionnumberHandler=0;
    public static int Cash=0;
    private GameObject life;
    private GameObject lifeHolder;
    private GameObject intrebare;
    public static short CorrectAnswer;
    public static int WrongAnswer;
    
    private bool Safezone = true;
    public static string[] Wronganswers = new string[]{
        "", "", "",
    };
    public static string[] CurrentQuestions = new string[]{
        "-1", "-1", "-1"
    };//holds the current questions id so the player will not have 2 or 3 questions alike at the same time//
    GameObject endscreen;
    public void Start(){
        endscreen = GameObject.Find("EndOfGameScreen");
        lifeHolder= GameObject.Find("LifeHolder");
        pos1Xcoord= 13.4f;
        pos2Xcoord= 14.01f;
        pos3Xcoord= 14.49f;
    }
    //resets different parameters//
    public void ResetGame()
    {
        Wronganswers = new string[]{
        "", "", "",
        };
        score = 0;
        Safezone= true;
        CurrentQuestions = new string[]{
            "", "", ""
        };
        questionnumberHandler=0;
        lives=3;
        presumedLives=3;
    }

//the big boss function, spawns questions//
  public void SpawnQuestionsCoroutine(string QuestionPos)
{
    if (lives > 0)
    {
        Debug.Log($"Spawning question for {QuestionPos}");

        DBcon connection = new DBcon("adventure");
        query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'adventure' ORDER BY RAND() LIMIT 1;";
        string randomtablename = connection.ExecuteScalar(query).ToString();
        Start:
        query = "SELECT QuestionID, QuestionTXT, QuestionAns1, QuestionAns2, QuestionAns3, QuestionAns4, QuestionRightAns, QuestionSolution FROM " + randomtablename + " ORDER BY RAND() LIMIT 1;";
        try
        {
            connection.OpenConnection();
            using (MySqlDataReader reader = connection.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    Questionid = reader.GetInt32("QuestionID");
                    if (CurrentQuestions.Contains(Questionid.ToString()))
                    {
                        Debug.Log($"QuestionID {Questionid} already exists in CurrentQuestions. Skipping.");
                        goto Start;
                    }
                    QuestionTXT = reader.GetString("QuestionTXT");
                    QuestionAns1 = reader.GetString("QuestionAns1");
                    QuestionAns2 = reader.GetString("QuestionAns2");
                    QuestionAns3 = reader.GetString("QuestionAns3");
                    QuestionAns4 = reader.GetString("QuestionAns4");
                    QuestionRightAns = reader.GetInt32("QuestionRightAns");
                    solution = reader.GetString("QuestionSolution");
                    CurrentQuestions[questionnumberHandler % 3] = QuestionTXT;
                    SpawnQuestionBigBOSS(QuestionTXT, QuestionAns1, QuestionAns2, QuestionAns3, QuestionAns4, QuestionRightAns, QuestionPos, randomtablename, Questionid);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
        finally
        {
            connection.CloseConnection();
        }
    }
    questionnumberHandler++;
}
//loads questions based on the one found
void SpawnQuestionBigBOSS(string QuestionTXT, string ANS1, string ANS2, string ANS3, string ANS4, int RightAns, string position, string tablename, int questionid)
{
    switch (position)
    {
        case "Position1":
            Debug.Log("Spawning question at Position1");
            intrebare = GameObject.Find("HolderPosition1");
            if (intrebare != null)
            {
                intrebare.transform.localPosition = new Vector3(pos1Xcoord, 17f, 4.31f);
                QuestionPos1 question = intrebare.GetComponent<QuestionPos1>();
                question.QuestionID = questionid;
                question.QuestionTXT = QuestionTXT;
                question.QuestionAns1 = ANS1;
                question.QuestionAns2 = ANS2;
                question.QuestionAns3 = ANS3;
                question.QuestionAns4 = ANS4;
                question.QuestionCorrectAns = RightAns;
                question.position = position;
                question.tablename = tablename;
            }
            else
            {
                Debug.LogError("GameObject HolderPosition1 not found!");
            }
            break;

        case "Position2":
            Debug.Log("Spawning question at Position2");
            intrebare = GameObject.Find("HolderPosition2");
            if (intrebare != null)
            {
                intrebare.transform.localPosition = new Vector3(pos2Xcoord, 19.41f, 0f);
                QuestionPos2 question = intrebare.GetComponent<QuestionPos2>();
                question.QuestionID = questionid;
                question.QuestionTXT = QuestionTXT;
                question.QuestionAns1 = ANS1;
                question.QuestionAns2 = ANS2;
                question.QuestionAns3 = ANS3;
                question.QuestionAns4 = ANS4;
                question.QuestionCorrectAns = RightAns;
                question.position = position;
                question.tablename = tablename;
            }
            else
            {
                Debug.LogError("GameObject HolderPosition2 not found!");
            }
            break;

        case "Position3":
            Debug.Log("Spawning question at Position3");
            intrebare = GameObject.Find("HolderPosition3");
            if (intrebare != null)
            {
                intrebare.transform.localPosition = new Vector3(pos3Xcoord, 16.25f, -4.56f);
                QuestionPos3 question = intrebare.GetComponent<QuestionPos3>();
                question.QuestionID = questionid;
                question.QuestionTXT = QuestionTXT;
                question.QuestionAns1 = ANS1;
                question.QuestionAns2 = ANS2;
                question.QuestionAns3 = ANS3;
                question.QuestionAns4 = ANS4;
                question.QuestionCorrectAns = RightAns;
                question.position = position;
                question.tablename = tablename;
            }
            else
            {
                Debug.LogError("GameObject HolderPosition3 not found!");
            }
            break;
    }
}

    void FixedUpdate()
    {
        setActiveUnActive();
        if(lives==0&&WorkCamScript.aWindowIsOpened==true){
            endgame();
        }
        if(WorkCamScript.GameStarted&& Safezone){
            SpawnLifeHolder();
            loadlives(presumedLives);
            SpawnQuestionsCoroutine("Position1");
            SpawnQuestionsCoroutine("Position2");
            SpawnQuestionsCoroutine("Position3");
            WorkCamScript.aWindowIsOpened = true;
            Safezone = false;
        }
        if(WorkCamScript.GameStarted){
            if(presumedLives!=lives){
                loadlives(presumedLives);
            }
        }
        
    }
    //Score calculator//
    double CalculateScore(int rightAnswers, int WrongAnswers){
        const int pointsPerRight = 10;
        const int pointsPerWrong = 5;
        double baseScore = (rightAnswers * pointsPerRight) - (WrongAnswers * pointsPerWrong);
        double finalScore = baseScore;
        if(finalScore<0)finalScore=0;
        int roundedScore = (int)Math.Round(finalScore);
        return roundedScore;
    }
    //Life holder //
    void setActiveUnActive(){
        if(lives==0){
            lifeHolder.SetActive(false);
        }else{
            lifeHolder.SetActive(true);
        }
    }
    //spawns the end screen that contains score and more//
    void endgame(){
        WorkCamScript.aWindowIsOpened=false;
        WorkCamScript.GameStarted=false;
        score = CalculateScore(CorrectAnswer, WrongAnswer);
        if(lifeHolder!=null){
            lifeHolder.transform.position = new Vector3(-99f,-99f,-99f);
        }
        for(int i = 1; i<4; i++){
            GameObject aux = GameObject.Find("HolderPosition"+i);
            aux.transform.localPosition = new Vector3(0,0,0);
        }
        Cash = Cash + Convert.ToInt32(score);
        spawnEndScreen(score);
    }
    void spawnEndScreen(double score){
        Canclick= true;
        if(endscreen!=null){
            endscreen.transform.localPosition=new Vector3(14.39f,18.52f,0);
            TextMeshPro scoreTXT = GameObject.Find("Punctaj").GetComponent<TextMeshPro>();
            scoreTXT.text = score.ToString()+" Puncte";
        }else{
            Debug.Log("problem");
        }
    }
    void SpawnLifeHolder(){
        lifeHolder = GameObject.Find("LifeHolder");
        if(lifeHolder!=null){
            lifeHolder.transform.localPosition = new Vector3(3.75f, 0.2f, -4f);
        }
    }
    //updates the number of lifes//
    void loadlives(short presumedLives){
        short PositionHandler = -3;
        for(short i=presumedLives; i>=1;i--){
            if(i<=lives){
                spawnlife(PositionHandler, i);
            }else{
               despawnlife(i);
            }
            PositionHandler+=3;
        }
        presumedLives=lives;
    }
    void spawnlife(short position, short i){
        life = GameObject.Find("Life"+i);
        if(life!=null){
            life.transform.localPosition = new Vector3(position, 0, 0);
        }
    }
    void despawnlife(short i){
        life = GameObject.Find("Life"+i);
        if(life!=null){
            life.transform.localPosition = new Vector3(-999f, -999f, -999f);
        }
    }
}
