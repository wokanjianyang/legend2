
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ItemConfigCategory
    {
        public string GetLogoId(int configId)
        {
            try
            {
                return Get(configId).LogoId;

            }
            catch (Exception ex)
            {
            }

            return "";
        }
    }
}