using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace exercisesProject
{
    public class FormProvider
    {
        public static MainTab MainMenu
        {
            get
            {
                if (_mainMenu == null)
                {
                    _mainMenu = new MainTab();
                }
                return _mainMenu;
            }
        }
        private static MainTab _mainMenu;
    }
}
