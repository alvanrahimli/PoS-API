using System;

namespace StarDMS.Models.Dtos
{
    public class FirmReturnDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
    }
}