using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace iTunesAdjuster.ArgumentValidators
{
    /// <summary>
    /// 必須パラメーターであることを定義する属性です。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Constructor)]
    public class RequireAttribute : Attribute
    {
    }
}
