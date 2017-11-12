using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNARec = Microsoft.Xna.Framework.Rectangle;
using XNAColor = Microsoft.Xna.Framework.Color;
using System.IO;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    /// <summary>
    /// Draws IDisplayables to the screen
    /// </summary>
    public interface IRenderer
    {
        Boundary ScreenBounds { get; }
        Layer CurrentLayer { get; set; }
        void DrawSpriteGrid(IDisplayable item, SpriteGrid grid);
        void DrawSprite(IDisplayable item, TextureInfo texture, int cell);
        void DrawRectangle(IDisplayable displayable, XNAColor color);
    }

    class SpriteBatchRenderer : IRenderer
    {
        public Layer CurrentLayer { get; set; }

        private SpriteBatch spriteBatch;
        private XNATextureCollection textures;
         
        public SpriteBatchRenderer(SpriteBatch batch, XNAGameEngine engine, int ScreenWidth, int ScreenHeight)
        {
            spriteBatch = batch;
            textures = new XNATextureCollection(engine);
            ScreenBounds = new Boundary(new AbstractPosition());
            ScreenBounds.Position.Set(new Rectangle(0, 0, ScreenWidth, ScreenHeight));
        }
        
        public Boundary ScreenBounds { get; set; }

        public void DrawSpriteGrid(IDisplayable displayable, SpriteGrid grid)
        {
            if (displayable == null || grid == null || displayable.DrawInfo.Visible==false)
                return;

            Texture2D xnaTec = GetTexture(grid.Texture.ID);

            XNAColor color = XNAColor.White;

            int drawIndex = 0;
            var topLeft = GetScreenPoint(displayable);

            foreach (var tile in grid.Cells)
            {
                XNARec src = GetTextureCell(grid.Texture, tile);
                XNARec dest = new XNARec((int)topLeft.X, (int)topLeft.Y, src.Width, src.Height);

                dest.Offset(drawIndex.ToXY(grid.Cells.Columns).Scale(src.Size));
                spriteBatch.Draw(xnaTec, dest, src, color);

                drawIndex++;
            }
        }

        private Vector2 GetScreenPoint(IDisplayable displayable)
        {
            return displayable.Position.UpperLeft.Translate(-1 * ScreenBounds.Position.UpperLeft.Scale(CurrentLayer.ScrollingCoefficient));          
        }

        public void DrawSprite(IDisplayable displayable, TextureInfo texture, int cell)
        {
            var drawInfo = displayable.DrawInfo;

            if (displayable == null || texture == null || !drawInfo.Visible)
                return;

             Texture2D xnaTec = GetTexture(texture.ID);

            XNAColor color = XNAColor.White;

            int drawIndex = 0;
            var topLeft = GetScreenPoint(displayable);
           
            XNARec src = GetTextureCell(texture, cell);
            XNARec dest = new XNARec((int)topLeft.X, (int)topLeft.Y, src.Width, src.Height);

            Rectangle dest1 = new Rectangle(topLeft.X, topLeft.Y, displayable.Position.Width, displayable.Position.Height);
            Rectangle dest2 = new Rectangle(0,0, src.Width, src.Height);

            switch(texture.AnchorOrigin)
            {
                case AnchorOrigin.BottomCenter:
                    dest2.BottomCenter = dest1.BottomCenter;
                    break;
                case AnchorOrigin.Center:
                    dest2.Center = dest1.Center;
                    break;
                default:
                   throw new NotImplementedException();
            }

            if (texture.CellAnchorOffsets != null)
                dest2.Translate(texture.CellAnchorOffsets.GetValues()[cell].Flip(drawInfo.FlipX, drawInfo.FlipY));

            var dest3 = new XNARec((int)dest2.Left, (int)dest2.Top, (int)dest2.Width, (int)dest2.Height);

        
            SpriteEffects flip = SpriteEffects.None;
            if (!drawInfo.FlipOffsetsOnly)
            {
                if (drawInfo.FlipX)
                    flip = flip | SpriteEffects.FlipHorizontally;
                if (drawInfo.FlipY)
                    flip = flip | SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(xnaTec, dest3, src, color, 0, Vector2.Zero, flip, 0);

            drawIndex++;
        }
        
        private XNARec GetTextureCell(TextureInfo texture, int cell)
        {
            Vector2 xy = cell.ToXY(texture.Columns).Scale(texture.CellSize);       
            return new XNARec((int)(texture.Origin.X + xy.X), (int)(texture.Origin.Y + xy.Y), (int)texture.CellSize.X, (int)texture.CellSize.Y);
        }
        
        private Texture2D GetTexture(string id)
        {
            return textures.Get(id);
        }

      
        public void DrawRectangle(IDisplayable displayable, XNAColor color)
        {
            Texture2D xnaTec = GetTexture("pixel");
            
            var topLeft = GetScreenPoint(displayable);

            XNARec src = new XNARec(0, 0, 1, 1);
            XNARec dest = new XNARec((int)topLeft.X, (int)topLeft.Y, (int)displayable.Position.Width, (int)displayable.Position.Height);
            
            if(displayable.DrawInfo.Opacity < 1.0f)
            {
                color = color * displayable.DrawInfo.Opacity;
            }
            
            spriteBatch.Draw(xnaTec, dest, src, color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }

   
    class XNATextureCollection
    {
        private XNAGameEngine engine;
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public XNATextureCollection(XNAGameEngine gameEngine)
        {
            engine = gameEngine;
        }
        
        public Texture2D Get(string id)
        {
            Texture2D ret;
            if (!textures.TryGetValue(id, out ret))
            {
                ret = TextureInfoReader.Instance.Load(id);
                textures.Add(id, ret);               
            }

            return ret;
        }

    }

    
}
