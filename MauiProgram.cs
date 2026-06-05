using Microsoft.Extensions.Logging;
using FitTrackGymApp.Data;
using FitTrackGymApp.Services;

namespace FitTrackGymApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register Services and Database
		builder.Services.AddDbContext<AppDbContext>();
		builder.Services.AddSingleton<DatabaseService>();

		return builder.Build();
	}
}
