using RogueSharp;
using RogueSharp.MapCreation;
using UnityEngine;

namespace Assets.Scripts
{
   public class BoardManager : MonoBehaviour
   {
      public GameObject Player;
      public GameObject Floor;
      public GameObject Wall;

      public static float TileWidth = 16.0f;
      public static float TileHeight = 24.0f;
      public static int BoardWidth = 40;
      public static int BoardHeight = 24;
      public float KeyPressDelay = 0.2f;

      public Map Map;
      public GameObject[,] Tiles;

      private float _lastKeyPressTime;
      private GameObject _player;

      public void Start()
      {
         Tiles = new GameObject[BoardWidth, BoardHeight];
         Map = Map.Create( new BorderOnlyMapCreationStrategy<Map>( BoardWidth, BoardHeight ) );

         Transform boardHolder = GameObject.Find( "Board" ).transform;
         foreach ( var cell in Map.GetAllCells() )
         {
            int x = cell.X * 16;
            int y = cell.Y * 24;

            GameObject tileType = Wall;

            if ( cell.IsWalkable )
            {
               tileType = Floor;
            }

            GameObject instance = Instantiate( tileType, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
            instance.transform.SetParent( boardHolder );
            Tiles[cell.X, cell.Y] = instance;
         }

         _player = GameObject.Find( "Player" );
         _player.transform.SetParent( boardHolder );
         _player.transform.position = new Vector3( TileWidth * ( BoardWidth / 2 ), TileHeight * ( BoardHeight / 2 ), 0f );

         // See https://www.reddit.com/r/Unity2D/comments/2eeo4n/pixel_perfection_in_2d_xpost_from_unity3d/?st=iq709jzu&sh=5ae55305
         Camera.main.orthographicSize = Screen.height / 2.0f;
         _lastKeyPressTime = Time.time;
      }

      public void Update()
      {
         Transform playerTransform = _player.transform;

         if ( Input.anyKeyDown || Time.time - _lastKeyPressTime > KeyPressDelay )
         {
            _lastKeyPressTime = Time.time;

            float horizontal = Input.GetAxisRaw( "Horizontal" );
            float vertical = Input.GetAxisRaw( "Vertical" );

            if ( horizontal > 0 )
            {
               playerTransform.localScale = new Vector3( -1, 1, 1 );
               Vector3 newPosition = Vector.Right( playerTransform.position );
               UpdatePlayerPosition( newPosition, playerTransform );
            }
            else if ( horizontal < 0 )
            {
               playerTransform.localScale = new Vector3( 1, 1, 1 );
               Vector3 newPosition = Vector.Left( playerTransform.position );
               UpdatePlayerPosition( newPosition, playerTransform );
            }
            else if ( vertical > 0 )
            {
               Vector3 newPosition = Vector.Up( playerTransform.position );
               UpdatePlayerPosition( newPosition, playerTransform );
            }
            else if ( vertical < 0 )
            {
               Vector3 newPosition = Vector.Down( playerTransform.position );
               UpdatePlayerPosition( newPosition, playerTransform );
            }
         }
      }

      private void UpdatePlayerPosition( Vector3 newPosition, Transform playerTransform )
      {
         Point mapLocation = Vector.ToPoint( newPosition );
         if ( Map.IsWalkable( mapLocation.X, mapLocation.Y ) )
         {
            playerTransform.position = newPosition;
            var fov = new FieldOfView( Map );
            fov.ComputeFov( mapLocation.X, mapLocation.Y, 3, true );

            for ( int x = 0; x < BoardWidth; x++ )
            {
               for ( int y = 0; y < BoardHeight; y++ )
               {
                  GameObject tile = Tiles[x, y];
                  if ( fov.IsInFov( x, y ) )
                  {
                     tile.GetComponent<Renderer>().material.color = Color.yellow;
                  }
                  else
                  {
                     tile.GetComponent<Renderer>().material.color = Color.grey;
                  }
               }
            }
         }
      }

      private static class Vector
      {
         public static Vector3 FromMap( int x, int y )
         {
            return new Vector3( x * TileWidth, y * TileHeight );
         }

         public static Point ToPoint( Vector3 sourceVector )
         {
            int x = 0;
            int y = 0;
            if ( (int) sourceVector.x != 0 )
            {
               x = (int) ( sourceVector.x / TileWidth );
            }
            if ( (int) sourceVector.y != 0 )
            {
               y = (int) ( sourceVector.y / TileHeight );
            }
            return new Point( x, y );
         }

         public static Vector3 Left( Vector3 sourceVector )
         {
            return new Vector3( sourceVector.x - TileWidth, sourceVector.y );
         }

         public static Vector3 Right( Vector3 sourceVector )
         {
            return new Vector3( sourceVector.x + TileWidth, sourceVector.y );
         }
         public static Vector3 Up( Vector3 sourceVector )
         {
            return new Vector3( sourceVector.x, sourceVector.y + TileHeight );
         }
         public static Vector3 Down( Vector3 sourceVector )
         {
            return new Vector3( sourceVector.x, sourceVector.y - TileHeight );
         }
      }
   }
}
