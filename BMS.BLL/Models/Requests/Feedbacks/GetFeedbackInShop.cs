using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Feedbacks
{
    public class GetFeedbackInShop : PagingRequest
    {
        public int? Rating { get; set; } = 0;
    }
}
