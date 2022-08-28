using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Customer Address class.
    /// </summary>
    public class CustomerAddress
    {
        #region Properties
        public string Block { get; set; }
        public string Street { get; set; }
        public string HouseBuildingNo { get; set; }
        public string Address { get; set; }
        public string AddressInstructions { get; set; }
        #endregion

    }
}
