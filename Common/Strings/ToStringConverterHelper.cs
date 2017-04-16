using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Caliburn.Micro;

namespace NTouchTypeTrainer.Common.Strings
{
    public static class ToStringConverterHelper
    {
        private const string ParamsStart = " (";
        private const string ParamColon = ": ";
        private const string ParamNull = "null";
        private const string ParamsSeparator = ", ";
        private const string ParamsEnd = ")";

        public static string GetObjectId<T>(params object[] idValues)
        {
            var builder = new StringBuilder().Append(typeof(T).Name);

            AppendParameterList(builder, idValues);

            return builder.ToString();
        }

        public static string GetObjectId<T>(params Expression<Func<dynamic>>[] idValues)
        {
            var builder = new StringBuilder().Append(typeof(T).Name);

            AppendParameterList(builder, idValues);

            return builder.ToString();
        }

        public static void AppendParameterList(StringBuilder builder, params object[] paras)
        {
            if ((paras != null) && paras.Any())
            {
                builder.Append(ParamsStart);

                foreach (var idVal in paras)
                {
                    builder.Append(idVal?.ToString() ?? ParamNull);
                    builder.Append(ParamsSeparator);
                }
                builder.Remove(builder.Length - ParamsSeparator.Length, ParamsSeparator.Length);

                builder.Append(ParamsEnd);
            }
        }

        public static void AppendParameterList(StringBuilder builder, params Expression<Func<dynamic>>[] paras)
        {
            if (paras != null && paras.Any())
            {
                builder.Append(ParamsStart);

                foreach (var param in paras.Where(arg => arg != null))
                {
                    builder
                        .Append(param.GetMemberInfo().Name)
                        .Append(ParamColon)
                        .Append(param.Compile().Invoke()?.ToString() ?? ParamNull);
                    builder.Append(ParamsSeparator);
                }
                builder.Remove(builder.Length - ParamsSeparator.Length, ParamsSeparator.Length);

                builder.Append(ParamsEnd);
            }
        }
    }
}