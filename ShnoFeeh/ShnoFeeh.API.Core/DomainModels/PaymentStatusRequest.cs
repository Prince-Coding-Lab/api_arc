using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Payment Status Request class.
    /// </summary>
    public class PaymentStatusRequest
    {
        #region Properties
        public string Key { get; set; }
        public string KeyType { get; set; }
        #endregion
    }
}
