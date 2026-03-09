using Npgsql;
using System.Text.RegularExpressions;
using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.DTOs;
using TFTDataTrackerApi.Models;

namespace TFTDataTrackerApi.Repository
{
    public class MatchRepository
    {
        private readonly DbContext context;

        public MatchRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task<List<MatchFulldto>> ListarPartidasPorPatch(string patchnumber)
        {
            var partidas = new List<MatchFulldto>();
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            using var comando = new NpgsqlCommand("SELECT * FROM vw_matches_full WHERE patch_number = @patchnumber ORDER BY id DESC", conexao);
            comando.Parameters.AddWithValue("@patchnumber", patchnumber);
            using var reader = await comando.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                partidas.Add(new MatchFulldto
                {
                    id = reader.GetInt32(0),
                    compname = reader.GetString(1),
                    patchnumber = reader.GetString(2),
                    placement = reader.GetInt32(3),
                    finallevel = reader.GetInt32(4),
                    goldstage32 = reader.GetInt32(5),
                    goldstage41 = reader.GetInt32(6),
                    hpstage32 = reader.GetInt32(7),
                    forced = reader.GetBoolean(8),
                    contested = reader.GetBoolean(9),
                    comment = reader.GetString(10),

                });
            }
            return partidas;
        }

        public async Task<List<Matches>> ListarCompIDPorPatchID(int patchId, int compId)
        {
            var partidas = new List<Matches>();
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            using var comando = new NpgsqlCommand("SELECT * FROM matches WHERE comp_id = @compId AND patch_id = @patchId ORDER BY id DESC", conexao);
            comando.Parameters.AddWithValue("@compId", compId);
            comando.Parameters.AddWithValue("@patchId", patchId);
            using var reader = await comando.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                partidas.Add(new Matches
                {
                    id = reader.GetInt32(0),
                    comp_id = reader.GetInt32(1),
                    patch_id = reader.GetInt32(2),
                    placement = reader.GetInt32(3),
                    finallevel = reader.GetInt32(4),
                    goldstage32 = reader.GetInt32(5),
                    goldstage41 = reader.GetInt32(6),
                    hpstage32 = reader.GetInt32(7),
                    forced = reader.GetBoolean(8),
                    contested = reader.GetBoolean(9),
                    comment = reader.GetString(10),
                });
            }
            return partidas;
        }
 
        public async Task<bool> AddPartida(Matches matches)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var insertQuery = "SELECT add_match(@patch, @comp, @placement, @finallevel, @goldstage32, @goldstage41, @hpstage32, @forced, @contested, @comment)";

            using var comando = new NpgsqlCommand(insertQuery, conexao);
            comando.Parameters.AddWithValue("@patch", matches.patch_id);
            comando.Parameters.AddWithValue("@comp", matches.comp_id);
            comando.Parameters.AddWithValue("@placement", matches.placement);
            comando.Parameters.AddWithValue("@finallevel", matches.finallevel);
            comando.Parameters.AddWithValue("@goldstage32", matches.goldstage32);
            comando.Parameters.AddWithValue("@goldstage41", matches.goldstage41);
            comando.Parameters.AddWithValue("@hpstage32", matches.hpstage32);
            comando.Parameters.AddWithValue("@forced", matches.forced);
            comando.Parameters.AddWithValue("@contested", matches.contested);
            comando.Parameters.AddWithValue("@comment", matches.comment);

            await comando.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> DeleteMatch(int id)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var deleteQuery = "DELETE FROM matches WHERE id = @id";
            using var comando = new NpgsqlCommand(deleteQuery, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var linha = await comando.ExecuteNonQueryAsync();
            return linha > 0;
        }
    }
}
