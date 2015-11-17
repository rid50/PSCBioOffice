using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSourceServices
{
    abstract class DataSource
    {
        public enum IMAGE_TYPE
        {
            picture = 0,
            wsq = 1
        }

        public abstract byte[][] GetImage(IMAGE_TYPE imageType, int id);
    }
}
