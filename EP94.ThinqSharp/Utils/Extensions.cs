using EP94.ThinqSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EP94.ThinqSharp.Utils
{
    internal static class Extensions
    {
        public static Dictionary<string, string?> ToDictionary(this object? arg)
        {
            if (arg is null)
            {
                return new Dictionary<string, string?>();
            }
            if (arg is Dictionary<string, string?> dictionary)
            {
                return dictionary;
            }
            return arg.GetType().GetProperties().ToDictionary(property => GetPropertyName(property), property => property.GetValue(arg)?.ToString());
        }

        public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, AsymmetricKeyParameter privateKey)
        {
            RSA rsa;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                rsa = DotNetUtilities.ToRSA(privateKey as RsaPrivateCrtKeyParameters);
            }
            else
            {
                RSAParameters parameters = DotNetUtilities.ToRSAParameters(privateKey as RsaPrivateCrtKeyParameters);
                rsa = RSA.Create();
                rsa.ImportParameters(parameters);
            }

            return RSACertificateExtensions.CopyWithPrivateKey(certificate, rsa);
        }

        public static string EncodeSHA512(this string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] result;
            using SHA512 sha = SHA512.Create();
            result = sha.ComputeHash(data);
            return Convert.ToHexString(result).ToLower();
        }

        /// <summary>
        /// Converts a regular DateTime to a RFC822 date string.
        /// </summary>
        /// <param name="date">DateTime object to convert.</param>
        /// <returns>The specified date formatted as a RFC822 date string.</returns>
        public static string ToRFC822Date(this DateTime date)
        {
            return date.ToString("ddd',' d MMM yyyy HH':'mm':'ss", CultureInfo.InvariantCulture)
                + " "
                + date.ToString("zzzz").Replace(":", "");
        }

        public static T ToObject<T>(this Dictionary<string, object> source) where T : class, new()
        {
            T result = new T();
            Type resultType = result.GetType();

            foreach (var item in source)
            {
                resultType
                         .GetProperty(item.Key)?
                         .SetValue(result, item.Value, null);
            }

            return result;
        }


        private static string GetPropertyName(PropertyInfo propertyInfo)
        {
            JsonPropertyAttribute? jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
            if (jsonPropertyAttribute is not null && jsonPropertyAttribute.PropertyName is not null)
            {
                return jsonPropertyAttribute.PropertyName;
            }
            return propertyInfo.Name;
        }
    }
}
