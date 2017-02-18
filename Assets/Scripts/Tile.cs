using UnityEngine;

namespace Assets.Scripts
{
   public class Tile : MonoBehaviour
   {
      public bool IsTileLit
      {
         get; set;
      }

      public void Update()
      {
         if ( IsTileLit )
         {
            //GetComponent<Renderer>().material.color = Color.Lerp( Color.white, new Color( .75f, .75f, .75f ), Random.Range( 0f, 1.0f ) );
            GetComponent<Renderer>().material.color = Color.white;
         }
         else
         {
            GetComponent<Renderer>().material.color = Color.grey;
         }
      }
   }
}
