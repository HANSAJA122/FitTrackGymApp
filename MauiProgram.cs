using Microsoft.Extensions.Logging;
using FitTrackGymApp.Data;
using FitTrackGymApp.Services;
using FitTrackGymApp.ViewModels;
using FitTrackGymApp.Views;

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

		// Register Pages and ViewModels
		builder.Services.AddTransient<MembersViewModel>();
		builder.Services.AddTransient<MembersPage>();
		
		builder.Services.AddTransient<AddEditMemberViewModel>();
		builder.Services.AddTransient<AddEditMemberPage>();

		builder.Services.AddTransient<PlansViewModel>();
		builder.Services.AddTransient<PlansPage>();

		builder.Services.AddTransient<PaymentsViewModel>();
		builder.Services.AddTransient<PaymentsPage>();

		builder.Services.AddTransient<DashboardViewModel>();
		builder.Services.AddTransient<DashboardPage>();

		builder.Services.AddTransient<AttendanceViewModel>();
		builder.Services.AddTransient<AttendancePage>();

		builder.Services.AddSingleton<ExerciseApiService>();
		builder.Services.AddTransient<ExerciseViewModel>();
		builder.Services.AddTransient<ExercisePage>();

		return builder.Build();
	}
}
