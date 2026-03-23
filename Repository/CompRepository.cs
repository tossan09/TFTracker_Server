using Npgsql;
using System.Security.Cryptography;
using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.DTOs;
using TFTDataTrackerApi.Models;

namespace TFTDataTrackerApi.Repository
{
    public class CompRepository
    {
        private readonly DbContext context;

        public CompRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task<List<Comps>> ListarComps()
        {
            var comps = new List<Comps>();
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            using var comando = new NpgsqlCommand("SELECT * FROM comps", conexao);
            using var reader = await comando.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                comps.Add(new Comps
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    traits = reader.IsDBNull(2) ? null : reader.GetString(2),
                    style = reader.IsDBNull(3) ? null : reader.GetString(3),
                    setid = reader.GetInt32(4)

                });
            }
            return comps;
        }

        public async Task<List<CompDto>> ListarCompsPorSet(int SetNumber)
        {
            var comps = new List<CompDto>();
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            using var comando = new NpgsqlCommand("SELECT * FROM vw_comps_full WHERE set_number = @setnumber ORDER BY id DESC", conexao);
            comando.Parameters.AddWithValue("@setnumber", SetNumber);

            using var reader = await comando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                comps.Add(new CompDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Traits = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Style = reader.IsDBNull(3) ? null : reader.GetString(3),
                    SetNumber = reader.GetInt32(4)
                });
            }
            return comps;
        }


        public async Task<bool> AddComp(Comps comp)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var insertQuery = "INSERT INTO comps (name, traits, style, set_id) VALUES (@name, @traits, @style, @setid)";

            using var comando = new NpgsqlCommand(insertQuery, conexao);
            comando.Parameters.AddWithValue("@name", comp.name);
            comando.Parameters.AddWithValue("@traits", comp.traits ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@style", comp.style ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@setid", comp.setid);

            await comando.ExecuteNonQueryAsync();
            return true;
        }


        public async Task<bool> EditComp(int id, Comps comps)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var updateQuery = "UPDATE comps SET name = @name, traits = @traits, style = @style WHERE id = @id";
            using var comando = new NpgsqlCommand(updateQuery, conexao);
            comando.Parameters.AddWithValue("@name", comps.name);
            comando.Parameters.AddWithValue("@traits", (object?)comps.traits ?? DBNull.Value);
            comando.Parameters.AddWithValue("@style", (object?)comps.style ?? DBNull.Value);
            comando.Parameters.AddWithValue("@id", id);

            var linha = await comando.ExecuteNonQueryAsync();
            return linha > 0;
        }

        public async Task<bool> DeleteComp(int id)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var deleteQuery = "DELETE FROM comps WHERE id = @id";
            using var comando = new NpgsqlCommand(deleteQuery, conexao);
            comando.Parameters.AddWithValue("id", id);

            var linha = await comando.ExecuteNonQueryAsync();
            return linha > 0;
        }

    }
}
