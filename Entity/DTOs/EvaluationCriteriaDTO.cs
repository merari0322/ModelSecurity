﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class EvaluationCriteriaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public int EvaluationId { get; set; }
    }
}
