namespace ShnoFeeh.API.Core.Dto
{
    /*
      This class contains data transfer object
      properties for VerificationToken functionality
    */
    public class VerificationTokenDto
    {
        #region Properties
        public int UserID { get; set; }
        public string Token { get; set; }
        public string Purpose { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        #endregion

    }
}
