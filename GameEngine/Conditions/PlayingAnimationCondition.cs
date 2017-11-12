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

        public PlayingAnimationCondition(AnimationSet animations, AnimationKey key)
        {
            Animations = animations;
            Key = key;
        }

        public override bool IsActive => Animations.IsPlaying(Key);
    }

    public static class PlayingAnimationExtension
    {
        public static ICondition ContinueWhileAnimationPlaying(this ICondition condition, AnimationSet animation, AnimationKey key)
        {
            return new ContinueWhileCondition(condition, new PlayingAnimationCondition(animation, key));
        }
    }
}
