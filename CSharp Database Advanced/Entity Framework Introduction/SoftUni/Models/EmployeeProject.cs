﻿using System;
using System.Collections.Generic;

namespace SoftUni.Models
{
    public partial class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }  
        public Project Project { get; set; }
    }
}