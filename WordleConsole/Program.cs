// See https://aka.ms/new-console-template for more information
using WordleConsole;

Console.WriteLine("Hello, World!");

Game game = new Game(new Screen(), new Banner());
game.banner.Caption = "Start typing to begin";
game.Initialize();
game.Run();
//clock = pygame.time.Clock();