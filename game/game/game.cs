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
    public class game : scene
    {
        player_entity player;
        Vector2 camera_pos = new Vector2(0, 0);

        tilemap level;

        List<enemy_entity> enemies = new List<enemy_entity>();
        List<ragdoll_entity> ragdolls = new List<ragdoll_entity>();

        IntPtr radio, arrow;
        bool got_radio = false;

        int time;

        public override void on_load()
        {
            if(!scene_handler.get_scene("MiniJam88")) {
                scene_handler.add_scene("MiniJam88", new game());
                scene_handler.add_scene("Opening", new opening_scene());
                scene_handler.add_scene("Scene2", new scene_2());
                scene_handler.add_scene("Scene3", new scene_3());
                scene_handler.add_scene("menu", new main_menu());
                scene_handler.load_scene("menu");
            }

            radio = texture_handler.load_texture("radio_small.png", game_manager.renderer);
            arrow = texture_handler.load_texture("arrow.png", game_manager.renderer);

            game_manager.set_render_background(96, 96, 96, 255);

            level = new tilemap(40, 40, 8, 8, 8, 8);
            var full_path = game_manager.executable_path + "\\src\\resources\\MiniJam88\\data\\tilemaps\\map2.json";
            var file = System.IO.File.ReadAllText(full_path);

            level = JsonConvert.DeserializeObject<tilemap>(file);

            level.load_atlas();
            level.zoom = 2.5;
            level.position = new Vector2(0, 0);

            player = new player_entity(level);
            game_manager.set_render_resolution(game_manager.renderer, 450, 253);
            
            level.position.X -= 100;
            level.position.Y -= 100;

            var rand = new Random();
            for(var i = 0; i < 400; i++) {
                enemies.Add(new enemy_entity(rand.Next(0, 1000), rand.Next(0, 1000), player, level));
            }
        }

        public override void update()
        {
            time++;

            base.update();
            player.update();

            camera_pos.X += (player.position.X - camera_pos.X) / 6;
            camera_pos.Y += (player.position.Y - camera_pos.Y) / 6;
            camera.set_viewport(camera_pos.X, camera_pos.Y);

            for(var i = 0; i < enemies.Count; i++) {
                enemies[i].update();
                if(enemies[i].should_die) {
                    ragdolls.Add(new ragdoll_entity(enemies[i].tex_id, (int)enemies[i].position.X, (int)enemies[i].position.Y, player, (int)math_uti.point_direction(player.position, enemies[i].position), 5, level));
                    enemies.RemoveAt(i);
                }
            }

            for(var i = 0; i < ragdolls.Count; i++) {
                ragdolls[i].update();
            }

            if(math_uti.point_distance(player.position, new Vector2(0, 0)) < 100 && got_radio) {
                scene_handler.load_scene("menu");
            }

            if(math_uti.point_distance(player.position, new Vector2(913, 1212)) < 60) {
                got_radio = true;
            }
        }

        public override void render()
        {
            base.render();

            level.draw_tilemap();

            foreach(enemy_entity en in enemies) {
                en.render();
            }

            for(var i = 0; i < ragdolls.Count; i++) {
                ragdolls[i].render();
            }

            player.render();

            if(!got_radio) 
                draw.texture_ext(game_manager.renderer, radio, 913, 1212 + (int)((Math.Sin(time) * 5) / 2), 0, 64, 64, new SDL2.SDL.SDL_Point(16, 16), true);
            else {
                draw.texture_ext(game_manager.renderer, radio, (int)player.position.X + (int)math_uti.lengthdir_x(40, player.texture_angle - 105), (int)player.position.Y + (int)math_uti.lengthdir_y(40, player.texture_angle - 105), player.texture_angle, 32, 32, new SDL2.SDL.SDL_Point(4, 6), true);
                
                IntPtr tex;
                SDL2.SDL.SDL_Rect rect;

                font_handler.get_text_and_rect(game_manager.renderer, "Go back to START.", "default", out tex, out rect, 0, 0, 255, 255, 255, 255);
                draw.texture(game_manager.renderer, tex, 120, 10, 0, false);  
            }
        }
    }
}