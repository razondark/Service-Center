using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Dto.Response
{
	public class ClientsResponse
	{
		public IEnumerable<ClientDto> Clients { get; set; } = null!;

		public ClientsResponse(List<ClientDto> clients)
		{
			Clients = clients;
		}
	}
}
