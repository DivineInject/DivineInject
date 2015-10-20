using System.Collections.Generic;
using System.Linq;
using TestFirst.Net;

namespace DivineInject.Test
{
    public static class AMixedList
    {
        public static Builder<T> Of<T>()
        {
            return new Builder<T>();
        }

        public class Builder<T>
        {
            public AMixedList<T> With<T1>(IMatcher<T1> value)
                where T1 : T
            {
                return new AMixedList<T>(value);
            }

            public AMixedList<T> With<T1, T2>(IMatcher<T1> value1, IMatcher<T2> value2)
                where T1 : T
                where T2 : T
            {
                return new AMixedList<T>(value1, value2);
            }
        }
    }

    public class AMixedList<T> : AbstractMatcher<IEnumerable<T>>
    {
        private readonly IMatcher[] m_matchers;

        internal AMixedList(params IMatcher[] matchers)
        {
            m_matchers = matchers;
        }

        public override bool Matches(IEnumerable<T> actual, IMatchDiagnostics diag)
        {
            var list = actual.ToList();
            if (list.Count != m_matchers.Count())
            {
                diag.MisMatched("Expected {0} items, but found {1}", m_matchers.Length, list.Count);
                return false;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var matcher = m_matchers[i];
                if (!matcher.Matches(element, diag))
                {
                    diag.MisMatched("Item at index {0} in list did not match, expected {1} but was {2}", i, matcher,
                        element);
                    return false;
                }
            }

            return true;
        }
    }
}