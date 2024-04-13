using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServiceCenterLibrary.Exceptions
{
	public class ExceptionHandler : Exception
	{
		private class ErrorMessageResponse
		{
			public string? message { get; set; }
		}

		private static string ParseJsonMessage(string jsonMessage)
		{
			try
			{
				var parsedMessage = JsonSerializer.Deserialize<ErrorMessageResponse>(jsonMessage)?.message;
				return parsedMessage ?? "Неизвестная ошибка";
			}
			catch 
			{
				return jsonMessage;
			}
		}

		public ExceptionHandler(string message) : base(ParseJsonMessage(message)) { }
	}
}
