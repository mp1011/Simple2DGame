using GameEngine;

namespace QuickGame1
{
    public static class Recoil
    {

        public static void RecoilFrom<TObject,TOther>(this TObject obj, TOther other)
            where TObject : IMoveable
            where TOther : IWithPositionAndDirection
        {
            var x = Config.ReadValue<AxisMotionConfig>("recoilX");
            var y = Config.ReadValue<AxisMotionConfig>("recoilY");

            if(other.Direction != Direction.None)
                obj.Direction = other.Direction.Opposite();

            obj.Motion.AdjustImmediately(v =>
            {
                v.X.Current = x.GetStartSpeed(obj);
                v.Y.Current = y.GetStartSpeed(obj);
            });            
        }
    }
}
