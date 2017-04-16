using System;
using System.Linq.Expressions;
using System.Text;
using static NTouchTypeTrainer.Common.Strings.ToStringConverterHelper;

namespace NTouchTypeTrainer.Common.Logging
{
    public static class LoggingExtensions
    {
        public static string Enter(string idCaller = null, params Expression<Func<dynamic>>[] args)
        {
            var builder = new StringBuilder();

            builder.Append("Enter");

            AppendParameterList(builder, args);

            if (idCaller != null)
            {
                builder.Append(GetCallerIdText(idCaller));
            }

            return builder.ToString();
        }

        public static string Leave(object returnValue)
        {
            return $"Leave, returning {returnValue}";
        }

        public static string Leave(string idCaller = null)
        {
            return "Leave " + GetCallerIdText(idCaller);
        }

        public static string Leave(string idCaller, object returnValue)
        {
            return $"Leave {GetCallerIdText(idCaller)}, returning {returnValue}";
        }

        public static string MsgReceived(object message, string idCaller = null)
        {
            return "Msg: " + message + GetCallerIdText(idCaller);
        }

        public static string GetCallerIdText(string idCaller)
        {
            return (idCaller != null) ? "(Id: " + idCaller + ")" : "";
        }
    }
}