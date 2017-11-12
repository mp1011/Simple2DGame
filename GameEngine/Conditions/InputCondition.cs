using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class InputCondition : Condition
    {
        private InputKey Button;
        private IGameInput Input;

        public override bool IsActive { get => Input.GetButtonPressed(Button); }
       
        public InputCondition(InputKey button, IGameInput input)
        {
            Button = button;
            Input = input;             
        }
    }

    public class AnyButtonPressedCondition : Condition
    {
        private InputKey Button;
        private IGameInput Input;

        public override bool IsActive => Input.GetPressedButton() != null; 

        public AnyButtonPressedCondition(IGameInput input)
        {
            Input = input;
        }
    }

}
