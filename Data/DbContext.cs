using Npgsql;

namespace TFTDataTrackerApi.Data
{
    public class DbContext
    {
        private readonly string? _connectionString;

        public DbContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public NpgsqlConnection CriarConexao()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
