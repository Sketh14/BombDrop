namespace FrontLineDefense.Global
{
    public enum ShootStatus { AVAILABLE_TO_SHOOT, SHOT_PROJECTILE, RECHARGING, RECHARGE_DONE, SEARCHING_PLAYER }
    public enum PlaneRotateStatus { LEFT, RIGHT, IN_PROCESS_OF_TURNING }
    public enum BombStatus { AVAILABLE, SHOT }
    public enum PlayerAction { BOMB_DROP, PLAYER_HIT }

    public class UniversalConstants
    {
        public const string Bomb = "Bomb";
        public const string Player = "Player";
        public const string StatComponent = "StatComponent";
        public const float _gravity = -9.8f;
    }
}