using System;
using System.Diagnostics;
using System.Linq;

namespace iTunesAdjuster.ArgumentValidators
{
    /// <summary>
    /// 呼び出し元のメソッド・プロパティ・インデクサ・コンストラクタのパラメーターが NULL かどうかを検証する機能を定義します。
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// 呼び出し元のメソッド・プロパティ・インデクサ・コンストラクタのパラメーターが NULL かどうかを検証します。
        /// </summary>
        /// <param name="arguments">呼び出し元のパラメーター</param>
        /// <remarks>
        /// <![CDATA[
        /// 呼び出し元メソッド・プロパティ・インデクサ・コンストラクタのパラメーターに RequireAttribute の指定がある引数のみ NULL かどうかを確認します。
        /// 呼び出し元メソッド・プロパティ・インデクサ・コンストラクタのパラメーターの順序、数は一致するように指定してください。
        /// ]]>
        /// </remarks>
        /// <example>
        /// [Require]
        /// public void Hoge(string arg1, string arg2, string arg3)
        /// {
        ///     //throw new ArgumentNullException("arg1");
        ///     RequireAttribute.Validate(arg1, arg2, arg3);
        /// }
        /// public void Hoge([Require]string arg1, string arg2, [Require]string arg3)
        /// {
        ///     //throw new ArgumentNullException("arg1");
        ///     RequireAttribute.Validate(arg1, arg2, arg3);
        /// }
        /// [Require]
        /// public string Hoge
        /// {
        ///     get
        ///     {
        ///         return this._hoge;
        ///     }
        ///     set
        ///     {
        ///         //throw new ArgumentNullException("value");
        ///         RequireAttribute.Validate(value); 
        ///     }
        /// }
        /// [Require]
        /// public int this[int index]
        /// {
        ///     get
        ///     {
        ///         return this._hoge[index];
        ///     }
        ///     set
        ///     {
        ///         //throw new ArgumentNullException("index");
        ///         RequireAttribute.Validate(index, value); 
        ///     }
        /// }
        /// public class Hoge
        /// {
        ///     [Require]
        ///     public Hoge(string arg1, string arg2, string arg3)
        ///     {
        ///         //throw new ArgumentNullException("arg1");
        ///         RequireAttribute.Validate(index, value); 
        ///     }
        /// }
        /// </example>
        /// <exception cref="System.ArgumentException">引数の数が一致しません。</exception>
        public static void Validate(params object[] arguments)
        {
            var call = new StackTrace()
                .GetFrame(1)
                .GetMethod();

            var parameters = call.GetParameters();

            if (parameters.Length != arguments.Length)
            {
                throw new ArgumentException("引数の数が一致しません。");
            }

            var attributeType = typeof(RequireAttribute);

            var isMethodAllCheck = call.IsSpecialName;
            if (isMethodAllCheck == false)
            {
                isMethodAllCheck = call.GetCustomAttributes(attributeType, true).Any();
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (isMethodAllCheck == false && parameter.GetCustomAttributes(attributeType, true).Any() == false)
                {
                    continue;
                }

                if (arguments.ElementAtOrDefault(i) != null)
                {
                    continue;
                }

                throw new ArgumentNullException(parameters[i].Name);
            }
        }
    }
}
