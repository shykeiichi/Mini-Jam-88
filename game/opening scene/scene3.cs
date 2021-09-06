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

namespace Proj.Game
{
    public class scene_3 : scene
    {
        int time = 0;
        IntPtr radio;

        Vector2 playerpos = new Vector2(32, 140);
        Vector2 alertpos = new Vector2(-400, -400);
        Vector2 handpos = new Vector2(-400, -400);
        Vector2 camera_pos = new Vector2(0, 0);

        int player_angle = 0;

        tilemap level;

        public override void on_load()
        {
            game_manager.set_asset_pack("MiniJam88");
            radio = texture_handler.load_texture("radio.png", game_manager.renderer);

            level = new tilemap(20, 20, 8, 8, 8, 8);
            var full_path = game_manager.executable_path + "\\src\\resources\\MiniJam88\\data\\tilemaps\\scene3.json";
            var file = System.IO.File.ReadAllText(full_path);

            level = JsonConvert.DeserializeObject<tilemap>(file);

            level.load_atlas();
            level.zoom = 2.5;
            level.position = new Vector2(-200, -130);
        }

        public override void update()
        {
            if(input.get_key_just_pressed(input.key_space)) {
                scene_handler.load_scene("MiniJam88");
            }

            time++;

            camera_pos.X += (0 - camera_pos.X) / 6;
            camera_pos.Y += (0 - camera_pos.Y) / 6;
            camera.set_viewport(camera_pos.X, camera_pos.Y);

            if(time == 200) {
                scene_handler.load_scene("MiniJam88");
            }
        }

        public override void render() 
        {
            level.draw_tilemap();

            draw.texture_ext(game_manager.renderer, radio, 0, -30, 0, 146, 146, new SDL2.SDL.SDL_Point(73, 73), true);
        }
    }
}