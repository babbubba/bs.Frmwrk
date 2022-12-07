﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.ViewModels.Auth
{
    public  interface IUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastLogin { get; set; }
        string? Token { get; set; }
    }
}