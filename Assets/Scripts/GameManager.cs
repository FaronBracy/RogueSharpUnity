using UnityEngine;
using UnityEngine.UI;

namespace RogueSharpUnity
{
   public class GameManager : MonoBehaviour
   {
      public float levelStartDelay = 2f;
      public float turnDelay = 0.1f;
      public int playerFoodPoints = 10000;
      public static GameManager instance = null;

      [HideInInspector]
      public bool playersTurn = true;

      private Text levelText;
      private GameObject levelImage;
      private BoardManager boardScript;
      private int level = 1;
      private bool doingSetup = true;

      public void Awake()
      {
         if ( instance == null )
         {
            instance = this;
         }
         else if ( instance != this )
         {
            Destroy( gameObject );
         }

         DontDestroyOnLoad( gameObject );

         boardScript = GetComponent<BoardManager>();

         InitGame();
      }

      public void OnLevelWasLoaded( int index )
      {
         level++;
         InitGame();
      }

      public void InitGame()
      {
         doingSetup = true;

         levelImage = GameObject.Find( "LevelImage" );

         levelText = GameObject.Find( "LevelText" ).GetComponent<Text>();

         levelText.text = "Day " + level;

         levelImage.SetActive( true );

         Invoke( "HideLevelImage", levelStartDelay );

         boardScript.SetupScene( level );
      }

      public void HideLevelImage()
      {
         levelImage.SetActive( false );

         doingSetup = false;
      }

      public void GameOver()
      {
         levelText.text = "After " + level + " days, you starved.";

         levelImage.SetActive( true );

         enabled = false;
      }
   }
}

