using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class XYMotion : IMotionAdjuster
    {
        private Vector2 _mps = Vector2.Zero;

        public Vector2 MotionPerSecond
        {
            get { return _mps; }
            set
            {
                _mps = value;
                applied = false;
            }
        }
        
        public float DistancePerSecond
        {
            get { return MotionPerSecond.Length(); }
            set
            {
                applied = false;
                MotionPerSecond = MotionPerSecond.SetLength(value);
            }
        }
        
        public double AngleInDegrees
        {
            get { return MotionPerSecond.GetDegrees(); }
            set
            {
                applied = false;
                MotionPerSecond = MotionPerSecond.SetDegrees(value);
            }
        }
    
        public XYMotion(string name, IMoveable objectToMove)
        {
            _name = name;
            objectToMove.Motion.Forces.Add(this);     
        }

        private string _name;
        string IMotionAdjuster.Name => _name;

        bool IMotionAdjuster.Active { get; set; } = true;

        private bool applied = false;
        Vector2 IMotionAdjuster.AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 currentMotionPerSecond)
        {
            if (applied)
                return currentMotionPerSecond;

            applied = true;
            return MotionPerSecond;
        }
    }
}
