using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.Magics
{
    public class MagicWandSwitch
    {
        IMagicWandHolder User;
        IMagicWand Last;
        public MagicWandSwitch(IMagicWandHolder user)
        {
            User = user;
        }
        public bool Fire()
        {
            return Last != null && Last.Fire();
        }
        public void Hold(IMagicWand magicWand)
        {
            if (magicWand != Last)
            {
                Last?.Release();
                Last = magicWand;
                Last?.Hold(User);
            }
        }
    }
}
