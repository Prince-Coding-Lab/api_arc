namespace ShnoFeeh.API.Core.Entities
{
    /*
        This class contains properties of database entity Categories
   */
    public class Categories : BaseEntity
    {
        #region Properties
        public string CatName { get; set; }
        public string CategoryAr { get; set; }
        public int? CityId { get; set; }
        public string Keywords { get; set; }
        public string Logo { get; set; }
        #endregion
    }
}
