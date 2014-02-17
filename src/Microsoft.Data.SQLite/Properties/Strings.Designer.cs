// <auto-generated />
namespace Microsoft.Data.SQLite
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.Data.SQLite.Strings", typeof(Strings).GetTypeInfo().Assembly);

        /// <summary>
        /// The argument '{argumentName}' cannot be null, empty or contain only white space.
        /// </summary>
        internal static string ArgumentIsNullOrWhitespace(object argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ArgumentIsNullOrWhitespace", "argumentName"), argumentName);
        }

        /// <summary>
        /// {methodName} can only be called when the connection is open.
        /// </summary>
        internal static string CallRequiresOpenConnection(object methodName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CallRequiresOpenConnection", "methodName"), methodName);
        }

        /// <summary>
        /// CommandText must be set before {methodName} can be called.
        /// </summary>
        internal static string CallRequiresSetCommandText(object methodName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CallRequiresSetCommandText", "methodName"), methodName);
        }

        /// <summary>
        /// ConnectionString cannot be set when the connection is open.
        /// </summary>
        internal static string ConnectionStringRequiresClosedConnection
        {
            get { return GetString("ConnectionStringRequiresClosedConnection"); }
        }

        /// <summary>
        /// The CommandType '{commandType}' is invalid.
        /// </summary>
        internal static string InvalidCommandType(object commandType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidCommandType", "commandType"), commandType);
        }

        /// <summary>
        /// The IsolationLevel '{isolationLevel}' is invalid.
        /// </summary>
        internal static string InvalidIsolationLevel(object isolationLevel)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidIsolationLevel", "isolationLevel"), isolationLevel);
        }

        /// <summary>
        /// ConnectionString must be set before Open can be called.
        /// </summary>
        internal static string OpenRequiresSetConnectionString
        {
            get { return GetString("OpenRequiresSetConnectionString"); }
        }

        private static string GetString(string name, params string[] argumentNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            for (var i = 0; i < argumentNames.Length; i++)
            {
                value = value.Replace("{" + argumentNames[i] + "}", "{" + i + "}");
            }

            return value;
        }
    }
}
