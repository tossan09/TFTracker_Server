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
                    AvgConsistency = reader.GetDecimal(5),
                    TotalMatches = reader.GetInt32(6)
                });
            }
            return stats;
        }

        public async Task<CompStatsDto?> GetStatsPorComp(string compId, string? patchId = null)
        {
            using var conexao = context.CriarConexao();
            await conexao.OpenAsync();

            var query = patchId == null ? @"
                SELECT comp_id,
                       ROUND(AVG(placement)::numeric, 2) AS avg_placement,
                       ROUND(AVG(CASE WHEN placement = 1 THEN 1 ELSE 0 END)::numeric * 100, 2) AS winrate_percent,
                       ROUND(AVG(CASE WHEN placement <= 4 THEN 1 ELSE 0 END)::numeric * 100, 2) AS top4rate_percent,
                       ROUND(AVG(CASE WHEN placement >= 5 THEN 1 ELSE 0 END)::numeric * 100, 2) AS bot8rate_percent,
                       COUNT(*) AS total_matches
                FROM matches
                WHERE comp_id = @compId
                GROUP BY comp_id;"
            :
                @"SELECT comp_id, patch_id,
                       ROUND(AVG(placement)::numeric, 2) AS avg_placement,
                       ROUND(AVG(CASE WHEN placement = 1 THEN 1 ELSE 0 END)::numeric * 100, 2) AS winrate_percent,
                       ROUND(AVG(CASE WHEN placement <= 4 THEN 1 ELSE 0 END)::numeric * 100, 2) AS top4rate_percent,
                       ROUND(AVG(CASE WHEN placement >= 5 THEN 1 ELSE 0 END)::numeric * 100, 2) AS bot8rate_percent,
                       COUNT(*) AS total_matches
                FROM matches
                WHERE comp_id = @compId AND patch_id = @patchId
                GROUP BY comp_id, patch_id;";

            using var comando = new NpgsqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@compId", compId);
            if (patchId != null) comando.Parameters.AddWithValue("@patchId", patchId);

            using var reader = await comando.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CompStatsDto
                {
                    CompId = reader.GetString(0),
                    PatchId = patchId == null ? null : reader.GetString(1),
                    AvgPlacement = reader.GetDecimal(patchId == null ? 1 : 2),
                    WinratePercent = reader.GetDecimal(patchId == null ? 2 : 3),
                    Top4RatePercent = reader.GetDecimal(patchId == null ? 3 : 4),
                    Bot8RatePercent = reader.GetDecimal(patchId == null ? 4 : 5),
                    TotalMatches = reader.GetInt32(patchId == null ? 5 : 6)
                };
            }
            return null;
        }
    }
}