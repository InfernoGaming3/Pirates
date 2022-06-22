using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityStandardAssets._2D;
//using System;

public class PasswordGame : MonoBehaviour
{
    [Header("UI Stuff:")]
   // public GameObject passwordGameCanvas;
    public TextMeshProUGUI targetInfoText;
    public TextMeshProUGUI underscoretext;
    public TMP_InputField passwordField;
    public TextMeshProUGUI underscoreInputtext;
    public TextMeshProUGUI correctAnswerText;
   //public Button checkAnswerButton;


    [Header("Generate Password Stuff:")]
    string[] _firstNames = new string[] { "Adam", "Alex", "Aaron", "Ben", "Bert", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger", "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter" };

    string[] _lastNames = new string[] { "Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard", "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt", "Ebner", "Frick", "Gibbons", "Hancock", "Haworth", "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki", "Mccord", "McCormack", "Miller", "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew", "Quinn", "Quizoz", "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson", "Shaffer", "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil", "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente", "Van Zandt", "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss", "Woo", "Yang", "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi", "Caswell", "Celedon", "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo", "Dinkins", "Doran", "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer", "Hylan", "Hydinger", "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones", "Jurgenson", "Kalleg", "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki", "Martin", "McGinnis", "Mills", "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson", "Ostrander", "Reamer", "Reardon", "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom", "Sawyer", "Schlicht", "Schmitt", "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler" };

    string[] _occupations = new string[] { "Veternarian", "Doctor", "Engineer", "Spy", "Sniper", "Soldier", "Criminal", "Electrician", "Scientist", "Teacher", "Manager" };
   
    string firstName;
    string lastName;
    string occupation;
    int favNumber;
    public string passwordGenerated;
    string passwordPromptText;
    string prevUnderscores;
    //int birthday;
    PlatformerCharacter2D player;
    Enemy enemy;
    FollowPath enemyPath;

    // Start is called before the first frame update
    void Start()
    {
        //StartPasswordGame();
    }

    public string GeneratePassword()
    {
        print("password is being generated");
        int firstnameRng = UnityEngine.Random.Range(0, _firstNames.Length);
        int lastnameRng = UnityEngine.Random.Range(0, _lastNames.Length);
        int occupationRng = UnityEngine.Random.Range(0, _occupations.Length);

        favNumber = UnityEngine.Random.Range(0, 101);

        int rng = UnityEngine.Random.Range(0, 4);

        if (rng != 3) firstName = _firstNames[firstnameRng]; else firstName = "";
        if (rng != 3) lastName = _lastNames[lastnameRng]; else lastName = "";
        if (rng == 3) occupation = _occupations[occupationRng]; else occupation = "";

        string firstnameAbr = firstName;
        string lastnameAbr = lastName;
        string occupationAbr = occupation;
        if (rng == 1)
        {
            firstnameAbr = _firstNames[firstnameRng];
            firstnameAbr = firstnameAbr.Substring(0, UnityEngine.Random.Range(0, firstnameAbr.Length - 1));
            //firstName = firstnameAbr;

            lastnameAbr = _lastNames[lastnameRng];
            lastnameAbr = lastnameAbr.Substring(0, UnityEngine.Random.Range(0, lastnameAbr.Length - 1));
            //lastName = lastnameAbr;
        }
        int rngOccuAbr = UnityEngine.Random.Range(0, 3);
        if(rngOccuAbr == 1)
        {
            occupationAbr = _occupations[occupationRng];
            occupationAbr = occupationAbr.Substring(0, UnityEngine.Random.Range(0, occupationAbr.Length - 1));
        }


        int orderRng = UnityEngine.Random.Range(0, 5);
        passwordGenerated = "";
        if (orderRng == 1)
        {
            passwordGenerated = firstnameAbr + lastnameAbr + occupationAbr + favNumber;
        }
        else if (orderRng == 2)
        {
            passwordGenerated = lastnameAbr + firstnameAbr + occupationAbr + favNumber;
        }
        else if (orderRng == 3)
        {
            passwordGenerated = favNumber + firstnameAbr + lastnameAbr + occupation;
        }
        else if (orderRng == 0)
        {
            passwordGenerated = occupationAbr + firstnameAbr + lastnameAbr + favNumber;
        }
        else if (orderRng == 4)
        {
            passwordGenerated = favNumber + lastnameAbr + firstnameAbr + occupation;
        }
        //print(passwordGenerated);
        //Not used for calculation only for general info if they're empty
        if (firstName == "") firstName = _firstNames[firstnameRng];
        if (lastName == "") lastName = _lastNames[lastnameRng];
        if (occupation == "") occupation = _occupations[occupationRng];


        return passwordGenerated.ToLower();
    }

    public void SetTargetInfoText()
    {
        targetInfoText.SetText("::target info" + "\n" + "information gathered by target" + "\n" + "First Name: " + firstName + "\n" + "Last Name: " + lastName + "\n" + "Occupation: " + occupation + "\n" + "Favorite Number:" + favNumber);
    }

    private void Update()
    {
        if (this.gameObject.GetComponent<CanvasGroup>().alpha == 1f)
        {
            UpdatePromptText();
            correctAnswerText.SetText(CheckAnswer());
        }


    }

    public void UpdatePromptText()
    {
        string underscoreInputString = passwordField.text;

        string[] inputSpaceString = underscoreInputString.Select(c => c.ToString()).ToArray();

        //print("uIS: " + inputSpaceString);
        underscoreInputtext.SetText(String.Join("    ", inputSpaceString));
    }

    public string CheckAnswer()
    {
        string rightAnswerString = "";
        int i = 0;
        foreach (char Pc in passwordField.text.ToLower())
        {
            int j = 0;
            bool charFound = false;
            foreach(char Uc in passwordGenerated.ToLower())
            {

                if(Uc == Pc && i==j && !charFound)
                {
                    rightAnswerString += Uc + "    ";
                    charFound = true;
                }
                j++;
            }
            if(!charFound) rightAnswerString += "    ";
            i++;
        }
        //print(rightAnswerString);
        return rightAnswerString;
     
    }

    public void EndPasswordGame()
    {
        string answer = CheckAnswer().ToLower().Replace("    ", "");
        print(answer);
        if(answer == passwordGenerated.ToLower() && passwordField.text != "")
        {
            player.freezePlayer = false;
            enemyPath.freezeMovement = false;
            print("password is correct!");
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            Currency _currency = player.GetComponent<Currency>();
            if (player != null) _currency.AddMoney(UnityEngine.Random.Range(1, 1000));
            if(enemy.goldenEnemy) _currency.AddMoney(_currency.GetMoneyToCompleteLevel());
            if (enemyPath.gameObject != null) Destroy(enemyPath.gameObject);
            GetComponentInChildren<Button>().enabled = false;
        }

    }

    public void StartPasswordGame()
    {
        print("password game started");
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        print(this.gameObject.GetComponent<CanvasGroup>().alpha);

        GeneratePassword();

        string underscores = "";
        GetComponentInChildren<Button>().enabled = true;

        // print(passwordGenerated.Length);
        for (int i = 0; i < passwordGenerated.Length; i++)
        {
            // print("underscores are being added");
            underscores += "  _  ";
        }
        underscoretext.SetText(underscores);
        SetTargetInfoText();
        passwordPromptText = passwordField.text;
        prevUnderscores = underscoretext.text;
    }

    public void FreezePlayerEnemy(Enemy enemy, PlatformerCharacter2D player)
    {
        enemyPath = enemy.enemyPath;
        this.enemy = enemy;
        this.player = player;

        enemy.enemyPath.freezeMovement = true;
        player.freezePlayer = true;
    }

}
