using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiNationalAuthority.Models
{
    public class GeneralMethods
    {
        /// <summary>
        ///   Concatinate List Of String to String.
        /// </summary>
        /// <param name="sStr"> List Of String. </param>
        /// <param name="cCh"> Seperate Char. </param>
        /// <returns> String. </returns>
        public string sConcatString(string[] sStr, char cCh)
        {
            string sConcatStr = null;
            if (sStr != null)
            {
                sConcatStr = String.Join(",", sStr);
            }
            return sConcatStr;
        }

        /// <summary>
        ///    Split String To List Of string.
        /// </summary>
        /// <param name="sStr"> String. </param>
        /// <param name="cCh"> Seperate Char. </param>
        /// <returns> List Of String. </returns>
        public List<string> lSplitString(string sStr, char cCh)
        {
            return sStr.Split(cCh).ToList();
        }
    }
}