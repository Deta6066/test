﻿namespace VIS_API.Utilities
{
    public class MyException : Exception
    {
        public string Remark;
        public MyException(string message, string remark) : base(message)
        {
            Remark = remark;
        }
    }
}

