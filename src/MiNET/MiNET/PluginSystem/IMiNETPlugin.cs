using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.API
{
    public interface IMiNETPlugin
    {
        /// <summary>
        /// This function will be called on plugin initialization.
        /// </summary>
        void OnEnable();

        /// <summary>
        /// This function will be called when the plugin will be disabled.s
        /// </summary>
        void OnDisable();
    }
}
