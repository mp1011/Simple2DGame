using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    public class InterpolatedVector
    {
        public InterpolatedNumber X { get; private set; } = new InterpolatedNumber(0, 0, 0);
        public InterpolatedNumber Y { get; private set; } = new InterpolatedNumber(0, 0, 0);

        public InterpolatedNumber GetAxis(Axis a)
        {
            if (a == Axis.X)
                return X;
            else if (a == Axis.Y)
                return Y;
            else
                throw new InvalidOperationException();
        }

        public Vector2 Current => new Vector2(X.Current, Y.Current);
        public Vector2 Target => new Vector2(X.Target, Y.Target);

        public void Adjust(TimeSpan elapsedInFrame)
        {
            X.Adjust(elapsedInFrame);
            Y.Adjust(elapsedInFrame);
        }
    }

}
