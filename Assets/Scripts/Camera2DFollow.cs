using UnityEngine;

namespace RogueSharpUnity
{
   public class Camera2DFollow : MonoBehaviour
   {
      public Transform target;
      public Camera camera;

      private void Update()
      {
         BoardManager boardManager = GameManager.instance.GetComponent<BoardManager>();

         float camVertExtent = camera.orthographicSize;
         float camHorzExtent = camera.aspect * camVertExtent;

         float leftBound = 0 + camHorzExtent;
         float rightBound = boardManager.Width - 1 - camHorzExtent;
         float bottomBound = 0 + camVertExtent;
         float topBound = boardManager.Height - 1 - camVertExtent;

         float camX = Mathf.Clamp( target.transform.position.x, leftBound, rightBound );
         float camY = Mathf.Clamp( target.transform.position.y, bottomBound, topBound );

         transform.position = new Vector3( camX, camY, transform.position.z );
      }
   }
}
