using System;
using System.Collections.Generic;
using System.Text;

namespace SysDiaPulseCore.Common.Filtres
{
    public interface IFilter
    {
        /// <summary>
        /// Применить данный фильтр
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public void Apply(ref byte[,,] image);
    }
}
