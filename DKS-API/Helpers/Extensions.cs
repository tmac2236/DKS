using System;
using DFPS_API.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DFPS.API.Helpers
{
    public static class Extensions
    {
        //When Publish
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
        public static void AddPagination(this HttpResponse response,
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            //Convert "TitleCase" to "CamelCase"
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            //if when the date in the year have not reached.
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
        /// <summary>
        //  convert object to string ,
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 ""</returns>
        public static string ToString(this object value, string tempFormat)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return string.Format("{0:" + tempFormat + "}", value);

        }

        /// <summary>
        //  convert object to string ,
        //  dedault is 0  .e.g. Stocking_Fitting_Finished_Date.ToString("MM/dd/yyyy")
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static string ToSafetyString(this object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();

        }


        /// <summary>
        //  convert object to Byte , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 0</returns>
        public static byte ToByte(this object value)
        {
            if (value == null)
                return 0;
            byte result = 0;
            byte.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        //  convert object to SByte , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 0</returns>
        public static SByte ToSByte(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            sbyte result = 0;
            sbyte.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        //  convert object to Short , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static short ToShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            short result = 0;
            short.TryParse(value.ToString(), out result);
            return result;
        }


        /// <summary>
        //  convert object to UInt , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static uint ToUInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }


        /// <summary>
        //  convert object to Ushort , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static ushort ToUShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        //  convert object to Int , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static int ToInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            int result = 0;
            int.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        //  convert object to Float , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static float ToFloat(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            float result = 0;
            float.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        //  convert object to Double , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static double ToDouble(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            double result = 0;

            double.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        //  convert object to Long , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static long ToLong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            long result = 0;

            long.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        //  convert object to ULong , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static ulong ToULong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            ulong result = 0;

            ulong.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        //  convert object to Decimal , default is 0.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "0"</returns>
        public static decimal ToDecimal(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            decimal result = 0;

            decimal.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        //  convert object to Char , default is ''.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 ''</returns>
        public static char ToChar(this object value)
        {
            //Tối ưu hơn phân cách khi dùng hàm ||
            if (value == null || value.ToString() == string.Empty || (value.ToString().Length > 1))
            {
                return ' ';
            }
            char result = ' ';
            char.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        //  convert object to bool , default is false.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 false</returns>
        public static bool ToBool(this object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.ToInt() == 1)
            {
                return true;
            }

            bool result = false;
            bool.TryParse(value.ToString(), out result);
            return result;
        }
        /// <summary>
        //  convert object to Datetime , default is "1/1/0001 12:00:00 AM”.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 "1/1/0001 12:00:00 AM”</returns>
        public static DateTime ToDateTime(this object value)
        {
            if (value == null || value.ToString() == string.Empty || value.ToString() == " ")
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;

            DateTime.TryParse(value.ToString(), out result);

            return result;

        }
        /// <summary>
        //  convert object to Datetime , default is null.
        /// </summary>
        /// <param name="value">Object </param>
        /// <returns>Default value 為 null</returns>
        public static DateTime? ToDateTimeOrNull(this object value)
        {

            try
            {
                DateTime dt = DateTime.Parse(value.ToString());

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}