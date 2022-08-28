using System;
using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.API.Core.Dto
{
    /*
        This class contains data transfer object
        properties for Category functionality
    */
    public class CategoryDto
    {
        #region Properties
        public int Id { get; set; }
        public string CatName { get; set; }
        public string City { get; set; }
        public string Keywords { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Logo { get; set; }
        public string CategoryAr { get; set; }
        #endregion
    }
    /*
        This class contains data transfer object
        properties for Add Category functionality
    */
    public class AddCategoryDto
    {
        #region Properties
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string CatName { get; set; }
        public string CategoryAr { get; set; }
        public int? CityId { get; set; }
        [Required]
        public string Keywords { get; set; }
        public int? CreatedBy { get; set; }
        public string Logo { get; set; }
        #endregion
    }
    /*
       This class contains data transfer object
       properties for Update Category functionality
   */
    public class UpdateCategoryDto
    {
        #region Properties
        public int CategoryId { get; set; }
        public string CatName { get; set; }
        public string CategoryAr { get; set; }
        public int? CityId { get; set; }
        public string Keywords { get; set; }
        public int? ModifiedBy { get; set; }
        public string Logo { get; set; }
        #endregion
    }
}
