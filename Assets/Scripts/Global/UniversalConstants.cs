namespace FrontLineDefense.Global
{
    public enum ShootStatus { AVAILABLE_TO_SHOOT, SHOT_PROJECTILE, RECHARGING, SEARCHING_PLAYER, FOUND_PLAYER }
    public enum PlaneRotateStatus { LEFT, RIGHT, IN_PROCESS_OF_TURNING }
    public enum BombStatus { AVAILABLE, SHOT }
    public enum PlayerAction { BOMB_DROP, PLAYER_HIT, PLAYER_DEAD }
    public enum ButtonClicked { RESTART }
    public enum SceneToLoad { MAIN_MENU, MAIN_GAMEPLAY }

    public class UniversalConstants
    {
        public const string Bomb = "Bomb";
        public const string Player = "Player";
        public const string StatComponent = "StatComponent";
        public const float _gravity = -9.8f;
    }
}