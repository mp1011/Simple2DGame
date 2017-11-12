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
        public Vector2 MotionPerSecond { get; set; }
        
        public float DistancePerSecond
        {
            get { return MotionPerSecond.Length(); }
            set
            {
                MotionPerSecond = MotionPerSecond.SetLength(value);
            }
        }
        
        public double AngleInDegrees
        {
            get { return MotionPerSecond.GetDegrees(); }
            set
            {
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

        Vector2 IMotionAdjuster.AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 currentMotionPerSecond)
        {
            //todo, handle adding to existing motion
            return MotionPerSecond;   
        }
    }
}
