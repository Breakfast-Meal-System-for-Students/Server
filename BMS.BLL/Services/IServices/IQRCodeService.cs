﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string content);
    }
}