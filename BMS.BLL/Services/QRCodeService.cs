using BMS.BLL.Services.IServices;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BMS.BLL.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string content)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeAsPng = qrCode.GetGraphic(20);
                return qrCodeAsPng;
            }
        }
    }

}
