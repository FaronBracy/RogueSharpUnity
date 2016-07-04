using RogueSharp;
using RogueSharp.MapCreation;
using UnityEngine;

namespace Assets.Scripts
{
   public class BoardManager : MonoBehaviour
   {
      public void Start()
      {
         var map = Map.Create( new BorderOnlyMapCreationStrategy<Map>( 10, 10 ) );

         Transform boardHolder = GameObject.Find( "Board" ).transform;  
         GameObject mapWall = GameObject.Find( "Wall" );
         GameObject mapFloor = GameObject.Find( "Floor" );
         foreach ( var cell in map.GetAllCells() )
         {
            int x = cell.X * 16;
            int y = cell.Y * 24;
            if ( cell.IsWalkable )
            {
               GameObject instance = Instantiate( mapFloor, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
               instance.transform.SetParent( boardHolder );
            }
            else
            {
               GameObject instance = Instantiate( mapWall, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
               instance.transform.SetParent( boardHolder );
            }
         }
      }
   }
}
