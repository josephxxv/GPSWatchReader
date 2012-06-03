using System;

namespace WatchApi
{
    public static class Extensions
    {
        public static uint ToUint(this int i)
        {
            return Convert.ToUInt32(i);
        }
        
        public static int ToInt(this uint i)
        {
            return Convert.ToInt32(i);
        }
    }
}