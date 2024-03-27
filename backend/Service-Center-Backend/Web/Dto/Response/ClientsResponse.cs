using Service_Center_Backend.Models;

namespace Service_Center_Backend.Web.Dto.Response
{
    public class ClientsResponse
    {
        public List<ClientDto> Clients { get; set; } = null!;

        public ClientsResponse(List<ClientDto> clients)
        {
            Clients = clients;
        }
    }
}
