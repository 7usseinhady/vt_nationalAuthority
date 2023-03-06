using apiWSDLs.ServiceReference1;
using System;

namespace apiWSDLs.wsdls
{
    public class sms
    {
        /// <summary>
        ///   Send SMS For Special Phone.
        /// </summary>
        /// <param name="message"> Message Will Send. </param>
        /// <param name="phoneNumber"> Phone Number. </param>
        /// <returns> Int. </returns>
        public int sendSMS(string message, string phoneNumber)
        {
            int smsResult = -115;
            try
            {
                ServiceSoapClient sms = new ServiceSoapClient();
                smsResult = sms.SendSMSWithDLR("Social Security Dev", "9ad1O9S9x9", message, "A", "Insurance", phoneNumber);
            }
            catch
            {
            }

            return smsResult;
        }

    }
}