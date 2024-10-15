using BMS.BLL.Services.IServices;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;

namespace BMS.BLL.Services
{
    public class QRCodeService : IQRCodeService
    {
        /*public byte[] GenerateQRCode(string content)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeAsPng = qrCode.GetGraphic(20);
                return qrCodeAsPng;
            }
        }*/
        public string GenerateQRCode(string content)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeAsPng = qrCode.GetGraphic(20);

                // Tạo hash của QRCode để lưu vào database thay vì byte[]
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(qrCodeAsPng);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();  // Chuỗi hash
                }
            }
        }
    }

}
