using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    /// <summary>
    /// Moves an object every frame
    /// </summary>
    public class MotionManager : IUpdateable
    {
        public Rectangle FrameStartPosition { get; private set; } = new Rectangle();

        private IMovingWorldObject ObjectToMove;
        UpdatePriority IUpdateable.Priority => UpdatePriority.Motion;
        IRemoveable IUpdateable.Root => ObjectToMove;

        private InterpolatedVector MotionVector { get; } = new InterpolatedVector();
        
        public Vector2 FrameVelocity { get; private set; } = Vector2.Zero;

        public Vector2 TargetMotionPerSecond => MotionVector.Target;
        public Vector2 CurrentMotionPerSecond => MotionVector.Current;

        private List<MotionAdjuster> MotionAdjusters = new List<MotionAdjuster>();

        private List<MotionMultiplier> MotionMultipliers = new List<MotionMultiplier>();

        public void AddMultiplier(MotionMultiplier m)
        {
            MotionMultipliers.Add(m);
        }

        public void AdjustImmediately(Action<InterpolatedVector> action)
        {
            action(MotionVector);
        }

        public void AdjustImmediately(MotionAdjuster adjuster)
        {
            adjuster.Update(ObjectToMove, MotionVector);
        }

        public MotionManager(IMovingWorldObject objectToMove)
        {
            ObjectToMove = objectToMove;
            ObjectToMove.Layer.Scene.AddObject(this);
        }

        public void Stop(Axis axis, bool setTarget=false)
        {
            MotionVector.GetAxis(axis).Current = 0;

            if (axis == Axis.X)
                ObjectToMove.Position.SetLeft(FrameStartPosition.Left);
            else if (axis == Axis.Y)
                ObjectToMove.Position.SetTop(FrameStartPosition.Top);

            if (setTarget)
            {
                MotionVector.GetAxis(axis).SetTarget(0, 0);
            }
        }

        public void AddAdjuster(MotionAdjuster adjuster)
        {
            MotionAdjusters.Add(adjuster);
        }
        
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            FrameStartPosition = ObjectToMove.Position.Copy();

            foreach (var adjuster in MotionAdjusters)
                adjuster.Update(ObjectToMove,MotionVector);

       
            foreach(var multiplier in MotionMultipliers)
            {
                MotionVector.X.TargetScale = multiplier.GetTargetMod();
                MotionVector.X.DeltaScale = multiplier.GetDeltaMod();
                MotionVector.Y.TargetScale = multiplier.GetTargetMod();
                MotionVector.Y.DeltaScale = multiplier.GetDeltaMod();
            }

            MotionVector.Adjust(elapsedInFrame);

            
            var motion = CurrentMotionPerSecond;

            FrameVelocity = CurrentMotionPerSecond.Scale(elapsedInFrame.TotalSeconds);
            ObjectToMove.Position.Translate(FrameVelocity);
        }

        /// <summary>
        /// Translates both the current and frame start positions
        /// </summary>
        /// <param name="adjustment"></param>
        public void CorrectPosition(Vector2 adjustment)
        {
            ObjectToMove.Position.Translate(adjustment);
            FrameStartPosition.Translate(adjustment);
        }
    }
}
