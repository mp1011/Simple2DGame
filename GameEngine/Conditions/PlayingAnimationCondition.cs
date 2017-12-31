using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class PlayingAnimationCondition : Condition
    {
        private AnimationSet Animations;
        private AnimationKey Key;
        private int FrameMin;
        private int FrameMax;

        public PlayingAnimationCondition(AnimationSet animations, AnimationKey key) : this(animations, key, 0, 9999) { }

        public PlayingAnimationCondition(AnimationSet animations, AnimationKey key, int frameMin, int frameMax)
        {
            Animations = animations;
            Key = key;
            FrameMin = frameMin;
            FrameMax = frameMax;
        }

        public override bool IsActive
        {
            get
            {
                if(Animations.CurrentKey.Equals(Key))
                {
                    var current = Animations.CurrentAnimation;
                    var ret = !current.Finished && current.CurrentFrame >= FrameMin && current.CurrentFrame <= FrameMax;
                    return ret;
                }

                return false;
            }
        }    
    }

    public static class PlayingAnimationExtension
    {
        public static Condition IsAnimationPlaying(this AnimationSet animation, AnimationKey key)
        {
            return new PlayingAnimationCondition(animation, key);
        }
    }
}
