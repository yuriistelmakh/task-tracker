using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class InvitationRepository : Repository<Invitation, int>, IInvitationRepository
{
    public InvitationRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public override async Task<int> AddAsync(Invitation entity)
    {
        var sql = @"
            SELECT *
            FROM Invitations
            WHERE BoardId = @BoardId AND
                  InviteeId = @InviteeId AND
                  IsAnswered = 0
        ";

        var notifications = await Connection.QueryAsync<Invitation>(sql, new { entity.BoardId, entity.InviteeId }, Transaction);

        if (notifications.Count() > 0)
        {
            return 0;
        }

        return await base.AddAsync(entity);
    }
}
