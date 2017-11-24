using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class BlockingCollisionResult
    {
        public Vector2 CollisionSpeed { get; set; }
        public Vector2 CorrectedPosition { get; set; }
    }

    public class BlockingCollisionResponder<TFirst, TSecond> : ICollisionResponder<TFirst, TSecond>
        where TFirst : IMovingWorldObject
        where TSecond : ICollidable
    {

     
        void ICollisionResponder<TFirst, TSecond>.RespondToCollision(TFirst moveableObject, TSecond collidable, CollisionInfo collisionInfo)
        {
            var movingCollidable = collidable as IMovingCollidable;

            var tryPosition = new Rectangle();
            bool collideX, collideY;

            tryPosition.Set(moveableObject.Position);
            tryPosition.SetLeft(moveableObject.Motion.FrameStartPosition.Left);

            if(movingCollidable != null)
                collideX = !movingCollidable.DetectFrameStartCollision(tryPosition);
            else
                collideX = !collidable.DetectCollision(tryPosition, true);
          
            tryPosition.Set(moveableObject.Position);
            tryPosition.SetTop(moveableObject.Motion.FrameStartPosition.Top);

            if(movingCollidable != null)
                collideY = !movingCollidable.DetectFrameStartCollision(tryPosition);
            else
                collideY = !collidable.DetectCollision(tryPosition, true);

            tryPosition.Set(moveableObject.Position);

            if (collideY && !collideX)
            {
                TryCorrectPosition(tryPosition, moveableObject, collidable, correctX: false, correctY: true);
            }

            if (collideX && !collideY)
            {
                TryCorrectPosition(tryPosition, moveableObject, collidable, correctX: true, correctY: false);
            }

            if (collidable.DetectCollision(tryPosition,false))
            {
                TryCorrectPosition(tryPosition, moveableObject, collidable, correctX: false, correctY: true);

                if(collidable.DetectCollision(tryPosition,false))
                    TryCorrectPosition(tryPosition, moveableObject, collidable, true, true);
            }

            if (collidable.DetectCollision(tryPosition,false))
                TryCorrectPosition(tryPosition, moveableObject, collidable, true, true, true);

            BeforePositionCorrected(moveableObject, collidable, collisionInfo, tryPosition);

            if (tryPosition.Center.Y < moveableObject.Position.Center.Y - 18)
                GlobalDebugHelper.NoOp();

            moveableObject.Position.Set(tryPosition);            
        }
        
        protected virtual void BeforePositionCorrected(TFirst moveableObject, TSecond collidable, CollisionInfo collisionInfo, Rectangle tryPosition)
        {
            if (tryPosition.Center.X != moveableObject.Position.Center.X)
                moveableObject.Motion.Stop(Axis.X);

            if (tryPosition.Center.Y != moveableObject.Position.Center.Y)
                moveableObject.Motion.Stop(Axis.Y);
        }

        private static void TryCorrectPosition(Rectangle tryPosition, TFirst moveableObject, ICollidable collidable, bool correctX, bool correctY, bool correctPastOriginal=false)
        {
            float newX = tryPosition.Center.X;
            float newY = tryPosition.Center.Y;

            float deltaX=0, deltaY=0;
            if(correctPastOriginal)
            {
                newX = newX.Approach(moveableObject.Motion.FrameStartPosition.Center.X, 1);                
                newY = newY.Approach(moveableObject.Motion.FrameStartPosition.Center.Y, 1);

                deltaX = (newX - moveableObject.Motion.FrameStartPosition.Center.X).Unit();
                deltaY = (newY - moveableObject.Motion.FrameStartPosition.Center.Y).Unit();

                if (deltaX == 0 && deltaY == 0)
                    deltaY = -1;
            }
            while (collidable.DetectCollision(tryPosition,true))
            {
                float newX2 = newX, newY2 = newY;

                if (correctPastOriginal)
                {
                    newX2 = newX + deltaX;
                    newY2 = newY + deltaY;
                }
                else
                {
                    if (correctX)
                        newX2 = newX.Approach(moveableObject.Motion.FrameStartPosition.Center.X, 1);

                    if (correctY)
                        newY2 = newY.Approach(moveableObject.Motion.FrameStartPosition.Center.Y, 1);
                }

                if (newX2 == newX && newY2 == newY)
                {
                    newX = newX2;
                    newY = newY2;
                    break;
                }
                else
                {
                    newX = newX2;
                    newY = newY2;
                }

                tryPosition.Center = new Vector2(newX, newY);
            }

        }


    }

}
