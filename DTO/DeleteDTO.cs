using Core.Models.Manager.Interface;

namespace Core.Models.Manager.DTO;

class DeleteDTO : IDeleteDTO
{
    public DateTime Time { get; set; }
    public bool Deleted { get; set; }
}