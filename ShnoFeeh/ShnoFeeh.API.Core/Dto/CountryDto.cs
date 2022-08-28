namespace ShnoFeeh.API.Core.Dto
{
    /*
      This class contains data transfer object
      properties for Country functionality
   */
    public class CountryDto
    {
        #region Properties
        public int Id { get; set; }
        public string Country { get; set; }
        public string CountryAr { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        #endregion
    }
    /*
     This class contains data transfer object
     properties for Update Country functionality
  */
    public class UpdateCountryDto
    {
        #region Properties
        public int CountryId { get; set; }
        public string CountryAr { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
