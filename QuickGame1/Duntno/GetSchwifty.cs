using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1.Duntno
{

    public abstract class BitBlock
    {
        public int BitLength { get; private set; }
        public int PossibleValues { get { return (int)Math.Pow(2, BitLength); } }

        public BitBlock(int bitLength)
        {
            BitLength = bitLength;
        }
    }

    public abstract class BitBlock<T> : BitBlock
    {
        public abstract T Value { get; set; }

        protected BitBlock(int bitLength) : base(bitLength) { }
    }

    public class Flag : BitBlock<Boolean>
    {
        public override bool Value { get; set; }
        public Flag() : base(1) { }
    }

    public class ByteBlock : BitBlock<byte>
    {
        public override byte Value { get; set; }
        public ByteBlock() : base(8) { }
    }

    public class Block : BitBlock<CyclingInteger>
    {
        public override CyclingInteger Value { get; set; }

        public Block(int bitLength) : base(bitLength)
        {
        }
    }

    public class Structure : BitBlock
    {
        private BitBlock[] Blocks;
        
        public Structure(params BitBlock[] blocks) : base(blocks.Sum(p=>p.BitLength))
        {
            Blocks = blocks.ToArray();
        }
    }

    public class ObjectTypeDef
    {

    }

    public class Sprite
    {
        public Block ID = new Block(4);
        public Block XPos = new Block(8);
        public Block YPos = new Block(8);
        public Block Screen = new Block(3);
        public Flag Flipped = new Flag();
        public Block AnimationFrame = new Block(3);
        
        public BitBlock GetStruct()
        {
            return new Structure(ID, XPos, YPos, Screen, Flipped, AnimationFrame);
        }
    }


   
}
