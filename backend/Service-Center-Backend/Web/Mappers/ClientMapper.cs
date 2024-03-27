using Service_Center_Backend.Models;
using Service_Center_Backend.Web.Dto;

namespace Service_Center_Backend.Web.Mappers
{
	public static class ClientMapper
	{
		public static ClientDto ToDto(Client client)
		{
			return new ClientDto()
			{
				Id = client.Id,
				FullName = client.FullName,
				PhoneNumber = client.PhoneNumber,
				Email = client.Email
			};
		}

		public static List<ClientDto> ToDto(IList<Client> clients)
		{
			var listClientDto = new List<ClientDto>(clients.Count);

			foreach (var client in clients)
			{
				listClientDto.Add(ToDto(client));
			}

			return listClientDto;
		}

		public static Client ToModel(ClientDto clientDto)
		{
			return new Client()
			{
				Id = clientDto.Id,
				FullName = clientDto.FullName,
				PhoneNumber = clientDto.PhoneNumber,
				Email = clientDto.Email
			};
		}
	}
}
