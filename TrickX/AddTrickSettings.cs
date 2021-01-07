using System;
using System.Collections.Generic;
using System.Text;

namespace TrickX
{
    public static class AddTrickSettings
    {
        static public string name { get; set; }
        static public string points { get; set; }
        static public string author { get; set; }
        static public string url { get; set; }
        static public List<string> triggers { get; set; }
    }
}
