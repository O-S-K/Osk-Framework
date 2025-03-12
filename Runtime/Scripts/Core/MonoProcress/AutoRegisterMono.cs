using UnityEngine;

namespace OSK
{
    public abstract class AutoRegisterMono : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            Main.Mono.Register(this);
        }

        protected virtual void OnDisable()
        {
            Main.Mono.Unregister(this);
        }
    }

    /* example of usage
      public class Player : AutoRegisterMono, IUpdate, IFixedUpdate, ILateUpdate
      {
          public void Tick()
          {
              Debug.Log("Player Update");
          }

          public void FixedTick()
          {
              Debug.Log("Player Fixed Update");
          }

          public void LateTick()
          {
              Debug.Log("Player Late Update");
          }
      } 
     */
}