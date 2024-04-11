﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.Models
{
    public class PrivilegeOfTable
    {
        public string Owner { get; set; }
        public string TableName { get; set; }
        public string Grantee { get; set; }
        public string Privilege { get; set; }
        public string Grantable { get; set; }
        public int Number { get; set; }

    }
}