using System.ComponentModel.DataAnnotations;

namespace DriverService.DTOs
{
    public class ReserveDriverRequest
    {
        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public string SagaId { get; set; } = string.Empty;
    }

    public class ReleaseDriverRequest
    {
        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public string SagaId { get; set; } = string.Empty;
    }
}
