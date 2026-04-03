using Npgsql;
using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.DTOs;

namespace TFTDataTrackerApi.Repository
{
    public class StatsRepository(DbContext context)
    {
        private readonly DbContext context = context;

        public async Task<List<PatchStatsDto>> GetStatsPorPatch()
        {
            var stats = new List<PatchStatsDto>();
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var comando = new NpgsqlCommand(@"SELECT * FROM stats_matches_patch;", conexao);

            using var reader = await comando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                stats.Add(new PatchStatsDto
                {
                    PatchNumber = reader.GetString(0),
                    AvgPlacement = reader.GetDecimal(1),
                    WinratePercent = reader.GetDecimal(2),
                    Top4RatePercent = reader.GetDecimal(3),
                    Bot8RatePercent = reader.GetDecimal(4),
                    AvgConsistency = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                    TotalMatches = reader.GetInt32(6)
                });
            }
            return stats;
        }

        public async Task<IEnumerable<CompStatsDto>> GetStatsPorComp(string compName, string patchNumber)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var query = @"
            SELECT *
            FROM stats_matches_comp_patch
            WHERE comp_name = @compName
            AND patch_number = @patchNumber
            ORDER BY patch_number;";

            using var comando = new NpgsqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@compName", compName);
            comando.Parameters.AddWithValue("@patchnumber", patchNumber);

            using var reader = await comando.ExecuteReaderAsync();
            var result = new List<CompStatsDto>();

            while (await reader.ReadAsync())
            {
                result.Add(new CompStatsDto
                {
                    CompName = reader.GetString(0),
                    PatchNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                    AvgPlacement = reader.GetDecimal(2),
                    WinratePercent = reader.GetDecimal(3),
                    Top4RatePercent = reader.GetDecimal(4),
                    Bot8RatePercent = reader.GetDecimal(5),
                    AvgConsistency = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    TotalMatches = reader.GetDecimal(7)
                });
            }

            return result;
        }



    }
}