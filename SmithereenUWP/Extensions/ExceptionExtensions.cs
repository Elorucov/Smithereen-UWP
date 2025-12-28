using SmithereenUWP.API;
using SmithereenUWP.Core;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web;

namespace SmithereenUWP.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToHEX(this Exception ex) => $"0x{ex.HResult.ToString("x8")}";

        public static Tuple<string, string> ToUnderstandableInfo(this Exception ex)
        {
            Tuple<string, string> result = new Tuple<string, string>(String.Empty, String.Empty);
            if (ex is SmithereenAPIException apiEx)
            {
                string message = null;
                switch (apiEx.Code)
                {
                    case SmithereenAPI.ERROR_INVALID_RESPONSE:
                        message = "The server returns a response in the wrong format";
                        break;
                    case SmithereenAPI.ERROR_DOMAIN_NOT_RESOLVED:
                        message = "The domain you entered could not be resolved";
                        break;
                    default:
                        message = apiEx.Message;
                        break;
                }
                result = new Tuple<string, string>(Locale.Get("error_api"), message);
            }
            else if (ex is System.Net.Http.HttpRequestException httpex)
            {
                WebErrorStatus werror = WebError.GetStatus(httpex.HResult);
                string terr = Locale.Get("error_network");
                string nerr = string.Empty;
                switch (werror)
                {
                    default: nerr = $"{Locale.Get("error_network_general")}\n({werror})"; break;
                    case WebErrorStatus.CertificateCommonNameIsIncorrect: nerr = Locale.Get("error_network_ssl_hostname"); break;
                    case WebErrorStatus.CertificateIsInvalid:
                    case WebErrorStatus.CertificateContainsErrors:
                        terr = Locale.Get("error_network_ssl");
                        nerr = Locale.Get("error_network_ssl_certerror");
                        break;
                    case WebErrorStatus.CertificateExpired:
                        terr = Locale.Get("error_network_ssl");
                        nerr = Locale.Get("error_network_ssl_expired");
                        break;
                    case WebErrorStatus.CertificateRevoked:
                        terr = Locale.Get("error_network_ssl");
                        nerr = Locale.Get("error_network_ssl_revoked");
                        break;
                    case WebErrorStatus.ConnectionAborted: nerr = Locale.Get("error_network_aborted"); break;
                    case WebErrorStatus.RequestTimeout: nerr = Locale.Get("error_network_timeout"); break;
                    case WebErrorStatus.HostNameNotResolved: nerr = Locale.Get("error_network_no_connection"); break;
                }
                result = new Tuple<string, string>(terr, nerr);
            }
            else
            {
                result = new Tuple<string, string>(Locale.Get("error"), $"{ex.Message.Trim()} ({ex.ToHEX()})");
            }
            return result;
        }

        public static async Task ShowAsync(this Exception ex)
        {
            var info = ex.ToUnderstandableInfo();
            await new MessageDialog(info.Item2, info.Item1).ShowAsync();
        }
    }
}
