namespace api.Services.HeartBeat
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IHeartBeatService
    {
        Task ReportAliveAsync(string groupId, string id);
        Task<IEnumerable<string>> GetAliveMembersAsync(string groupId);
    }
}