namespace FrontLineDefense.Global
{
    public enum ShootStatus { AVAILABLE_TO_SHOOT, SHOT_PROJECTILE, RECHARGING, RECHARGE_DONE, SEARCHING_PLAYER }

    public class UniversalConstants
    {
        public const string Bomb = "Bomb";
        public const string Player = "Player";
        public const float _gravity = -9.8f;
    }
}