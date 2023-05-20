using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Oid85.FinMarket.Configuration.Data
{
    public static class DataBaseConfigurationExtentions
    {
        public static IConfigurationBuilder AddDataBase(this IConfigurationBuilder builder, string path)
        {
            return builder.Add(
                new DataBaseConfigurationSource(
                    new SqliteConnectionStringBuilder
                    {
                        DataSource = path
                    }.ToString()));
        }
    }
}