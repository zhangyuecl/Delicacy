﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class VerifyRegisterTsfer
    {
        public string GUID { get; set; }
        public string LoginId { get; set; }
        public bool IsUsed { get; set; }
        public System.DateTime OutDate { get; set; }
        public int Type { get; set; }
    }
}
