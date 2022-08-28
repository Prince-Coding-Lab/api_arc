using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Common
{
    public class CommonHelper
    {
        public static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public string GetJsonString(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, MicrosoftDateFormatSettings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsSupportedContentType_Files(string ContentType)
        {

            switch (ContentType)
            {

                case "application/pdf":
                    return true;

                case "application/msword":
                    return true;

                case "application/vnd.openxmlformats-":
                case "officedocument.wordprocessingml.document":
                    return true;

                case "text/html":
                    return true;

                default:
                    return false;
            }
        }

        public bool IsSupportedContentType_Images(string ContentType)
        {

            switch (ContentType)
            {

                case "image/png":
                    return true;

                case "image/jpeg":
                    return true;

                case "image/gif":
                    return true;
                case "application/octet-stream":
                    return true;

                default:
                    return false;
            }
        }
    }


}
