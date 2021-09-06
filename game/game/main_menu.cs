using static Proj.Modules.Debug.Debug;
using Proj.Modules.Misc;
using Proj.Modules.Game;
using Proj.Modules.Graphics;
using Proj.Modules.Debug;
using Proj.Modules.Input;
using Proj.Modules.Ui;
using Proj.Modules.Camera;
using Newtonsoft.Json;
using System.Numerics;
using SDL2;

namespace Proj.Game
{
    public class main_menu : scene
    {

        IntPtr title;

        public override void on_load()
        {
            game_manager.set_asset_pack("MiniJam88");
            font_handler.load_font("font", "FiraCode", 72);
            font_handler.load_font("font2", "FiraCode", 40);

            title = texture_handler.load_texture("title.png", game_manager.renderer);
        }

        public override void update()
        {
            game_manager.set_render_resolution(game_manager.renderer, 1280, 720);

            if(mouse.button_just_pressed(0) && math_uti.mouse_inside(20, 20, 600, 200)) {
                game_manager.set_render_resolution(game_manager.renderer, 450, 253);
                game_manager.set_render_resolution(game_manager.renderer, 450, 253);
                scene_handler.load_scene("Opening");
            }
        }

        public override void render() 
        {
            draw.round_rect(game_manager.renderer, new SDL.SDL_Rect(20, 20, 300, 100), 128, 237, 153, 255, 15, true);
            draw.round_rect(game_manager.renderer, new SDL.SDL_Rect(20, 140, 400, 150), 17, 50, 77, 255, 15, true);
            
            IntPtr tex;
            SDL.SDL_Rect rect;

            font_handler.get_text_and_rect(game_manager.renderer, "Play", "font", out tex, out rect, 0, 0, 255, 255, 255, 255);
            draw.texture(game_manager.renderer, tex, 160, 70, 0, false);

            font_handler.get_text_and_rect(game_manager.renderer, "How to Play:", "font2", out tex, out rect, 0, 0, 255, 255, 255, 255);
            draw.texture(game_manager.renderer, tex, 220, 180, 0, false);

            font_handler.get_text_and_rect(game_manager.renderer, "Move: W,A,D", "font2", out tex, out rect, 0, 0, 255, 255, 255, 255);
            draw.texture(game_manager.renderer, tex, 220, 232, 0, false);

            draw.texture_ext(game_manager.renderer, title, 550, 50, 0, 666, 400, new SDL.SDL_Point(0, 0), false);
        }
    }
}