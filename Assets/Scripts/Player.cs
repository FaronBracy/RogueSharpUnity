using UnityEngine;
using UnityEngine.UI;

namespace RogueSharpUnity
{
   public class Player : MovingObject
   {
      public float restartLevelDelay = 1f;
      public int pointsPerFood = 10;
      public int pointsPerSoda = 20;
      public Text foodText;
      public AudioClip moveSound1;
      public AudioClip moveSound2;
      public AudioClip eatSound1;
      public AudioClip eatSound2;
      public AudioClip drinkSound1;
      public AudioClip drinkSound2;
      public AudioClip gameOverSound;

      private Animator animator;
      private int food;

      protected override void Start()
      {
         this.transform.position = GameManager.instance.GetComponent<BoardManager>().GetRandomEmptyCell();
         animator = GetComponent<Animator>();

         food = GameManager.instance.playerFoodPoints;

         foodText.text = "Food: " + food;

         base.Start();
      }

      private void OnDisable()
      {
         GameManager.instance.playerFoodPoints = food;
      }

      private void Update()
      {
         if ( !GameManager.instance.playersTurn ) return;

         int horizontal = 0;
         int vertical = 0;

         horizontal = (int) ( Input.GetAxisRaw( "Horizontal" ) );

         vertical = (int) ( Input.GetAxisRaw( "Vertical" ) );

         if ( horizontal != 0 )
         {
            vertical = 0;
         }

         if ( horizontal != 0 || vertical != 0 )
         {
            AttemptMove( horizontal, vertical );
         }
      }

      protected override void AttemptMove( int xDir, int yDir )
      {
         food--;

         foodText.text = "Food: " + food;

         //base.AttemptMove( xDir, yDir );

         RaycastHit2D hit;

         if ( Move( xDir, yDir, out hit ) )
         {
            SoundManager.instance.RandomizeSfx( moveSound1, moveSound2 );
         }

         CheckIfGameOver();

         GameManager.instance.playersTurn = false;
      }

      private void OnTriggerEnter2D( Collider2D other )
      {
         if ( other.tag == "Exit" )
         {
            Invoke( "Restart", restartLevelDelay );

            enabled = false;
         }
         else if ( other.tag == "Food" )
         {
            food += pointsPerFood;

            foodText.text = "+" + pointsPerFood + " Food: " + food;

            SoundManager.instance.RandomizeSfx( eatSound1, eatSound2 );

            other.gameObject.SetActive( false );
         }
         else if ( other.tag == "Soda" )
         {
            food += pointsPerSoda;

            foodText.text = "+" + pointsPerSoda + " Food: " + food;

            SoundManager.instance.RandomizeSfx( drinkSound1, drinkSound2 );

            other.gameObject.SetActive( false );
         }
      }

      private void Restart()
      {
         Application.LoadLevel( Application.loadedLevel );
      }

      public void LoseFood( int loss )
      {
         animator.SetTrigger( "playerHit" );

         food -= loss;

         foodText.text = "-" + loss + " Food: " + food;

         CheckIfGameOver();
      }

      private void CheckIfGameOver()
      {
         if ( food <= 0 )
         {
            SoundManager.instance.PlaySingle( gameOverSound );

            SoundManager.instance.musicSource.Stop();

            GameManager.instance.GameOver();
         }
      }
   }
}

