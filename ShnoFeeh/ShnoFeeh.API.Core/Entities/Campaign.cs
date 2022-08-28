namespace ShnoFeeh.API.Core.Entities
{

    /*
     This class contains properties of database entity Campaign
   */
    public class Campaign : BaseEntity
    {
        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public int? UserId { get; set; }
        #endregion

    }
}
