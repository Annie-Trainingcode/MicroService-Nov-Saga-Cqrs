using DriverService.DTOs;
using DriverService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
    [ApiController]
    [Route("api/demo/saga/drivers")]
    [AllowAnonymous]
    public class DemoSagaDriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IDemoDriverReservationRepository _reservationRepository;

        public DemoSagaDriversController(
            IDriverRepository driverRepository,
            IDemoDriverReservationRepository reservationRepository)
        {
            _driverRepository = driverRepository;
            _reservationRepository = reservationRepository;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] ReserveDriverRequest request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetDriverByDriverIdAsync(request.DriverId);
            if (driver == null)
            {
                return NotFound(new { Message = $"Driver {request.DriverId} not found" });
            }

            var reserved = await _reservationRepository.ReserveAsync(request.DriverId, request.SagaId, cancellationToken);
            if (!reserved)
            {
                return Conflict(new { Message = $"Driver {request.DriverId} is already reserved by another saga" });
            }

            return Ok(new { Message = "Driver reserved", request.DriverId, request.SagaId });
        }

        [HttpPost("release")]
        public async Task<IActionResult> Release([FromBody] ReleaseDriverRequest request, CancellationToken cancellationToken)
        {
            var released = await _reservationRepository.ReleaseAsync(request.DriverId, request.SagaId, cancellationToken);
            if (!released)
            {
                return NotFound(new { Message = "Active reservation not found for this saga" });
            }

            return Ok(new { Message = "Driver released", request.DriverId, request.SagaId });
        }

        [HttpGet("{driverId:guid}/reservation")]
        public async Task<IActionResult> GetReservation(Guid driverId, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetActiveReservationAsync(driverId, cancellationToken);
            if (reservation == null)
            {
                return NotFound(new { Message = "No active reservation" });
            }

            return Ok(reservation);
        }
    }
}
