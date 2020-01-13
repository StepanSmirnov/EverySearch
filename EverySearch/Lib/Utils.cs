using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EverySearch.Lib
{
    public class Utils
    {
        public struct Result<T> where T: class
        {
            public IList<Exception> Exceptions;
            public T Value;
        }

        public static async Task<Result<T>> WaitAnySuccessful<T>(IList<Func<T>> funcs) where T: class{
            int count = funcs.Count;
            List<Task<T>> tasks = new List<Task<T>>();
            BlockingCollection<Exception> exceptions = new BlockingCollection<Exception>();
            foreach (var func in funcs)
            {
                tasks.Add(Task.Run(() => {
                    try
                    {
                        var res = func();
                        return res;
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                        return null;
                    }
                }));
            }
            while(tasks.Count > 0)
            {
                var task = await Task.WhenAny(tasks);
                if (task.Result != null)
                {
                    return new Result<T>() { Value = task.Result, Exceptions = exceptions.ToArray() };
                }
                tasks.Remove(task);
            }
            return new Result<T>() { Value = null, Exceptions = exceptions.ToArray() };
        }
    }
}
