using System;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace Media.Infrastructure
{
    internal static class MediaDatabase
    {
        public static SqlConnection Open()
        {
            var setting = ConfigurationManager.ConnectionStrings["DatabaseEntities"];
            if (setting == null)
                throw new ConfigurationErrorsException("Connection string 'DatabaseEntities' was not found.");
            var builder = new EntityConnectionStringBuilder(setting.ConnectionString);
            var connection = new SqlConnection(builder.ProviderConnectionString);
            connection.Open();
            return connection;
        }

        public static string PictureUrl(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            return Convert.ToString(value).Replace("SYSTEM_TYPE_PANEL", "/panel");
        }
    }
}
