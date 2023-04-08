using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomHelpers
{
    public static class RigidBodyHelper
    {
        public static void SetVelocity(this Rigidbody2D rigidbody2D, Vector2 velocity)
        {
            rigidbody2D.velocity = velocity;
        }
        
        public static void SetVelocity(this Rigidbody2D rigidbody2D, float xVelocity, float yVelocity)
        {
            rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
        }

        public static void SetVelocityX(this Rigidbody2D rigidbody2D, float xVelocity)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.ReplaceX(xVelocity);
        }
        
        public static void SetVelocityY(this Rigidbody2D rigidbody2D, float yVelocity)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.ReplaceY(yVelocity);
        }
        

        public static void ResetVelocity(this Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }
}
