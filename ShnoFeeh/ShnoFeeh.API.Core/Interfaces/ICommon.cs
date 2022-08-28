using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface ICommon
    {
        string GetUri(string uri);
        string GetTapUri(string uri);
        string GetWebUri(string uri);
        string GetUriWithoutBase(string uri);
    }
}
