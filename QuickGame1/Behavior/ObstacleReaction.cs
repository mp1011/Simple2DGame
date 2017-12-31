using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    enum ObstacleReaction
    {
        None,
        KeepGoing,
        TurnAround,
        Stop,
        ShortJump,
        LongJump
    }

    class ObstacleReactions
    {
        public ObstacleReaction WalkingOffLedge;
        public ObstacleReaction WalkingOffDangerousLedge;

        public ObstacleReaction WalkingIntoWall;

        private ObstacleReaction _walkingIntoShortWall;
        public ObstacleReaction WalkingIntoShortWall
        {
            get
            {
                if (_walkingIntoShortWall == ObstacleReaction.None)
                    return WalkingIntoWall;
                else
                    return _walkingIntoShortWall;
            }
            set
            {
                _walkingIntoShortWall = value;
            }
        } 
    }
}
