//https://gist.github.com/turtles/6a5a457a4289f0f457a87f1362eacbfc
using Mathf = UnityEngine.Mathf;

namespace FrontLineDefense.Utils
{
    public class MathfHelper
    {

        private const float PI_FLOAT = 3.14159265f;
        private const float PIBY2_FLOAT = 1.5707963f;

        public static float Atan2Approximation1(float y, float x)
        {
            //http://pubs.opengroup.org/onlinepubs/009695399/functions/atan2.html
            //Volkan SALMA

            float ONEQTR_PI = Mathf.PI / 4.0f;
            float THRQTR_PI = 0.75f * Mathf.PI;

            float r, angle;
            float abs_y = Mathf.Abs(y) + 1e-10f; // kludge to prevent 0/0 condition
            if (x < 0.0f)
            {
                r = (x + abs_y) / (abs_y - x);
                angle = THRQTR_PI;
            }
            else
            {
                r = (x - abs_y) / (x + abs_y);
                angle = ONEQTR_PI;
            }
            angle += (0.1963f * r * r - 0.9817f) * r;
            if (y < 0.0f)
                return -angle; // negate if in quad III or IV
            else
                return angle;
        }

        // |error| < 0.005
        public static float Atan2Approximation2(float y, float x)
        {
            if (x == 0.0f)
            {
                if (y > 0.0f) return PIBY2_FLOAT;
                if (y == 0.0f) return 0.0f;
                return -PIBY2_FLOAT;
            }
            float atan;
            float z = y / x;
            if (Mathf.Abs(z) < 1.0f)
            {
                atan = z / (1.0f + 0.28f * z * z);
                if (x < 0.0f)
                {
                    if (y < 0.0f) return atan - PI_FLOAT;
                    return atan + PI_FLOAT;
                }
            }
            else
            {
                atan = PIBY2_FLOAT - z / (z * z + 0.28f);
                if (y < 0.0f) return atan - PI_FLOAT;
            }
            return atan;
        }
    }
}
