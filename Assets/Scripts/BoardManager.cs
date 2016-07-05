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

      public float TileWidth = 16.0f;
      public float TileHeight = 24.0f;
      public float KeyPressDelay = 0.2f;

      public Map Map;

      private float _lastKeyPressTime;
      private GameObject _player;

      public void Start()
      {
         Map = Map.Create( new BorderOnlyMapCreationStrategy<Map>( 10, 10 ) );

         Transform boardHolder = GameObject.Find( "Board" ).transform;
         foreach ( var cell in Map.GetAllCells() )
         {
            int x = cell.X * 16;
            int y = cell.Y * 24;
            if ( cell.IsWalkable )
            {
               GameObject instance = Instantiate( Floor, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
               instance.transform.SetParent( boardHolder );
            }
            else
            {
               GameObject instance = Instantiate( Wall, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
               instance.transform.SetParent( boardHolder );
            }
         }

         _player = Instantiate( Player, new Vector3( TileWidth, TileHeight, 0f ), Quaternion.identity ) as GameObject;
         _player.transform.SetParent( boardHolder );  

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
               playerTransform.position = new Vector3( playerTransform.position.x + TileWidth, playerTransform.position.y );
            }
            else if ( horizontal < 0 )
            {
               playerTransform.localScale = new Vector3( 1, 1, 1 );
               playerTransform.position = new Vector3( playerTransform.position.x - TileWidth, playerTransform.position.y );
            }
            else if ( vertical > 0 )
            {
               playerTransform.position = new Vector3( playerTransform.position.x, playerTransform.position.y + TileHeight );
            }
            else if ( vertical < 0 )
            {
               playerTransform.position = new Vector3( playerTransform.position.x, playerTransform.position.y - TileHeight );
            }
         }
      }
   }
}
