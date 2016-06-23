using UnityEngine;
using System.Collections;

namespace RogueSharpUnity
{
   public abstract class MovingObject : MonoBehaviour
   {
      public float moveTime = 0.1f;       
      public LayerMask blockingLayer;     

      private BoxCollider2D boxCollider;  
      private Rigidbody2D rb2D;           
      private float inverseMoveTime;      

      protected virtual void Start()
      {
         boxCollider = GetComponent<BoxCollider2D>();

         rb2D = GetComponent<Rigidbody2D>();

         inverseMoveTime = 1f / moveTime;
      }

      protected bool Move( int xDir, int yDir, out RaycastHit2D hit )
      {
         Vector2 start = transform.position;

         Vector2 end = start + new Vector2( xDir, yDir );

         boxCollider.enabled = false;

         hit = Physics2D.Linecast( start, end, blockingLayer );
         Debug.Log( hit ); 
         boxCollider.enabled = true;

         if ( hit.transform == null )
         {
            StartCoroutine( SmoothMovement( end ) );

            return true;
         }

         return false;
      }

      protected IEnumerator SmoothMovement( Vector3 end )
      {
         float sqrRemainingDistance = ( transform.position - end ).sqrMagnitude;

         while ( sqrRemainingDistance > float.Epsilon )
         {
            Vector3 newPostion = Vector3.MoveTowards( rb2D.position, end, inverseMoveTime * Time.deltaTime );

            rb2D.MovePosition( newPostion );

            sqrRemainingDistance = ( transform.position - end ).sqrMagnitude;

            yield return null;
         }
      }

      protected virtual void AttemptMove( int xDir, int yDir )
      {
         //RaycastHit2D hit;

         //bool canMove = Move( xDir, yDir, out hit );

         //if ( hit.transform == null )
         //{
         //   return;
         //}
      }
   }
}
