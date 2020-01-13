using EverySearch.Lib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EverySearchTests
{
    [TestFixture]
    class UtilsTests
    {

        [Test]
        public async System.Threading.Tasks.Task WaitAnySuccessful_EmptyParameter_ReturnNullAsync()
        {
            IList<Func<string>> funcs = new List<Func<string>>();
            var result = await Utils.WaitAnySuccessful(funcs);
            Assert.IsNull(result.Value, "Result value is not null");
            Assert.IsTrue(result.Exceptions.Count == 0, "Exceptions are not empty");
        }

        [Test]
        public async System.Threading.Tasks.Task WaitAnySuccessful_AllFails_ReturnExceptions()
        {
            IList<Func<string>> funcs = new List<Func<string>>();
            int n = 200;
            for (int i = 0; i < n; i++)
            {
                funcs.Add(() => { throw new ArgumentException(); });
            }
            var result = await Utils.WaitAnySuccessful(funcs);
            Assert.IsNull(result.Value, "Result value is not null");
            Assert.IsTrue(result.Exceptions.Count == n, 
                $"Method returned not all exceptions. Actual is {result.Exceptions.Count}, {n} expected");
        }

        [Test]
        public async System.Threading.Tasks.Task WaitAnySuccessful_OneSuceed_ReturnValueAndExceptions()
        {
            IList<Func<string>> funcs = new List<Func<string>>()
            {
                () => { Thread.Sleep(50); return "Last task"; },
                () => { throw new FormatException(); },
                () => { throw new NullReferenceException(); }
            };
            var result = await Utils.WaitAnySuccessful(funcs);
            Assert.IsTrue(result.Value == "Last task", "Result value is not correct");
            Assert.IsTrue(result.Exceptions.Count == 2, "Wrong exceptions count");
        }

        [Test]
        public async System.Threading.Tasks.Task WaitAnySuccessful_AllSuceed_ReturnValue()
        {
            IList<Func<string>> funcs = new List<Func<string>>()
            {
                () => {Thread.Sleep(2000);return "First task"; },
                () => {Thread.Sleep(2000);return "Second task"; },
                () => {return "Third task"; },
            };
            var result = await Utils.WaitAnySuccessful(funcs);
            Assert.IsTrue(result.Value == "Third task", "Result value is null");
            Assert.IsTrue(result.Exceptions.Count == 0, "Wrong exceptions count");
        }
    }
}
