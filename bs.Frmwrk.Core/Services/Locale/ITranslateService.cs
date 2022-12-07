using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Services.Locale
{
    /// <summary>
    /// The service for translate messages
    /// </summary>
       public interface ITranslateService
    {
        //TODO: Ricorda di registrare nel bootstrap questo servizio per le traduzioni

        /// <summary>
        /// Translates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string Translate(string text);

        /// <summary>
        /// Translates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        string Translate(string text, params object[] objs);
    }
}
