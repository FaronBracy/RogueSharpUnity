using UnityEngine;
using Random = UnityEngine.Random;
using RogueSharp;
using RogueSharp.MapCreation;

namespace RogueSharpUnity
{
   public class BoardManager : MonoBehaviour
   {
      public int Width = 60;
      public int Height = 40;
      public GameObject exit;
      public GameObject[] floorTiles;								   
      public GameObject[] outerWallTiles;

      private Transform _boardHolder;
      private Map _map;

      public void BoardSetup()
      {
         //var mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>( Width, Height, 20, 18, 6 );
         var mapCreationStrategy = new BorderOnlyMapCreationStrategy<Map>( Width, Height );
         _map = Map.Create( mapCreationStrategy );

         _boardHolder = new GameObject( "Board" ).transform;

         foreach ( var cell in _map.GetAllCells() )
         {
            GameObject toInstantiate = floorTiles[Random.Range( 0, floorTiles.Length )];

            if ( !cell.IsWalkable )
            {
               toInstantiate = outerWallTiles[Random.Range( 0, outerWallTiles.Length )];
            }

            GameObject instance = Instantiate( toInstantiate, new Vector3( cell.X, cell.Y, 0f ), Quaternion.identity ) as GameObject;
            
            //instance.GetComponent<SpriteRenderer>().color = Color.grey;

            //Set the parent of our newly instantiated object instance to _boardHolder, this is just organizational to avoid cluttering hierarchy.
            instance.transform.SetParent( _boardHolder );
         }
      }

      public Vector3 GetRandomEmptyCell()
      {
         int randomX = Random.Range( 0, Width );
         int randomY = Random.Range( 0, Height );

         Cell cell = _map.GetCell( randomX, randomY );

         if ( cell.IsWalkable )
         {
            return new Vector3( randomX, randomY, 0f );
         }

         return GetRandomEmptyCell();
      }


      public void SetupScene( int level )
      {
         BoardSetup();

         Instantiate( exit, GetRandomEmptyCell(), Quaternion.identity );
      }
   }
}
