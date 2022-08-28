namespace ShnoFeeh.API.Core.Entities
{
    /*
      This class contains properties of database entity Countries
   */
    public class Countries
    {
        #region Properties
        public int Id { get; set; }
        public string Country { get; set; }
        public string CountryAr { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
