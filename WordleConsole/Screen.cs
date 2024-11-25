using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;

namespace WordleConsole
{
    internal unsafe class Screen
    {
        private int _desktopH;
        private float _scale;
        private nint _window;
        private nint _renderer;

        const int K_SCREEN_WIDTH = 500;
        const int K_SCREEN_HEIGHT = 782;
        const string BG_COLOR = "black";
        const string INACTIVE_COLOR = "grey";
        const string NOT_TESTED_COLOR = "white";
        const string INCORRECT_COLOR = "red";
        const string WRONG_PLACE_COLOR = "yellow";
        const string CORRECT_COLOR = "green";


        public Screen()
        {
            InitSdl();
        }


        public string GetLetterColor(Dictionary<char, Status> letters, char letter)
        {
            if (letters[letter] == Status.Correct)
                return CORRECT_COLOR;
            if (letters[letter] == Status.Incorrect)
                return INCORRECT_COLOR;
            if (letters[letter] == Status.WrongPlace)
                return WRONG_PLACE_COLOR;
            if (letters[letter] == Status.NotTested)
                return NOT_TESTED_COLOR;
            else
                return INACTIVE_COLOR;
        }

        public void DrawScreen(List<List<Square>> grid, Banner banner, Dictionary<char, Status> letters)
        {
            int exitCode;

            //Fill the surface black
            if (SDL_SetRenderDrawColor(_renderer, 135, 0, 0, 0) < 0)
            {
                SDL_Log($"SDL could not initialize! SDL error: {SDL_GetError()}");
            }

            //Update the surface
            SDL_UpdateWindowSurface(_window);

            int infobarStart = DrawSquares(grid);
            DrawBanner(banner, infobarStart);
            DrawKeyboard(infobarStart + (int)(24 * _scale), letters);

            SDL_RenderPresent(_renderer);
        }

        public bool InitSdl()
        {
            //Initialization flag
            bool success = true;

            //Initialize SDL
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                SDL_Log($"SDL could not initialize! SDL error: {SDL_GetError()}");
                success = false;
            }
            else
            {
                //Create window
                _window = SDL_CreateWindow("SDL2 Window", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, K_SCREEN_WIDTH, K_SCREEN_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (_window == IntPtr.Zero)
                {
                    SDL_Log($"Window could not be created! SDL error: {SDL_GetError()}");
                    success = false;
                }
                else
                {
                    //Get window surface
                    _renderer = SDL_CreateRenderer(_window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
                    if (_renderer == IntPtr.Zero)
                    {
                        SDL_Log($"Rendering surface could not be created! SDL error: {SDL_GetError()}");
                        success = false;
                    }
                }
            }
            return success;
        }

        public int DrawSquares(List<List<Square>> grid)
        {
            int size = (int)(88 * _scale);
            int screenEdge = (int)(10 * _scale);
            int hMargin = (int)(10 * _scale);
            int vMargin = (int)(10 * _scale);
            int fontSize = (int)(36 * _scale);
            //int square_font = pygame.freetype.SysFont("OCR-A Extended", font_size);

            int end_y = 0;

            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Square square = grid[row][column];
                    string textColor = INACTIVE_COLOR;

                    if (square.Status == Status.NotTested)
                        textColor = NOT_TESTED_COLOR;
                    else if (square.Status == Status.Incorrect)
                        textColor = INCORRECT_COLOR;
                    else if (square.Status == Status.Correct)
                        textColor = CORRECT_COLOR;
                    else if (square.Status == Status.WrongPlace)
                        textColor = WRONG_PLACE_COLOR;

                    //square_rect = pygame.Rect(column * (size + hor_margin) + screen_edge, row * (size + ver_margin) + screen_edge, size, size);
                    //pygame.draw.rect(self.window, text_color, square_rect, 1);
                    //text_surface, text_rect = square_font.render(square.letter, text_color, BG_COLOR);
                    //self.window.blit(text_surface, (square_rect.left + square_rect.width / 2 - text_rect.width / 2, square_rect.top + square_rect.height / 2 - text_rect.height / 2));

                    //end_y = max(end_y, square_rect.bottom + ver_margin);
                }
            }

            return end_y;
        }

        public void DrawBanner(Banner banner, int startY)
        {
            //var labelFont = pygame.freetype.SysFont("Lucida Console", 22 * _scale);
            //text_surface, text_rect = labelFont.render(banner.caption, NOT_TESTED_COLOR);
            //self.window.blit(text_surface, (0 + self.window.get_width() / 2 - text_rect.width / 2, start_y));
        }

        public void DrawKeyboard(int startY, Dictionary<char, Status> letters)
        {
            int size = (int)(42 * _scale);
            int font_size = (int)(18 * _scale);
            int hor_screen_edge = (int)(8 * _scale);
            int hor_margin = (int)(7 * _scale);
            int ver_margin = (int)(7 * _scale);
            int row_margin = (int)(8 * _scale);

            startY += ver_margin;
            List<string> rows = ["QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM"];

            for (int row = 0; row < 3; row++)
            {
                int column = 0;
                foreach (char letter in rows[row])
                {
                    DrawCharacter(row, column++, letter);
                }
            }

            void DrawCharacter(int row, int column, char letter)
            {
                string fgColor = GetLetterColor(letters, letter);
                // square_font = pygame.freetype.SysFont("Trebuchet MS", font_size);

                //square_rect = pygame.Rect(hor_screen_edge + column * (size + hor_margin) + row * row_margin, start_y + row * (size + ver_margin), size, size);
                //pygame.draw.rect(self.window, fgColor, square_rect, 1);
                //text_surface, text_rect = square_font.render(letter, fgColor);
                //self.window.blit(text_surface, (square_rect.left + square_rect.width / 2 - text_rect.width / 2, square_rect.top + square_rect.height / 2 - text_rect.height / 2));
            }
        }
    }
}
