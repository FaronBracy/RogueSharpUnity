using UnityEngine;

namespace RogueSharpUnity
{
   public class Loader : MonoBehaviour
   {
      public GameObject gameManager;
      public GameObject soundManager;


      public void Awake()
      {
         if ( GameManager.instance == null )
         {
            Instantiate( gameManager );
         }

         if ( SoundManager.instance == null )
         {
            Instantiate( soundManager );
         }
      }
   }
}