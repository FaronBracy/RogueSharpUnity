using UnityEngine;

namespace Assets.Scripts
{
   public class MovePlayer : MonoBehaviour
   {
      public float ElapsedTime;
      public float LastKeyPressTime;

      public void Start()
      {
         // See https://www.reddit.com/r/Unity2D/comments/2eeo4n/pixel_perfection_in_2d_xpost_from_unity3d/?st=iq709jzu&sh=5ae55305
         Camera.main.orthographicSize = Screen.height / 2.0f;
         LastKeyPressTime = Time.time;
      }

      public void Update()
      {
         ElapsedTime = Time.time;
         if ( Time.time - LastKeyPressTime > 0.2f )
         {
            LastKeyPressTime = Time.time;
            float vertical = Input.GetAxisRaw( "Vertical" );
            float horizontal = Input.GetAxisRaw( "Horizontal" );

            if ( vertical > 0 )
            {
               transform.position = new Vector3( transform.position.x, transform.position.y + 24.0f );
            }
            else if ( vertical < 0 )
            {
               transform.position = new Vector3( transform.position.x, transform.position.y - 24.0f );
            }
            else if ( horizontal > 0 )
            {
               transform.localScale = new Vector3( -1, 1, 1 );
               transform.position = new Vector3( transform.position.x + 16.0f, transform.position.y);
            }
            else if ( horizontal < 0 )
            {
               transform.localScale = new Vector3( 1, 1, 1 );
               transform.position = new Vector3( transform.position.x - 16.0f, transform.position.y );
            }
         }
      }
   }
}
