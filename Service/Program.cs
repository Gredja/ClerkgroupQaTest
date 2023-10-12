using Service;

var setup = WebApplication.CreateBuilder(args);
var startup = new Startup();

startup.ConfigureServices(setup.Services,
    setup.Configuration,
    setup.Environment);

var app = setup.Build();

startup.Configure(app);

app.Run();
