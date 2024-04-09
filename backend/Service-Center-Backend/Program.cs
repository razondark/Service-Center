
using Service_Center_Backend.Context;
using Service_Center_Backend.Services;
using Service_Center_Backend.Services.Implements;

namespace Service_Center_Backend
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ServiceCenterContext>();

			builder.Services.AddScoped<IAccountService, AccountServiceImplement>();
			builder.Services.AddScoped<IClientService, ClientServiceImplement>();
			builder.Services.AddScoped<IDeviceService, DeviceServiceImplement>();
			builder.Services.AddScoped<IEquipmentHandoverReceipt, EquipmentHandoverReceiptImplement>();
			builder.Services.AddScoped<IEmployeeService, EmployeeServiceImplement>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
