using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{    
    public enum TextureFlipBehavior
    {
        None,
        FlipWhenFacingLeft,
        FlipWhenFacingRight,
        NextRowWhenFacingLeft,
        NextRowWhenRacingRight
    }

    public class Animation : IDisplayable, IUpdateable
    {
        public UpdatePriority Priority { get { return UpdatePriority.Behavior; } }

        public TextureDrawInfo DrawInfo { get; private set; }
      
        private Sprite Sprite;
        private IWithPosition Parent;
        public ConfigValue<TimeSpan> TimePerFrame { get; private set; }
        private int[] Frames;
        private CyclingInteger CurrentFrameIndex;
        private TimeSpan TimeRemainingInFrame;
       
        public IRemoveable Root { get; private set; }

        public bool Finished {  get { return CurrentFrameIndex.JustCycled; } }
         
        public TextureInfo Texture
        {
            get { return Sprite.Texture; }
            set { Sprite = Sprite.ChangeTexture(value); }
        }

        public void Reset()
        {
            CurrentFrameIndex.Reset();
        }

        public Animation(IRemoveable root, IWithPosition parent, Sprite sprite, params int[] frames)
        {
            Root = root;
            Parent = parent;
            TimePerFrame = new ConfigValue<TimeSpan>("animation frame");
            Sprite = sprite;
            Frames = frames;
            CurrentFrameIndex = new CyclingInteger(frames.Length);
            DrawInfo = sprite.DrawInfo;            
        }

        public int CurrentFrame
        {
            get
            {
                if (Frames.Length == 0)
                    return 0;

                return Frames[CurrentFrameIndex.Value];
            }
        }
        
        public Rectangle Position { get { return Parent.Position; } }
     
        public void Update(TimeSpan elapsedInFrame)
        {            
            TimeRemainingInFrame -= elapsedInFrame;

            if(TimeRemainingInFrame.TotalMilliseconds <= 0)
            {
                CurrentFrameIndex++;
                TimeRemainingInFrame = TimePerFrame.Value + TimeRemainingInFrame;
            }

            this.Sprite.Cell = this.CurrentFrame;
        }

        protected virtual int BeforeDraw(IRenderer painter, Sprite sprite, TextureDrawInfo drawInfo)
        {
            return Sprite.Cell;
        }

        public void Draw(IRenderer painter)
        {
            DrawInfo.FlipOffsetsOnly = false;
            DrawInfo.FlipX = false;
            DrawInfo.FlipY = false;

            int cell = BeforeDraw(painter, Sprite, DrawInfo);
            painter.DrawSprite(this, this.Sprite.Texture, cell);           
        }
    }

    public class DirectedAnimation : Animation
    {
        private IWithPositionAndDirection Parent;
        private TextureFlipBehavior FlipBehavior;

        public DirectedAnimation(IRemoveable root, IWithPositionAndDirection parent, Sprite sprite, TextureFlipBehavior flipbehavior, params int[] frames)
            :base(root,parent,sprite,frames)
        {
            Parent = parent;            
            FlipBehavior = flipbehavior;
        }

        protected override int BeforeDraw(IRenderer painter, Sprite sprite, TextureDrawInfo drawInfo)
        {
            int cell = sprite.Cell;

            DrawInfo.FlipOffsetsOnly = false;
            DrawInfo.FlipX = false;
            DrawInfo.FlipY = false;

            switch (FlipBehavior)
            {
                case TextureFlipBehavior.FlipWhenFacingLeft:
                    DrawInfo.FlipX = (Parent.Direction == Direction.Left);
                    break;
                case TextureFlipBehavior.FlipWhenFacingRight:
                    DrawInfo.FlipX = (Parent.Direction == Direction.Right);
                    break;
                case TextureFlipBehavior.NextRowWhenFacingLeft:
                    if (Parent.Direction == Direction.Left)
                    {
                        cell += sprite.Texture.Columns;
                        DrawInfo.FlipX = true;
                        DrawInfo.FlipOffsetsOnly = true;
                    }
                    break;
                case TextureFlipBehavior.NextRowWhenRacingRight:
                    if (Parent.Direction == Direction.Right)
                    {
                        cell += sprite.Texture.Columns;
                        DrawInfo.FlipX = true;
                        DrawInfo.FlipOffsetsOnly = true;
                    }
                    break;
            }


            return cell;
        }

    }

    public struct AnimationKey
    {
        public string Name { get; private set; }

        public AnimationKey(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AnimationSet : DisplayableSet<Animation>, IUpdateable
    {
        public UpdatePriority Priority { get { return UpdatePriority.Behavior; } }
        public Animation CurrentAnimation {  get { return Current; } }

        IRemoveable IUpdateable.Root => CurrentAnimation.Root;

        private bool animationChanged;
        private AnimationKey animationKey;

        public AnimationKey CurrentKey
        {
            get
            {
                return animationKey;
            }
            set
            {
                if (!this.ContainsKey(value.Name))
                    return;

                if (Key != value.Name)
                {
                    animationKey = value;
                    animationChanged = true;
                }
                Key = value.Name;
                
            }
        }

        public Animation Add<T>(AnimationKey key, T gameObject, TextureFlipBehavior flipBehavior, params int[] frames)
            where T:IRemoveable, IWithPositionAndDirection, IWithSprite
        {
            var anim = new DirectedAnimation(gameObject, gameObject, gameObject.Sprite, flipBehavior, frames);
            Add(key.Name, anim);
            return anim;
        }

        public Animation AddRange<T>(AnimationKey key, T gameObject, TextureFlipBehavior flipBehavior, int from, int to, int holdFrame=-1, int holdFor=2)
          where T : IRemoveable, IWithPositionAndDirection, IWithSprite
        {
            List<int> frames = Enumerable.Range(from, (to - from)).ToList();
            if(holdFrame > -1)
            {
                frames = frames.SelectMany(p => (p == holdFrame) ? Enumerable.Range(0,holdFor).Select(q=>p) : new int[] { p }).ToList();
            }

            return Add(key, gameObject, flipBehavior, frames.ToArray());
        }

        public AnimationSet(IDisplayable sprite) : base(sprite)
        {
        }

        public void Update(TimeSpan elapsedInFrame)
        {
            if(animationChanged)
            {
                animationChanged = false;
                Current.Reset();
            }

            Current.Update(elapsedInFrame);            
        }

        public bool IsPlaying(AnimationKey key)
        {
            return CurrentKey.Name == key.Name && !CurrentAnimation.Finished;
        }
    }
   
}
