namespace BombDrop.Global
{
    public enum ShootStatus { AVAILABLE_TO_SHOOT, SHOT_PROJECTILE, RECHARGING, SEARCHING_PLAYER, FOUND_PLAYER }
    public enum PlaneRotateStatus { LEFT, RIGHT, IN_PROCESS_OF_TURNING }
    public enum BombStatus { AVAILABLE, SHOT }
    public enum PlayerAction { BOMB_DROP, PLAYER_HIT, PLAYER_DEAD, COIN_COLLECTED, INSIDE_BOUNDARY, OUTSIDE_BOUNDARY, OUTSIDE_BOUNDARY_SAFEZONE }
    public enum ButtonClicked { RESTART }
    public enum SceneToLoad { MAIN_MENU, MAIN_GAMEPLAY }
    public enum MissilePointIndex { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT }
    // public enum ProjectileType { BULLET, FOLLOWING_MISSILE, STRAIGHT_MISSILE, PLAYER_BOMB }

    public class UniversalConstants
    {
        public const string Bomb = "Bomb";
        public const string Player = "Player";
        public const string StatComponent = "StatComponent";
        public const string WaterTag = "Water";
        public const float Gravity = -9.8f;
        public const int WaitTimeBeforeCoinCollection = 3000;
    }
}