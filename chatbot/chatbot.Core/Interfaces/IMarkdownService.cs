﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Core.Interfaces
{
    public interface IMarkdownService
    {
        string ConvertToHtml(string markdownText);
    }
}
