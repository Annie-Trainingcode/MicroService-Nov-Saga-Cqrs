using DriverService.Models;

namespace DriverService.Repositories
{
    public interface IDemoDriverReservationRepository
    {
        Task<bool> ReserveAsync(Guid driverId, string sagaId, CancellationToken cancellationToken);
        Task<bool> ReleaseAsync(Guid driverId, string sagaId, CancellationToken cancellationToken);
        Task<DemoDriverReservation?> GetActiveReservationAsync(Guid driverId, CancellationToken cancellationToken);
    }
}
