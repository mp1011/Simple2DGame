using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    static class Textures
    {


        private static TextureInfo cursorTexture;
        public static TextureInfo CursorTexture
        {
            get
            {
                return cursorTexture ?? (cursorTexture = new TextureInfo
                {
                    ID = "bullets",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(122, 267),
                    Size = new Vector2(520, 361),
                    AnchorOrigin = AnchorOrigin.Center
                });
            }
        }

        private static TextureInfo slimeTexture;
        public static TextureInfo SlimeTexture
        {
            get
            {
                return slimeTexture ?? (slimeTexture = new TextureInfo
                {
                    ID = "HorrorSlime",
                    CellSize = new Vector2(64, 32),
                    Origin = new Vector2(0, 0),
                    Size = new Vector2(128, 128)
                });
            }
        }

        private static TextureInfo fairyTexture;
        public static TextureInfo FairyTexture
        {
            get
            {
                return fairyTexture ?? (fairyTexture = new TextureInfo
                {
                    ID = "FairyA",
                    CellSize = new Vector2(25, 27),
                    Origin = new Vector2(0, 0),
                    Size = new Vector2(75, 27)
                });
            }
        }


        private static TextureInfo movingBlockTexture;
        public static TextureInfo MovingBlockTexture
        {
            get
            {
                return movingBlockTexture ?? (movingBlockTexture = new TextureInfo
                {
                    ID = "Items",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(64, 48),
                    Size = new Vector2(176, 112)
                });
            }
        }

        private static TextureInfo bulletTexture;
        public static TextureInfo BulletTexture
        {
            get
            {
                return bulletTexture ?? (bulletTexture = new TextureInfo
                {
                    ID = "bullets",
                    CellSize = new Vector2(18, 18),
                    Size = new Vector2(520, 361),
                    Origin = new Vector2(136, 6),
                    AnchorOrigin = AnchorOrigin.Center
                });
            }
        }

        private static TextureInfo bigFontTexture;
        public static TextureInfo BigFontTexture
        {
            get
            {
                return bigFontTexture ?? (bigFontTexture = new TextureInfo
                {
                    ID = "BigFont",
                    CellSize = new Vector2(18, 16),
                    Size = new Vector2(162, 128)
                });
            }
        }

        private static TextureInfo smallFontTexture;
        public static TextureInfo SmallFontTexture
        {
            get
            {
                return smallFontTexture ?? (smallFontTexture = new TextureInfo
                {
                    ID = "SmallFont",
                    CellSize = new Vector2(13, 12),
                    Size = new Vector2(117, 96)
                });
            }
        }

        private static TextureInfo swooshTexture;
        public static TextureInfo SwooshTexture
        {
            get
            {
                return swooshTexture ?? (swooshTexture = new TextureInfo
                {
                    ID = "swoosh",
                    CellSize = new Vector2(32, 32),
                    Size = new Vector2(128, 32)
                });
            }
        }

        private static TextureInfo border;
        public static TextureInfo Border
        {
            get
            {
                return border ?? (border = new TextureInfo
                {
                    ID = "border",
                    CellSize = new Vector2(8, 8),
                    Size = new Vector2(24, 24)
                });

            }
        }

        private static TextureInfo heartTexture;
        public static TextureInfo HeartTexture
        {
            get
            {
                return heartTexture ?? (heartTexture = new TextureInfo
                {
                    ID = "heart",
                    CellSize = new Vector2(16, 16),
                    Size = new Vector2(48, 16)
                });
            }
        }


        private static TextureInfo smallHeartTexture;
        public static TextureInfo SmallHeartTexture
        {
            get
            {
                return smallHeartTexture ?? (smallHeartTexture = new TextureInfo
                {
                    ID = "smallheart",
                    CellSize = new Vector2(8, 8),
                    Size = new Vector2(8, 8)
                });
            }
        }

        private static TextureInfo boxTexture;
        public static TextureInfo BoxTexture
        {
            get
            {
                return boxTexture ?? (boxTexture = new TextureInfo
                {
                    ID = "sheet",
                    CellSize = new Vector2(12, 12),
                    Size = new Vector2(12, 12),
                    Origin = new Vector2(130, 66)
                });
            }
        }

        private static TextureInfo breakableBlockTexture;
        public static TextureInfo BrokenBlockTexture
        {
            get
            {
                return breakableBlockTexture ?? (breakableBlockTexture = new TextureInfo
                {
                    ID = "sheet",
                    CellSize = new Vector2(16, 16),
                    Size = new Vector2(16, 16),
                    Origin = new Vector2(257, 112)
                });
            }
        }

        private static TextureInfo rockTiles;
        public static TextureInfo RockTiles
        {
            get
            {
                return rockTiles ?? (rockTiles = new TextureInfo
                {
                    ID = "sheet",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(112, 0),
                    Size = new Vector2(224, 128)
                });
            }
        }

        private static TextureInfo grassTiles;
        public static TextureInfo GrassTiles
        {
            get
            {
                return grassTiles ?? (grassTiles = new TextureInfo
                {
                    ID = "sheet",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(144, 64),
                    Size = new Vector2(224, 128)
                });
            }
        }

        private static TextureInfo coin;
        public static TextureInfo CoinTexture
        {
            get
            {
                return coin ?? (coin = new TextureInfo
                {
                    ID = "Items",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(96, 80),
                    Size = new Vector2(176, 112)
                });
            }
        }

        private static TextureInfo poof;
        public static TextureInfo PoofTexture
        {
            get
            {
                return poof ?? (poof = new TextureInfo
                {
                    ID = "Items",
                    CellSize = new Vector2(16, 16),
                    Origin = new Vector2(16, 16),
                    Size = new Vector2(176, 112)
                });
            }
        }

        private static TextureInfo splash;
        public static TextureInfo SplashTexture
        {
            get
            {
                return splash ?? (splash = new TextureInfo
                {
                    ID = "splash",
                    CellSize = new Vector2(32, 32),
                    Origin = new Vector2(0, 0),
                    Size = new Vector2(128, 32)
                });
            }
        }

        private static TextureInfo kingTexture;
        public static TextureInfo KingTexture
        {
            get
            {
                return kingTexture = new TextureInfo
                {
                    ID = "characters_7",
                    Size = new Vector2(736, 32),
                    Origin = new Vector2(0, 32),
                    CellSize=new Vector2(32,32),
                    AnchorOrigin = AnchorOrigin.BottomCenter                    
                };
            }
        }

        private static TextureInfo elfTexture;
        public static TextureInfo ElfTexture
        {
            get
            {
                return elfTexture = new TextureInfo
                {
                    ID = "characters_7",
                    Size = new Vector2(736, 32),
                    Origin = new Vector2(0, 64),
                    CellSize = new Vector2(32, 32),
                    AnchorOrigin = AnchorOrigin.BottomCenter
                };
            }
        }

        private static TextureInfo grapemanTexture;
        public static TextureInfo GrapemanTexture
        {
            get
            {
                return grapemanTexture = new TextureInfo
                {
                    ID = "characters_7",
                    Size = new Vector2(736, 32),
                    Origin = new Vector2(0, 0),
                    CellSize = new Vector2(32, 32),
                    AnchorOrigin = AnchorOrigin.BottomCenter
                };
            }
        }

        private static TextureInfo treeTexture;
        public static TextureInfo TreeTexture
        {
            get
            {
                return treeTexture = new TextureInfo
                {
                    ID = "sheet",
                    Size = new Vector2(272, 128),
                    Origin = new Vector2(32, 0),
                    CellSize = new Vector2(64, 128),
                    AnchorOrigin = AnchorOrigin.BottomCenter
                };
            }
        }

        private static TextureInfo snakeTexture;
        public static TextureInfo SnakeTexture
        {
            get
            {
                return snakeTexture ?? (snakeTexture = new TextureInfo
                {
                    ID = "characters_7",
                    Size = new Vector2(736, 32),
                    Origin = new Vector2(0, 98),
                    CellSize = new Vector2(32, 32),
                    AnchorOrigin = AnchorOrigin.BottomCenter
                });
            }
        }
    }
}
