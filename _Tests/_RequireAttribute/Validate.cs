using iTunesAdjuster.ArgumentValidators;
using NUnit.Framework;
using System;

namespace _Tests._RequireAttribute
{
    [TestFixture]
    public class Validate
    {
        [TestCase(null, null, null, "arg1")]
        [TestCase("", null, null, "arg2")]
        [TestCase("", "", null, "arg3")]
        public void コンストラクタに属性をつけた場合ArgumentNullExceptionがスローされる(string arg1, string arg2, string arg3, string parameterName)
        {
            var actual = Assert.Throws<ArgumentNullException>(() =>
            {
                new ConstructorTestClass(arg1, arg2, arg3);
            });
            Assert.AreEqual(parameterName, actual.ParamName);
        }

        [TestCase(null, null, null, "arg1")]
        [TestCase("", null, null, "arg2")]
        [TestCase("", "", null, "arg3")]
        public void メソッドに属性をつけた場合ArgumentNullExceptionがスローされる(string arg1, string arg2, string arg3, string parameterName)
        {
            var actual = Assert.Throws<ArgumentNullException>(() =>
            {
                RequireAllArguments(arg1, arg2, arg3);
            });
            Assert.AreEqual(parameterName, actual.ParamName);
        }

        [TestCase(null, null, null, "arg1")]
        [TestCase("", "", null, "arg3")]
        public void メソッドの引数に属性をつけた場合ArgumentNullExceptionがスローされる(string arg1, string arg2, string arg3, string parameterName)
        {
            var actual = Assert.Throws<ArgumentNullException>(() =>
            {
                RequirePartOfArguments(arg1, arg2, arg3);
            });
            Assert.AreEqual(parameterName, actual.ParamName);
        }

        [Test]
        public void メソッドの引数に属性をつけてない場合例外はスローされない()
        {
            Assert.IsTrue(RequirePartOfArguments("", null, ""));
        }

        [Test]
        public void プロパティに属性をつけた場合ArgumentNullExceptionをスローする()
        {
            var privateException = Assert.Throws<ArgumentNullException>(() =>
            {
                PrivateProperty = null;
            });
            Assert.AreEqual("value", privateException.ParamName);

            var publicException = Assert.Throws<ArgumentNullException>(() =>
            {
                PublicProperty = null;
            });
            Assert.AreEqual("value", publicException.ParamName);
        }

        [TestCase("0", null, "value")]
        [TestCase(null, "", "index")]
        public void インデクサに属性をつけた場合ArgumentNullExceptionをスローする(string index, string value, string parameterValue)
        {
            var exception1 = Assert.Throws<ArgumentNullException>(() =>
            {
                this[index] = value;
            });
            Assert.AreEqual(parameterValue, exception1.ParamName);
        }

        [Test]
        public void チェック対象関数の引数と関数の引数の数が一致しない場合例外をスローする()
        {
            var exception1 = Assert.Throws<ArgumentException>(() =>
            {
                DifferenceArgumentLengthMethod("", "", null);
            });
            Assert.AreEqual("引数の数が一致しません。", exception1.Message);
        }

        #region テストメソッド
        [Require]
        private bool RequireAllArguments(string arg1, string arg2, string arg3)
        {
            ArgumentValidator.Validate(arg1, arg2, arg3);
            return true;
        }
        private bool RequirePartOfArguments([Require]string arg1, string arg2, [Require]string arg3)
        {
            ArgumentValidator.Validate(arg1, arg2, arg3);
            return true;
        }
        private bool DifferenceArgumentLengthMethod([Require]string arg1, string arg2, [Require]string arg3)
        {
            ArgumentValidator.Validate(arg1, arg2, arg3, "");
            return true;
        }
        #endregion

        #region テストインデクサ
        [Require]
        private string this[string index]
        {
            get
            {
                return this._TestValue;
            }
            set
            {
                ArgumentValidator.Validate(index, value);

                this._TestValue = value;
            }
        }
        #endregion

        #region テストプロパティ
        [Require]
        public string PublicProperty
        {
            get
            {
                return this._TestValue;
            }
            set
            {
                ArgumentValidator.Validate(value);

                this._TestValue = value;
            }
        }
        [Require]
        private string PrivateProperty
        {
            get
            {
                return this._TestValue;
            }
            set
            {
                ArgumentValidator.Validate(value);

                this._TestValue = value;
            }
        }
        private string _TestValue;
        #endregion
    }

    #region テストコンストラクタ
    public class ConstructorTestClass
    {
        [Require]
        public ConstructorTestClass(string arg1, string arg2, string arg3)
        {
            ArgumentValidator.Validate(arg1, arg2, arg3);
        }
    }
    #endregion
}
