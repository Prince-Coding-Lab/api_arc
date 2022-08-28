using System.Collections.Generic;

namespace ShnoFeeh.API.Core.Dto
{
    /*
      This class contains data transfer object
      properties for Email Address.
   */
    public class EmailAddressDto
    {
        #region Properties
        public List<string> To { get; set; }
        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string MessageBody { get; set; }
        public string SmtpClient { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public int SmtpPort { get; set; }
        #endregion

    }
    /*
     This class contains 
     properties for Email Address.
    */
    public class EmailAddress
    {
        #region Properties
        public string Name { get; set; }
        public string Address { get; set; }
        #endregion
    }
}
