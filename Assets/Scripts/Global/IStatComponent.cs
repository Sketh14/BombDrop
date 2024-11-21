using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontLineDefense.Global
{
    public interface IStatComponent
    {
        void TakeDamage(float damageTaken);
    }
}
