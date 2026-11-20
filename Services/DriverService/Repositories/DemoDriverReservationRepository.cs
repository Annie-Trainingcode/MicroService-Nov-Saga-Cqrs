using DriverService.Data;
using DriverService.Models;
using MongoDB.Driver;

namespace DriverService.Repositories
{
    public class DemoDriverReservationRepository : IDemoDriverReservationRepository
    {
        private readonly MongoDbContext _context;

        public DemoDriverReservationRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<DemoDriverReservation?> GetActiveReservationAsync(Guid driverId, CancellationToken cancellationToken)
        {
            return await _context.DemoDriverReservations
                .Find(r => r.DriverId == driverId && r.Status == "Reserved")
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ReserveAsync(Guid driverId, string sagaId, CancellationToken cancellationToken)
        {
            var activeReservation = await GetActiveReservationAsync(driverId, cancellationToken);
            if (activeReservation != null)
            {
                return false;
            }

            var reservation = new DemoDriverReservation
            {
                DriverId = driverId,
                SagaId = sagaId,
                Status = "Reserved",
                ReservedAt = DateTime.UtcNow
            };

            await _context.DemoDriverReservations.InsertOneAsync(reservation, cancellationToken: cancellationToken);
            return true;
        }

        public async Task<bool> ReleaseAsync(Guid driverId, string sagaId, CancellationToken cancellationToken)
        {
            var filter = Builders<DemoDriverReservation>.Filter.Where(r =>
                r.DriverId == driverId &&
                r.SagaId == sagaId &&
                r.Status == "Reserved");

            var update = Builders<DemoDriverReservation>.Update
                .Set(r => r.Status, "Released")
                .Set(r => r.ReleasedAt, DateTime.UtcNow);

            var result = await _context.DemoDriverReservations.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0;
        }
    }
}
